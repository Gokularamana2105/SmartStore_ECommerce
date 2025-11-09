using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels.AdminViewModels;
using SmartStoreModelsUtility;

namespace SmartStoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Roles.Admin+","+Roles.Editor)]
    
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _uk;
        private readonly IUserService _us;

        public DashboardController(IUnitOfWork uk,IUserService us)
        {
            _uk = uk;
            _us = us;
        }
        public IActionResult Index()
        {

            var totalOrder = _uk.order.Count(x => (x.isValid == true && x.isActive == true) || (x.Status == "Approved"));
            var approvedOrder = _uk.order.Count(x => x.Status == "Approved");
            var users = _uk.user.Count(x=>x.isActive&& x.isValid);
            var products=_uk.prodt.Count(x=>x.isValid==true);

            var Dashboard = new DashboardVM()
            {
                TotalOrders = totalOrder,
                ApprovedOrders = approvedOrder,
                TotalUsers = users,
                TotalProducts = products

            };
            return View(Dashboard);
        }
        [HttpGet]
        public async Task<JsonResult> GetOrderChartData(string fromDate,string toDate)
        {
            try
            {
                string from = fromDate ?? new DateTime(DateTime.Now.Year, 1, 1).ToString("MM-dd-yyyy");
                string to = toDate ?? new DateTime(DateTime.Now.Year, 12, 31).ToString("MM-dd-yyyy");

                var data = await _uk.orderSummary.GetMontlyOrders(from, to);
                return Json(new
                {
                    month = data.Select(x => x.MonthName).ToList(),
                    approved = data.Select(x => x.Approved).ToList(),
                    pending = data.Select(x => x.Pending).ToList(),
                    processing = data.Select(x => x.Processing).ToList()
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { error = ex.Message });
            }
            
        }

        public  IActionResult GetLoginName()
        {
            var userName =  _us.GetUserName();
            return Json(userName);
        }
    }
}
