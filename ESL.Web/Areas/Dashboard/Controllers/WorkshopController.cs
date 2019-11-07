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
    public class WorkshopController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_WorkshopPlan.Where(x => x.WP_IsDelete == false).Select(x => new Model_Workshop
            {
                ID = x.WP_ID,
                Workshop = x.Tbl_SubWorkshop.SW_Title,
                Description = x.WP_Description,
                Cost = x.WP_Cost,
                Location = x.WP_Location,
                Activeness = x.WP_IsActive,
                Capacity = x.WP_Capacity,
                Date = x.WP_Date,
                CreationDate = x.WP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_WorkshopCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_WorkshopPlan q = new Tbl_WorkshopPlan
                {
                    WP_Guid = Guid.NewGuid(),
                    WP_SWID = db.Tbl_SubWorkshop.Where(x => x.SW_Guid.ToString() == model.SubWorkshop).SingleOrDefault().SW_ID,
                    WP_Description = model.Description,
                    WP_Cost = model.Cost,
                    WP_Location = model.Location,
                    WP_Capacity = model.Capacity,
                    WP_Date = DateConverter.ToGeorgianDateTime(model.Date),
                    WP_IsActive = model.Activeness,
                    WP_CreationDate = DateTime.Now,
                    WP_ModifiedDate = DateTime.Now,
                };

                db.Tbl_WorkshopPlan.Add(q);

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

        public JsonResult Get_WorkshopList(string searchTerm)
        {
            var q = db.Tbl_Workshop.ToList();

            if (searchTerm != null)
            {
                q = db.Tbl_Workshop.Where(a => a.Workshop_Title.Contains(searchTerm)).ToList();
            }

            var md = q.Select(a => new { id = a.Workshop_ID, text = a.Workshop_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_SubWorkshopList(string WorkshopID)
        {
            var q = db.Tbl_Workshop.Where(a => a.Workshop_Guid.ToString() == WorkshopID).SingleOrDefault();

            var t = db.Tbl_SubWorkshop.Where(a => a.Tbl_Workshop.Workshop_ID == q.Workshop_ID).ToList();
            var md = t.Select(a => new { id = a.SW_Guid, text = a.SW_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

    }
}