using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModels.Models.CustomerModels;
using SmartStoreModels.Models.CustomerModels.CustomerViewModels;
using SmartStoreModelsUtility;
using Stripe;
using System.Collections.Generic;
using System.Linq;

namespace SmartStoreProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles =Roles.Customer+","+Roles.Admin+","+Roles.Editor)]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uk;
        private readonly IWebHostEnvironment _web;
        private readonly IUserService _us;
        private readonly UserManager<IdentityUser> _ui;
        private readonly AppDbContext _db;

        public CartController(IUnitOfWork uk,IWebHostEnvironment web, IUserService us,UserManager<IdentityUser>ui,AppDbContext db)
        {
            _uk = uk;
            _web = web;
            _us = us;
            _ui = ui;
            _db= db;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _ui.GetUserAsync(User);
            var manageRole = new[] { Roles.Admin, Roles.Editor };
            List<Cart> cart = new List<Cart>();
            if (User.IsInRole(Roles.Customer))
            {
                 cart = await _uk.cart.GetCartById(user?.Id);
                return View(cart);
            }
            if(manageRole.Any(role=>User.IsInRole(role)))
            {
                cart = await _uk.cart.GetAllCart();
                return View(cart);
            }
           return View();
        }



        public async Task<IActionResult> Insert(Cart cart)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userId=_us.GetUserId();
                    string userName=_us.GetUserName();

                    string rootpath = _web.WebRootPath;
                    cart.CreatedOn = DateTime.Now;
                    cart.CreatedBy= userId;
                    //cart.UpdatedOn= DateTime.Now;
                    //cart.UpdatedBy= userId;
                    if (!string.IsNullOrEmpty(cart.FormImage) )
                    {

                       
                        string productPath = Path.Combine(rootpath, "images","Product");
                        string sourcePath=Directory.GetFiles(productPath,"*"+cart.FormImage).FirstOrDefault();
                        if(sourcePath != null)
                        {
                            string fileName = Guid.NewGuid().ToString() + "_" + cart.FormImage;
                            string folderPath = Path.Combine(rootpath, @"images\Cart");
                            string filePath = Path.Combine(folderPath, fileName);

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            System.IO.File.Copy(sourcePath, filePath, true);
                            cart.ProductImage = @"\images\Cart\" + fileName;
                        }
                      
                     
                        cart.TotalAmout = cart.Price * cart.count;
                        cart.isValid = true;
                        cart.isApproved = false;
                    }

                    await _uk.cart.Add(cart);
                    await _uk.Save();
                    return Json(new { success = true,message="Added To Cart", redirectUrl = Url.Action("Index", "Home") });
                }
                return Json(new { success = false, message = "Error on Data" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            await _uk.cart.RemoveCart(id);
            await _uk.Save();
            return Json(new {success=true,message="Product Deleted Successfully"});
        }

        [HttpPost]
        public async Task<IActionResult> Increase_DecreaseCount(Guid id, int count, decimal price)
        {
          
            var cart = new Cart()
            {
                Id = id,
                count = count,
                Price = price
            };
            await _uk.cart.Update(cart);
            await _uk.Save();
            return Json(new { success = true, newcount = count });
        }

        [HttpPost]
        public async Task<IActionResult> InsertSummary(string summaries)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<Summary>>(summaries);
                    string rootpath = _web.WebRootPath;
                    string userId = _us.GetUserId();
                    var user = _us.GetUserInfo(userId);

                await _uk.summary.RemoveAllSummary(userId);
                await _uk.Save();
                foreach (var summary in list)
                   {
                        summary.CreatedBy = userId;
                        summary.UpdatedBy = userId;
                        summary.CreatedOn = DateTime.Now;
                        summary.UserName = user?.Name;
                        summary.City = user?.City;
                        summary.Phone = user?.PhoneNumber;
                        summary.Address = user?.Street;
                        summary.Total = summary.count * summary.Price;
                        summary.isValid = true;
                        summary.isActive = true;

                        if (!string.IsNullOrEmpty(summary.FormImage))
                        {
                            string productPath = Path.Combine(rootpath, "images", "Product");
                            string sourcePath = Directory.GetFiles(productPath, "*" + summary.FormImage).FirstOrDefault();
                            if (sourcePath != null)
                            {
                                string fileName = Guid.NewGuid().ToString() + "_" + summary.FormImage;
                                string folderPath = Path.Combine(rootpath, @"images/summary");
                                string combinePath = Path.Combine(folderPath, fileName);
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                System.IO.File.Copy(sourcePath, combinePath, true);
                                summary.ProductImage = @"/images/summary/" + fileName;

                                await _uk.summary.Add(summary);
                            }
                    
                              
                        }
                   }

                await _uk.Save();


                int lastOrderNumber = 1000;

                if (_db.orders.Any(x => x.UserId == userId))
                {
                    //var users = _db.orders.FirstOrDefault(x=>x.UserId==userId);
                    //if (users != null)
                    //{
                    //    await _uk.order.Remove(users);
                    //    await _uk.Save();
                    //}

                    await _uk.order.RemoveActive(userId);
                    await _uk.Save();
                }
                var lastOrder = _db.orders.OrderByDescending(o => o.OrderNumber).FirstOrDefault();
                if (lastOrder != null)
                {
                    lastOrderNumber = lastOrder.OrderNumber;
                }
                int newOrderNumber = lastOrderNumber + 1;

                var orders = new Orders
                {
                    OrderNumber = newOrderNumber,
                    UserId=userId,
                    UserName = user?.Name,
                    City = user.City,
                    PhoneNumber = user.PhoneNumber,
                    
                    Total = list.Sum(p => p.Price * p.count),
                    Status = "Pending",
                    isValid = true,
                    isActive = true,
                    OrderedDate=DateTime.Now,
                };
                await _uk.order.Add(orders);
                await _uk.Save();
                return Json(new { success = true, redirectUrl = Url.Action("Summary", "Cart") });

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Summary()
        {
            var userId = _us.GetUserId();
            var summaries= await _uk.summary.GetSummaryById(userId);
            var first=summaries.FirstOrDefault();
            var orders=await _uk.order.GetOrderById(userId);
            
            if (orders == null)
            {
                return NotFound();
            }

            var userDetails = new UserInfo()
            {
                Name = first?.UserName,
                PhoneNumber = first?.Phone,
                City = first?.City,
                Street=first?.Address
            };
            var orderDetail = new Orders()
            {
                UserName = orders.UserName,
                City = orders.City,
                OrderNumber = orders.OrderNumber,
                PhoneNumber = orders.PhoneNumber,
                OrderedDate = orders.OrderedDate,
                ShippingDate = orders.ShippingDate,
                Status = orders.Status,
                Total = orders.Total,
                Id = orders.Id,
                UserId = orders.UserId

            };
            var summaryVm = new SummaryDetailsVM()
            {
                users= userDetails,
                summary=summaries,
                orders=orderDetail
            };
          
            ViewBag.PublicKey = "pk_test_51SAb0s65G9FHzEWhi8BZ94rUYr05ULZZoS94cIKkvHPG0hm20aXo0EkvV65ImTPKOabwHW6or9fxfqQIIX5pcGij006EVBL6vp";
            return View(summaryVm);
        }
      
    }
}
