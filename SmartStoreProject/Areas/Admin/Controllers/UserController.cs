using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModelsUtility;

namespace SmartStoreProject.Areas.Admin.Controllers
{
    [Authorize(Roles =Roles.Admin+","+Roles.Editor)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uk;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        public UserController(IUnitOfWork uk, ILogger<UserController> logger,SignInManager<IdentityUser> sign)
        {
            _uk = uk;
            _logger = logger;
            _signInManager = sign;
        }
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> app=await _uk.user.GetAllUser();
            return View(app);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid User Id" });
            }

            await _uk.user.RemoveUser(id);
            await _uk.Save();
            return Json(new { success = true, message = "User Deleted Successfully" });
        }
        public async Task<IActionResult> Include(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid User Id" });
            }

            await _uk.user.IncludeUser(id);
            await _uk.Save();
            return Json(new { success = true, message = "User Include Successfully" });
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid User Id" });
            }

            await _uk.user.RemoveActive(id);
            await _uk.Save();
            return Json(new { success = true, message = "User Removed Successfully" });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
