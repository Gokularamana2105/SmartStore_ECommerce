using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartStoreData.IRepositories;
using SmartStoreModels.Models.AdminModels;
using SmartStoreModels.Models.AdminModels.AdminViewModels;
using SmartStoreModels.Models.CommonMethods;
using SmartStoreModelsUtility;

namespace SmartStoreProject.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uk;
        private readonly IWebHostEnvironment _web;
        public ProductController(IUnitOfWork uk,IWebHostEnvironment web)
        {
            _uk= uk;    
            _web= web;
        }
        [Route("/Admin/Product")]
        public async Task<IActionResult> Index()
        {
            List<Product> prdt=await _uk.prodt.GetAllProduct();
            var result =await _uk.cateogry.Query();
            IEnumerable<SelectListItem> category = result.Select(x=>new SelectListItem
            {
                Text=x.Name,
                Value=x.Id.ToString(),
            });
            ProductVM prodtVM = new ProductVM()
            {
                products = prdt,
                categories = category,
                prd = new Product()
            };
            return View(prodtVM);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            if (id != Guid.Empty)
            {
                Product prodt=await _uk.prodt.GetProductById(id);
                var result =await _uk.cateogry.Query();
               
                ProductVM prodts = new ProductVM()
                {
                    prd = prodt,
                    categories = result.Select(x => new SelectListItem
                    {
                        Text=x.Name,
                        Value=x.Id.ToString(),
                    })
                };
                return Json(prodts);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM prodt)
        {
            if(ModelState.IsValid)
            {
                string rootpath=_web.WebRootPath;

                if (prodt.prd.FormFileImage != null)
                {
                    string fileExtension = Guid.NewGuid().ToString() + "_" + prodt.prd.FormFileImage.FileName;
                    string folderPath = Path.Combine(rootpath, @"images\Product");
                    string filePath=Path.Combine(folderPath, fileExtension);
                    if (prodt.prd.ProductImage!= null)
                    {
                        var oldImage=Path.Combine(rootpath,prodt.prd.ProductImage.Trim('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                       
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await prodt.prd.FormFileImage.CopyToAsync(fileStream);
                    }

                    prodt.prd.ProductImage = @"\images\Product\" + fileExtension;
                }
                else
                {
                    var existingProdt = await _uk.prodt.FindById(prodt.prd.Id);
                    if (existingProdt != null)
                    {
                        prodt.prd.ProductImage= existingProdt.ProductImage;
                    }
                }
                bool isNew = false;
                if(prodt.prd.Id==Guid.Empty)
                {
                    isNew=true;
                    await _uk.prodt.Add(prodt.prd);
                }
                else
                {
                    await _uk.prodt.Update(prodt.prd);
                }
                await _uk.Save();
                return Json(new { success = true, message = isNew==true?Messages.ProductMethod("Create"):Messages.ProductMethod("Update") });
            }
            return Json(new {success=false,message="Error on data"});
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            else
            {
                await _uk.prodt.RemoveProduct(id);
                await _uk.Save();
                return Json(new { success = true,message=Messages.ProductMethod("Delete") });
            }
        }
    }
}
