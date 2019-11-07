using ESL.Common.Plugins;
using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class ClassController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false).Select(x => new Model_Class
            {
                ID = x.CP_ID,
                Class = x.Tbl_Class.Class_Title,
                Description = x.CP_Description,
                Cost = x.CP_Cost,
                Location = x.CP_Location,
                Activeness = x.CP_IsActive,
                Capacity = x.CP_Capacity,
                Time = x.CP_Time,
                SessionsNum = x.CP_Capacity,
                SessionsLength = x.CP_Capacity,
                ExamDate = x.CP_ExamDate,
                CreationDate = x.CP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ClassCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ClassPlan q = new Tbl_ClassPlan
                {
                    CP_Guid = Guid.NewGuid(),
                    CP_ClassID = db.Tbl_Class.Where(x => x.Class_Guid.ToString() == model.Class).SingleOrDefault().Class_ID,
                    CP_Description = model.Description,
                    CP_Cost = model.Cost,
                    CP_Location = model.Location,
                    CP_Capacity = model.Capacity,
                    CP_Time = model.Time,
                    CP_SessionsNum = model.SessionsNum,
                    CP_SessionsLength = model.SessionsLength,
                    CP_ExamDate = DateConverter.ToGeorgianDateTime(model.ExamDate),
                    CP_IsActive = model.Activeness,
                    CP_CreationDate = DateTime.Now,
                    CP_ModifiedDate = DateTime.Now,
                };

                db.Tbl_ClassPlan.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                    return View();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        

    }
}