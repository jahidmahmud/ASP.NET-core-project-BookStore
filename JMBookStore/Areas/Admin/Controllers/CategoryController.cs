using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    unitOfWork.Category.Add(category);
                }
                else
                {
                    unitOfWork.Category.Update(category);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = unitOfWork.Category.GetAll();
            if (result != null)
            {
                return Json(new { data = result });
            }
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var cat = unitOfWork.Category.Get(id);
            if (cat == null)
            {
                return Json(new { success = false, message = "Cannot delete" });
            }
            unitOfWork.Category.Remove(cat);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
