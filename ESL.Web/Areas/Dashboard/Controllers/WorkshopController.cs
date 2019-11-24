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
        private ESLEntities db = new ESLEntities();

        [Authorize(Roles = "Student")]
        public ActionResult List()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_IsDelete == false && x.UWP_UserID == _User.User_ID).Select(x => new Model_UserWorkshopPlans
                {
                    ID = x.UWP_ID,
                    User = _User.User_FirstName + " " + _User.User_lastName,
                    Workshop = x.Tbl_WorkshopPlan.Tbl_SubWorkshop.Tbl_Workshop.Workshop_Title,
                    SubWorkshop = x.Tbl_WorkshopPlan.Tbl_SubWorkshop.SW_Title,
                    Location = x.Tbl_WorkshopPlan.WP_Location,
                    Date = x.Tbl_WorkshopPlan.WP_Date,
                    Cost = x.Tbl_WorkshopPlan.WP_Cost,
                    CreationDate = x.UWP_CreationDate,
                    //RegisterationState = 

                }).ToList();

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult UnRegister(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
                    model.Description = "آیا از لغو ثبت نام اطمینان دارید ؟";

                    return PartialView(model);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnRegister(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UWP_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("List", "Workshop");
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var q = db.Tbl_WorkshopPlan.Where(x => x.WP_IsDelete == false).Select(x => new Model_WorkshopPlan
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

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var q = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.WP_Description;
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
                var q = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.WP_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

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
                var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_WPID == id).Select(x => new Model_UserWorkshopPlan
                {
                    ID = x.UWP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    IsPresent = x.UWP_IsPresent,
                    CreationDate = x.UWP_CreationDate,
                    IsDelete = x.UWP_IsDelete,

                }).ToList();

                ViewBag.WorkshopID = id;

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.WP_IsActive
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
                var q = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.WP_IsActive = model.Activeness;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "Workshop", new { area = "Dashboard" });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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