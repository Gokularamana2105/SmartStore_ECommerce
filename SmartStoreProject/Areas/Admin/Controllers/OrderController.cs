using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CustomerModels;
using SmartStoreModels.Models.CustomerModels.CustomerViewModels;
using SmartStoreModelsUtility;

namespace SmartStoreProject.Areas.Admin.Controllers
{
    [Authorize(Roles =Roles.Admin+","+Roles.Editor)]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _uk;

        public OrderController(IUnitOfWork uk)
        {
            _uk = uk;
        }
        public async Task<IActionResult> Index()
        {
            var orders=await _uk.order.QueryAsync(x=>(x.isValid==true && x.isActive==true) || (x.Status=="Approved"));
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderDetail = await _uk.order.FindId(id);
            List<Summary> summaryDetail=null;
            if (orderDetail != null)
            {
                 summaryDetail =await _uk.summary.GetAllByAsync(x=>x.CreatedBy== orderDetail.UserId && (x.isValid && x.isActive));

            }
            var orderVM = new OrderDetailVM()
            {
                summary = summaryDetail,
                order=orderDetail,
            };
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            if(Id<=0)
            {
                return NotFound();
            }
            else
            {
                var orderId=await _uk.order.FindId(Id);
                //string summaryId=await _uk.summary.GetSummaryById
               await _uk.order.RemoveOrder(Id);
                await _uk.Save();
                return Json(new { success = true,message="Order Removed", redirectUrl = Url.Action("Index","Order") });
            }
        }
    }
}
