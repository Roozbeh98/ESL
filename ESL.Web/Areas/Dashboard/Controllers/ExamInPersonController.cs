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
    public class ExamInPersonController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        #region Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var _ExamsInPersonPlans = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_IsDelete == false).Select(x => new Model_ExamsInPersonPlan
            {
                ID = x.EIPP_ID,
                Exam = x.Tbl_SubExamInPerson.Tbl_ExamInPerson.EIP_Title,
                SubExam = x.Tbl_SubExamInPerson.SEIP_Title,
                Description = x.EIPP_Description,
                Cost = x.EIPP_Cost,
                Location = x.EIPP_Location,
                Activeness = x.EIPP_IsActive,
                Capacity = x.EIPP_Capacity,
                TotalMark = x.EIPP_TotalMark,
                PassMark = x.EIPP_PassMark,
                Date = x.EIPP_Date,
                CreationDate = x.EIPP_CreationDate,
                ModifiedDate = x.EIPP_ModifiedDate
                
            }).ToList();

            return View(_ExamsInPersonPlans);
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ExamInPersonPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPersonPlan _ExamInPersonPlan = new Tbl_ExamInPersonPlan
                {
                    EIPP_Guid = Guid.NewGuid(),
                    EIPP_SEIPID = db.Tbl_SubExamInPerson.Where(x => x.SEIP_Guid.ToString() == model.SubExam).SingleOrDefault().SEIP_ID,
                    EIPP_Description = model.Description,
                    EIPP_Cost = model.Cost,
                    EIPP_Location = model.Location,
                    EIPP_Capacity = model.Capacity,
                    EIPP_TotalMark = model.TotalMark,
                    EIPP_PassMark = model.PassMark,
                    EIPP_Date = DateConverter.ToGeorgianDateTime(model.Date),
                    EIPP_IsActive = model.Activeness,
                    EIPP_CreationDate = DateTime.Now,
                    EIPP_ModifiedDate = DateTime.Now,
                };

                db.Tbl_ExamInPersonPlan.Add(_ExamInPersonPlan);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "آزمون جدید با موفقیت ثبت شد";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "آزمون جدید با موفقیت ثبت نشد";

                    return View();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //[Authorize(Roles = "Admin")]
        //public ActionResult Edit(int id)
        //{
        //    var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

        //    if (_ExamInPersonPlan != null)
        //    {
        //        Model_ExamInPersonPlanEdit model = new Model_ExamInPersonPlanEdit()
        //        {
        //            ID = _ExamInPersonPlan.EIPP_ID,
        //            Exam = _ExamInPersonPlan.Tbl_SubExamInPerson.Tbl_ExamInPerson.EIP_Guid.ToString(),
        //            SubExam = _ExamInPersonPlan.Tbl_SubExamInPerson.SEIP_Guid.ToString(),
        //            Description = _ExamInPersonPlan.EIPP_Description,
        //            Cost = _ExamInPersonPlan.EIPP_Cost,
        //            Location = _ExamInPersonPlan.EIPP_Location,
        //            Capacity = _ExamInPersonPlan.EIPP_Capacity,
        //            TotalMark = _ExamInPersonPlan.EIPP_TotalMark,
        //            PassMark = _ExamInPersonPlan.EIPP_PassMark,
        //            Activeness = _ExamInPersonPlan.EIPP_IsActive,
        //            Date = _ExamInPersonPlan.EIPP_Date
        //        };

        //        return View(model);
        //    }

        //    return HttpNotFound();
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Model_ExamInPersonPlanEdit model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Tbl_ExamInPersonPlan _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

        //        if (_ExamInPersonPlan != null)
        //        {
        //            _ExamInPersonPlan.EIPP_Description = model.Description;
        //            _ExamInPersonPlan.EIPP_Cost = model.Cost;
        //            _ExamInPersonPlan.EIPP_Capacity = model.Capacity;
        //            _ExamInPersonPlan.EIPP_TotalMark = model.TotalMark;
        //            _ExamInPersonPlan.EIPP_PassMark = model.PassMark;
        //            _ExamInPersonPlan.EIPP_IsActive = model.Activeness;

        //            _ExamInPersonPlan.EIPP_ModifiedDate = DateTime.Now;

        //            db.Entry(_ExamInPersonPlan).State = EntityState.Modified;

        //            if (Convert.ToBoolean(db.SaveChanges() > 0))
        //            {
        //                TempData["TosterState"] = "success";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "آزمون مورد نظر با موفقیت ویرایش شد";

        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                TempData["TosterState"] = "error";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "آزمون مورد نظر با موفقیت ویرایش نشد";

        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }

        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

                if (_ExamInPersonPlan != null)
                {
                    model.ID = id.Value;
                    model.Name = _ExamInPersonPlan.EIPP_Description;
                    model.Description = "آیا از حذف آزمون مورد نظر اطمینان دارید ؟";

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
                var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                if (_ExamInPersonPlan != null)
                {
                    _ExamInPersonPlan.EIPP_IsDelete = true;
                    _ExamInPersonPlan.EIPP_ModifiedDate = DateTime.Now;

                    db.Entry(_ExamInPersonPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "آزمون مورد نظر با موفقیت حذف شد";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "آزمون مورد نظر با موفقیت حذف نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_ExamInPersonPlan.Any(x => x.EIPP_ID == id))
            {
                var _UsersExamInPersonPlans = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_EIPPID == id).Select(x => new Model_UsersExamInPersonPlan
                {
                    ID = x.UEIPP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    SeatNumber = x.UEIPP_SeatNumber,
                    Mark = x.UEIPP_Mark,
                    Presence = x.UEIPP_IsPresent,
                    Activeness = x.UEIPP_IsActive,
                    CreationDate = x.UEIPP_CreationDate

                }).ToList();

                ViewBag.ExamID = id;

                return View(_UsersExamInPersonPlans);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

            if (_ExamInPersonPlan != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = _ExamInPersonPlan.EIPP_IsActive
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
                var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                if (_ExamInPersonPlan != null)
                {
                    _ExamInPersonPlan.EIPP_IsActive = model.Activeness;
                    _ExamInPersonPlan.EIPP_ModifiedDate = DateTime.Now;

                    db.Entry(_ExamInPersonPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام شد";

                        return RedirectToAction("Index", "ExamInPerson", new { area = "Dashboard" });
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
                var _ExamsInPersonPlans = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_IsDelete == false && x.UEIPP_IsActive == true && x.UEIPP_UserID == _User.User_ID).Select(x => new Model_UserExamInPersonPlans
                {
                    ID = x.UEIPP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Exam = x.Tbl_ExamInPersonPlan.Tbl_SubExamInPerson.Tbl_ExamInPerson.EIP_Title,
                    SubExam = x.Tbl_ExamInPersonPlan.Tbl_SubExamInPerson.SEIP_Title,
                    SeatNumber = x.UEIPP_SeatNumber,
                    Mark = x.UEIPP_Mark,
                    Presence = x.UEIPP_IsPresent,
                    Location = x.Tbl_ExamInPersonPlan.EIPP_Location,
                    Date = x.Tbl_ExamInPersonPlan.EIPP_Date,
                    Cost = x.Tbl_ExamInPersonPlan.EIPP_Cost,
                    CreationDate = x.UEIPP_CreationDate

                }).ToList();

                return View(_ExamsInPersonPlans);
            }

            return HttpNotFound();
        }

        #endregion

        #region Functions

        [Authorize(Roles = "Admin")]
        public JsonResult Get_ExamList(string searchTerm)
        {
            var _ExamsInPerson = db.Tbl_ExamInPerson.ToList();

            if (searchTerm != null)
            {
                _ExamsInPerson = db.Tbl_ExamInPerson.Where(a => a.EIP_Title.Contains(searchTerm)).ToList();
            }

            var md = _ExamsInPerson.Select(a => new { id = a.EIP_ID, text = a.EIP_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Get_SubExamList(string ExamID)
        {
            var _ExamsInPerson = db.Tbl_ExamInPerson.Where(a => a.EIP_Guid.ToString() == ExamID).SingleOrDefault();

            if (_ExamsInPerson != null)
            {
                var _SubExamsInPerson = db.Tbl_SubExamInPerson.Where(a => a.Tbl_ExamInPerson.EIP_ID == _ExamsInPerson.EIP_ID).ToList();
                var md = _SubExamsInPerson.Select(a => new { id = a.SEIP_Guid, text = a.SEIP_Title });

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
