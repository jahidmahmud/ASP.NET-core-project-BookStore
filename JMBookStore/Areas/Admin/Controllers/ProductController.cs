using Dapper;
using JMBookStore.Models;
using JMBookStore.Models.ViewModels;
using JMBookStore.Repositories.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProductController(IUnitOfWork _unitOfWork, IWebHostEnvironment _hostEnvironment)
        {
            unitOfWork = _unitOfWork;
            hostEnvironment = _hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryList = unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypeList = unitOfWork.Cover.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(productViewModel);
            }
            productViewModel.Product = unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productViewModel.Product == null)
            {
                return NoContent();
            }
            return View(productViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                string rootPath = hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (productViewModel.Product.ImageUrl != null)
                    {
                        var imagepath = Path.Combine(rootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                    using(var filestreams=new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestreams);
                    }
                    productViewModel.Product.ImageUrl = @"\images\products\"+filename+extension;
                }
                else
                {
                    if (productViewModel.Product.Id != 0)
                    {
                        Product obj = unitOfWork.Product.Get(productViewModel.Product.Id);
                        productViewModel.Product.ImageUrl = obj.ImageUrl;
                    }
                }
                if (productViewModel.Product.Id == 0)
                {
                    unitOfWork.Product.Add(productViewModel.Product);
                }
                else
                {
                    unitOfWork.Product.Update(productViewModel.Product);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var obj = unitOfWork.Product.GetAll();
            return Json(new { data = obj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            var product = unitOfWork.Product.Get(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Unable to delete" });
            }
            else
            {
                string rootPath = hostEnvironment.WebRootPath;
                var imagepath = Path.Combine(rootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                unitOfWork.Product.Remove(product);
                unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
        }
        #endregion

    }
}
