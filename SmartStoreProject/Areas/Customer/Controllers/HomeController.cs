using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models;
using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.CustomerModels.CustomerViewModels;
using SmartStoreModels.Models.CustomerModels;
using Microsoft.AspNetCore.Authorization;
using SmartStoreModelsUtility;
using Microsoft.AspNetCore.Identity;
namespace SmartStoreProject.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _uk;
        private readonly UserManager<IdentityUser> _ui;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork uk,UserManager<IdentityUser> ui)
        {
            _logger = logger;
            _uk = uk;
            _ui = ui;
        }
        
        public async Task<IActionResult> Index(int? page)
        {
            var prodt=await _uk.prodt.GetAllProduct();
            int pageSize = 6;
            int pageNumer = page ?? 1;
            int totalCount= prodt.Count();
            int totalPages=(int)Math.Ceiling((double)totalCount/pageSize);
            var paginatedPost=prodt.Skip((pageNumer-1)*pageSize).Take(pageSize).ToList();
            ViewBag.TotalPages=totalPages;
            ViewBag.CurrentPages = pageNumer;
            List<Product> products= paginatedPost;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Route("/Customer/Home/Details")]
        [Authorize(Roles = Roles.Customer + "," + Roles.Admin + "," + Roles.Editor)]
        public async Task<IActionResult> Details(Guid id)
        {
            Product product=await _uk.prodt.FindById(id);
            var prodt = new List<Product>();
            if (product != null)
            {
                 prodt= await _uk.prodt.GetAllProduct(product.CategoryId,product.Id);
            }
            HomeDetailsVM hdVM = new HomeDetailsVM()
            {
                product = prodt,
                prodt = product,
                cart = new Cart()
            };
            return View(hdVM);
        }

        public async Task<IActionResult> CartCount()
        {
            var user = await _ui.GetUserAsync(User);
            int count = 0;
            if (User.IsInRole(Roles.Customer))
            {
                count = await _uk.cart.GetCountById(user?.Id);
            }
            var manageRole = new[] { Roles.Admin, Roles.Editor };
            if(manageRole.Any(role=>User.IsInRole(role)))
            {
                count = await _uk.cart.GetCount();

            }
            
            return Json(count);
        }
    }
}
