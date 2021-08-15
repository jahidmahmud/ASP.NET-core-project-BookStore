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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
            {
                return View(company);
            }
            company = unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    unitOfWork.Company.Add(company);
                }
                else
                {
                    unitOfWork.Company.Update(company);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = unitOfWork.Company.GetAll();
            if (result != null)
            {
                return Json(new { data = result });
            }
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var cat = unitOfWork.Company.Get(id);
            if (cat == null)
            {
                return Json(new { success = false, message = "Cannot delete" });
            }
            unitOfWork.Company.Remove(cat);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
