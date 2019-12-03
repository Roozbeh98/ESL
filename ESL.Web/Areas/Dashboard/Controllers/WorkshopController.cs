using ESL.Common.Plugins;
using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin, Student")]
    public class WorkshopController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        #region Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var _WorkshopPlans = db.Tbl_WorkshopPlan.Where(x => x.WP_IsDelete == false).Select(x => new Model_WorkshopPlan
            {
                ID = x.WP_ID,
                Workshop = x.Tbl_SubWorkshop.Tbl_Workshop.Workshop_Title,
                SubWorkshop = x.Tbl_SubWorkshop.SW_Title,
                Description = x.WP_Description,
                Cost = x.WP_Cost,
                Location = x.WP_Location,
                Activeness = x.WP_IsActive,
                Capacity = x.WP_Capacity,
                Date = x.WP_Date,
                CreationDate = x.WP_CreationDate,
                ModifiedDate = x.WP_ModifiedDate

            }).ToList();

            return View(_WorkshopPlans);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_WorkshopPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_WorkshopPlan _WorkshopPlan = new Tbl_WorkshopPlan
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
                    WP_ModifiedDate = DateTime.Now
                };

                db.Tbl_WorkshopPlan.Add(_WorkshopPlan);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کارگاه جدید با موفقیت ثبت شد";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کارگاه جدید با موفقیت ثبت نشد";

                    return View();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == id).SingleOrDefault();

                if (_WorkshopPlan != null)
                {
                    model.ID = id.Value;
                    model.Name = _WorkshopPlan.WP_Description;
                    model.Description = "آیا از حذف کارگاه مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.ID).SingleOrDefault();

                if (_WorkshopPlan != null)
                {
                    _WorkshopPlan.WP_IsDelete = true;
                    _WorkshopPlan.WP_ModifiedDate = DateTime.Now;

                    db.Entry(_WorkshopPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کارگاه مورد نظر با موفقیت حذف شد";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کارگاه مورد نظر با موفقیت حذف نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_WorkshopPlan.Any(x => x.WP_ID == id))
            {
                var _UserWorkshopPlans = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_WPID == id).Select(x => new Model_UserWorkshopPlan
                {
                    ID = x.UWP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Presence = x.UWP_IsPresent,
                    Activeness = x.UWP_IsActive,
                    CreationDate = x.UWP_CreationDate

                }).ToList();

                ViewBag.WorkshopID = id;

                return View(_UserWorkshopPlans);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == id).SingleOrDefault();

            if (_WorkshopPlan != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = _WorkshopPlan.WP_IsActive
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetActiveness(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.ID).SingleOrDefault();

                if (_WorkshopPlan != null)
                {
                    _WorkshopPlan.WP_IsActive = model.Activeness;
                    _WorkshopPlan.WP_ModifiedDate = DateTime.Now;

                    db.Entry(_WorkshopPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام شد";

                        return RedirectToAction("Index", "Workshop", new { area = "Dashboard" });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Student

        [Authorize(Roles = "Student")]
        public ActionResult List()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _UserWorkshopPlans = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_IsDelete == false && x.UWP_IsActive == true && x.UWP_UserID == _User.User_ID).Select(x => new Model_UserWorkshopPlans
                {
                    ID = x.UWP_ID,
                    User = _User.User_FirstName + " " + _User.User_lastName,
                    Workshop = x.Tbl_WorkshopPlan.Tbl_SubWorkshop.Tbl_Workshop.Workshop_Title,
                    SubWorkshop = x.Tbl_WorkshopPlan.Tbl_SubWorkshop.SW_Title,
                    Location = x.Tbl_WorkshopPlan.WP_Location,
                    Date = x.Tbl_WorkshopPlan.WP_Date,
                    Cost = x.Tbl_WorkshopPlan.WP_Cost,
                    CreationDate = x.UWP_CreationDate,

                }).ToList();

                return View(_UserWorkshopPlans);
            }

            return HttpNotFound();
        }

        #endregion

        #region Functions

        [Authorize(Roles = "Admin")]
        public JsonResult Get_WorkshopList(string searchTerm)
        {
            var _Workshops = db.Tbl_Workshop.ToList();

            if (searchTerm != null)
            {
                _Workshops = db.Tbl_Workshop.Where(a => a.Workshop_Title.Contains(searchTerm)).ToList();
            }

            var md = _Workshops.Select(a => new { id = a.Workshop_ID, text = a.Workshop_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Get_SubWorkshopList(string WorkshopID)
        {
            var _Workshop = db.Tbl_Workshop.Where(a => a.Workshop_Guid.ToString() == WorkshopID).SingleOrDefault();

            if (_Workshop != null)
            {
                var _SubWorkshops = db.Tbl_SubWorkshop.Where(a => a.Tbl_Workshop.Workshop_ID == _Workshop.Workshop_ID).ToList();
                var md = _SubWorkshops.Select(a => new { id = a.SW_Guid, text = a.SW_Title });

                return Json(md, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
