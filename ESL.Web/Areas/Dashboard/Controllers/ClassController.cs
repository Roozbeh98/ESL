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
        private readonly ESLEntities db = new ESLEntities();

        #region Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var _ClassPlans = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false).Select(x => new Model_ClassPlan
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
                SessionsLength = x.CP_SessionsLength,
                StartDate = x.CP_StartDate,
                CreationDate = x.CP_CreationDate,
                ModifiedDate = x.CP_ModifiedDate

            }).ToList();

            return View(_ClassPlans);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ClassPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ClassPlan _ClassPlan = new Tbl_ClassPlan
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

                db.Tbl_ClassPlan.Add(_ClassPlan);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کلاس جدید با موفقیت ثبت شد";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کلاس جدید با موفقیت ثبت نشد";

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

                var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

                if (_ClassPlan != null)
                {
                    model.ID = id.Value;
                    model.Name = _ClassPlan.Tbl_Class.Class_Title;
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
        public ActionResult Delete(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (_ClassPlan != null)
                {
                    _ClassPlan.CP_IsDelete = true;
                    _ClassPlan.CP_ModifiedDate = DateTime.Now;

                    db.Entry(_ClassPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کلاس مورد نظر با موفقیت حذف شد";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کلاس مورد نظر با موفقیت حذف نشد";

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
                var _UserClassPlans = db.Tbl_UserClassPlan.Where(x => x.UCP_CPID == id && x.UCP_IsDelete == false).Select(x => new Model_UserClassPlan
                {
                    ID = x.UCP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    CreationDate = x.UCP_CreationDate,
                    PresenceSessions = x.Tbl_UserClassPlanPresence.Where(xx => xx.UCPP_IsPresent == true).Count(),
                    Activeness = x.UCP_IsActive,

                }).ToList();

                ViewBag.ClassID = id;

                return View(_UserClassPlans);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

            if (_ClassPlan != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = _ClassPlan.CP_IsActive
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
                var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (_ClassPlan != null)
                {
                    _ClassPlan.CP_IsActive = model.Activeness;
                    _ClassPlan.CP_ModifiedDate = DateTime.Now;

                    db.Entry(_ClassPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام شد";

                        return RedirectToAction("Index", "Class", new { area = "Dashboard" });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش موفقیت انجام نشد";

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
                var _UserClassPlans = db.Tbl_UserClassPlan.Where(x => x.UCP_IsDelete == false && x.UCP_IsActive == true && x.UCP_UserID == _User.User_ID).Select(x => new Model_UserClassPlans
                {
                    ID = x.UCP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Class = x.Tbl_ClassPlan.Tbl_Class.Class_Title,
                    Type = x.Tbl_ClassPlan.Tbl_Code.Code_Display,
                    Location = x.Tbl_ClassPlan.CP_Location,
                    Time = x.Tbl_ClassPlan.CP_Time,
                    CreationDate = x.UCP_CreationDate

                }).ToList();

                return View(_UserClassPlans);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Info(int? id)
        {
            if (id.HasValue && db.Tbl_UserClassPlan.Any(x => x.UCP_ID == id))
            {
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == id).SingleOrDefault();

                var _UserClassPlanPresence = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_IsDelete == false && x.Tbl_UserClassPlan.UCP_UserID == _UserClassPlan.UCP_UserID && x.Tbl_UserClassPlan.UCP_CPID == _UserClassPlan.UCP_CPID && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_TypeCodeID == _UserClassPlan.Tbl_ClassPlan.CP_TypeCodeID && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_Location == _UserClassPlan.Tbl_ClassPlan.CP_Location && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_Time == _UserClassPlan.Tbl_ClassPlan.CP_Time).Select(x => new Model_UserClassPlanPresence
                {
                    ID = x.UCPP_ID,
                    Cost = x.Tbl_Payment.Payment_Cost,
                    Discount = x.Tbl_Payment.Payment_Discount,
                    Presence = x.UCPP_IsPresent,
                    Date = x.UCPP_Date

                }).ToList();

                TempData["UserID"] = _UserClassPlan.UCP_UserID;
                TempData["UserClassPlanID"] = id;

                return View(_UserClassPlanPresence);
            }

            return HttpNotFound();
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
