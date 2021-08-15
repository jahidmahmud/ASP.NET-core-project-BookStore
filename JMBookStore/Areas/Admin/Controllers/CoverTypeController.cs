using Dapper;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using JMBookStore.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
            {
                return View(coverType);
            }
            //without stored procedure
            //coverType = unitOfWork.Cover.Get(id.GetValueOrDefault());
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            coverType = unitOfWork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);
            if (coverType == null)
            {
                return NoContent();
            }
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", coverType.Name);
                if (coverType.Id == 0)
                {
                    unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Create, parameter);
                    //without stored procedure
                    //unitOfWork.Cover.Add(coverType);
                }
                else
                {
                    parameter.Add("@Id", coverType.Id);
                    unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Update, parameter);
                    //without stored procedure
                    //unitOfWork.Cover.Update(coverType);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //without stored procedure
            //var result = unitOfWork.Cover.GetAll();
            //with stored procedure
            var result = unitOfWork.SP_Call.List<CoverType>(StaticDetails.Proc_CoverType_GetAll, null);
            if (result != null)
            {
                return Json(new { data = result });
            }
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var cover = unitOfWork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);
            //without strd procedure
            //var cover = unitOfWork.Cover.Get(id);
            if (cover == null)
            {
                return Json(new { success = false, message = "Unable to delete" });
            }
            else
            {
                //without strd procedure
                //unitOfWork.Cover.Remove(cover);
                unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Delete, parameter);
                unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
        }
        #endregion
    }
}
