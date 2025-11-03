using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModelsUtility;

namespace SmartStoreProject.Areas.Admin.Controllers
{
  
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uk;

        public CategoryController(IUnitOfWork uk)
        {
            _uk = uk;
        }

        //[Route("/Admin/Category")]
        public async Task<IActionResult> Index()
        {
            List<Category> ct = await _uk.cateogry.GetAll();
            return View(ct);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(Guid id)
        {
           
            if (id == Guid.Empty)
            {
               return NotFound();
            }
            Category ct = await _uk.cateogry.FindById(id);
            if (ct == null)
            {
                return NotFound();
            }
            return Json(ct);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] Category ct)
        {
            if(ModelState.IsValid)
            {
              
                bool isNew=ct.Id == Guid.Empty;
              
                if(isNew)
                {
                    ct.DateCreatedOn = DateTime.Now;
                    await _uk.cateogry.Add(ct);

                }
                else
                {
                   
                    await _uk.cateogry.Update(ct);
                }
                await _uk.Save();
                return Json(new { success = true, message = isNew ? Messages.CategoryMethod("Create") : Messages.CategoryMethod("Update") });
            }

            return Json(new {success=false,message="Error on Category Changes"});

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            Category ct = await _uk.cateogry.FindById(id);
            if (ct == null)
            {
                return NotFound();
            }
            return View(ct);
        }

        [HttpPost]
        public async Task<JsonResult> DeletePost(Guid id)
        {
            if(id == Guid.Empty)
            {
                return Json(new { success = false,message="Invalid Category Id" });
            }

            Category ct = await _uk.cateogry.FindById(id);
            await _uk.cateogry.Remove(ct);
            return Json(new { success = true, message = Messages.CategoryMethod("Delete") });
        }
    }
}
