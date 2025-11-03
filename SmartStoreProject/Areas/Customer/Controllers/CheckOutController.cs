using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CustomerModels;
using Stripe.Checkout;

namespace SmartStoreProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckOutController : Controller
    {
        private readonly IUnitOfWork _uk;
        private readonly IUserService _us;
        private readonly UserManager<IdentityUser> _ui;
        private readonly AppDbContext _db;
        public CheckOutController(IUnitOfWork uk,IUserService us,UserManager<IdentityUser> ui,AppDbContext db)
        {
            _uk = uk;
            _us = us;
            _ui=ui;
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCheckOutSession([FromBody] CheckoutRequest responses)
        {
            var domain = "https://localhost:44300";

            string userId = responses.UserId;
            HttpContext.Session.SetString("UserId", responses.UserId);
            int OrderNumber = (int)responses.OrderNumber;

            var products = responses.Summary;
          
            if (userId != null)
            {
                var orderUpdate = await _uk.order.GetOrderById(userId);
                orderUpdate.Status = "Processing";
                await _uk.order.Update(orderUpdate);
                await _uk.Save();
            }
            var lineItems = products.Select(items => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(items.Price * 100),
                    Currency = "inr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = items.ProductName,
                        
                        Images = new List<string> { domain + items.ProductImage }
                    }
                },
                Quantity = items.count
            }).ToList();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + $"/Customer/CheckOut/OrderConfirmation?orderNumber={OrderNumber}",
                CancelUrl = domain + "/Customer/Cart/Summary",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id, url = session.Url });
        }

        public async Task<IActionResult> OrderConfirmation(int orderNumber)
        {
            ViewBag.OrderNumber = orderNumber;
            string userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                await _uk.cart.ActiveApproved(userId);
                await _uk.Save();
            }
            var orderDetails=await _uk.order.GetOrderNumber(orderNumber);
            if (orderDetails != null)
            {
                orderDetails.Status = "Approved";
                orderDetails.ShippingDate = DateTime.Now.AddDays(10);
                orderDetails.OrderedDate = DateTime.Now;
                await _uk.order.Update(orderDetails);
                await _uk.Save();
            }
            return View();
        }
    }
}
