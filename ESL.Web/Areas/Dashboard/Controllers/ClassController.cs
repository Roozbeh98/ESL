using ESL.Common.Plugins;
using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
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
    public class ClassController : Controller
    {
        private ESLEntities db = new ESLEntities();

        [Authorize(Roles = "Student")]
        public ActionResult List()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var q = db.Tbl_UserClass.Where(x => x.UC_IsDelete == false && x.UC_UserID == _User.User_ID).Select(x => new Model_UserClasses
                {
                    ID = x.UC_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Class = x.Tbl_ClassPlan.Tbl_Class.Class_Title,
                    Type = x.Tbl_ClassPlan.Tbl_Code.Code_Display,
                    Location = x.Tbl_ClassPlan.CP_Location,
                    Time = x.Tbl_ClassPlan.CP_Time,
                    CreationDate = x.UC_CreationDate

                }).ToList();

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Info(int? id)
        {
            if (id.HasValue && db.Tbl_UserClass.Any(x => x.UC_ID == id))
            {
                var p = db.Tbl_UserClass.Where(x => x.UC_ID == id).SingleOrDefault();

                var q = db.Tbl_UserClassPresence.Where(x => x.UCP_IsDelete == false && x.Tbl_UserClass.UC_UserID == p.UC_UserID && x.Tbl_UserClass.UC_CPID == p.UC_CPID && x.Tbl_UserClass.Tbl_ClassPlan.CP_TypeCodeID == p.Tbl_ClassPlan.CP_TypeCodeID && x.Tbl_UserClass.Tbl_ClassPlan.CP_Location == p.Tbl_ClassPlan.CP_Location && x.Tbl_UserClass.Tbl_ClassPlan.CP_Time == p.Tbl_ClassPlan.CP_Time).Select(x => new Model_UserClassPresence
                {
                    ID = x.UCP_ID,
                    Cost = x.Tbl_Payment.Payment_Cost,
                    Discount = x.Tbl_Payment.Payment_Discount,
                    Presence = x.UCP_IsPresent,
                    CreationDate = x.UCP_CreationDate

                }).ToList();

                TempData["UserID"] = p.UC_UserID;
                TempData["UserClassID"] = id;

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var q = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false).Select(x => new Model_Class
            {
                ID = x.CP_ID,
                Class = x.Tbl_Class.Class_Title,
                Type = x.Tbl_Code.Code_Display,
                Description = x.CP_Description,
                CostPerSession = x.CP_CostPerSession,
                Location = x.CP_Location,
                Activeness = x.CP_IsActive,
                Capacity = x.CP_Capacity,
                Time = x.CP_Time,
                SessionsLength = x.CP_Capacity,
                StartDate = x.CP_StartDate,
                CreationDate = x.CP_CreationDate

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
        public ActionResult Create(Model_ClassCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ClassPlan q = new Tbl_ClassPlan
                {
                    CP_Guid = Guid.NewGuid(),
                    CP_ClassID = db.Tbl_Class.Where(x => x.Class_Guid.ToString() == model.Class).SingleOrDefault().Class_ID,
                    CP_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type),
                    CP_Description = model.Description,
                    CP_CostPerSession = model.CostPerSession,
                    CP_Location = model.Location,
                    CP_Capacity = model.Capacity,
                    CP_Time = model.Time,
                    CP_SessionsLength = model.SessionsLength,
                    CP_StartDate = DateConverter.ToGeorgianDateTime(model.StartDate),
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

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.CP_Description;
                    model.Description = "آیا از حذف کلاس مورد نظر اطمینان دارید ؟";

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
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.CP_IsDelete = true;

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
            if (id.HasValue && db.Tbl_ClassPlan.Any(x => x.CP_ID == id))
            {
                var q = db.Tbl_UserClass.Where(x => x.UC_CPID == id).Select(x => new Model_UserClass
                {
                    ID = x.UC_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    CreationDate = x.UC_CreationDate,
                    IsDelete = x.UC_IsDelete,

                }).ToList();

                ViewBag.ClassID = id;

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.CP_IsActive
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
                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.CP_IsActive = model.Activeness;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "Class", new { area = "Dashboard" });
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
    }
}