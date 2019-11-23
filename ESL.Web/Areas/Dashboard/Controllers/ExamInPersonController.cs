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
        private ESLEntities db = new ESLEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_IsDelete == false).Select(x => new Model_ExamsInPersonPlan
            {
                ID = x.EIPP_ID,
                Exam = x.Tbl_SubExamInPerson.SEIP_Title,
                Description = x.EIPP_Description,
                Cost = x.EIPP_Cost,
                Location = x.EIPP_Location,
                Activeness = x.EIPP_IsActive,
                Capacity = x.EIPP_Capacity,
                TotalMark = x.EIPP_TotalMark,
                PassMark = x.EIPP_PassMark,
                Date = x.EIPP_Date,
                CreationDate = x.EIPP_CreationDate

            }).ToList();

            return View(q);
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
                Tbl_ExamInPersonPlan q = new Tbl_ExamInPersonPlan
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

                db.Tbl_ExamInPersonPlan.Add(q);

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
        public ActionResult Edit(int id)
        {
            var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_ExamInPersonPlanEdit model = new Model_ExamInPersonPlanEdit()
                {
                    ID = q.EIPP_ID,
                    Exam = q.Tbl_SubExamInPerson.Tbl_ExamInPerson.EIP_Guid.ToString(),
                    SubExam = q.Tbl_SubExamInPerson.SEIP_Guid.ToString(),
                    Description = q.EIPP_Description,
                    Cost = q.EIPP_Cost,
                    Location = q.EIPP_Location,
                    Capacity = q.EIPP_Capacity,
                    TotalMark = q.EIPP_TotalMark,
                    PassMark = q.EIPP_PassMark,
                    Activeness = q.EIPP_IsActive,
                    Date = q.EIPP_Date
                };

                return View(model);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model_ExamInPersonPlanEdit model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPersonPlan q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIPP_Description = model.Description;
                    q.EIPP_Cost = model.Cost;
                    q.EIPP_Capacity = model.Capacity;
                    q.EIPP_TotalMark = model.TotalMark;
                    q.EIPP_PassMark = model.PassMark;
                    q.EIPP_IsActive = model.Activeness;

                    q.EIPP_ModifiedDate = DateTime.Now;

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

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
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
                
                var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.EIPP_Description;
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
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIPP_IsDelete = true;

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
            if (id.HasValue && db.Tbl_ExamInPersonPlan.Any(x => x.EIPP_ID == id))
            {
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_EIPPID == id).Select(x => new Model_UsersExamInPersonPlan
                {
                    ID = x.UEIPP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    SeatNumber = x.UEIPP_SeatNumber,
                    Mark = x.UEIPP_Mark,
                    IsPresent = x.UEIPP_IsPresent,
                    CreationDate = x.UEIPP_CreationDate,
                    IsDelete = x.UEIPP_IsDelete

                }).ToList();

                ViewBag.ExamID = id;

                return View(q);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.EIPP_IsActive
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
                var q = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIPP_IsActive = model.Activeness;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "ExamInPerson", new { area = "Dashboard" });
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
        public JsonResult Get_ExamList(string searchTerm)
        {
            var q = db.Tbl_ExamInPerson.ToList();

            if (searchTerm != null)
            {
                q = db.Tbl_ExamInPerson.Where(a => a.EIP_Title.Contains(searchTerm)).ToList();
            }

            var md = q.Select(a => new { id = a.EIP_ID, text = a.EIP_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Get_SubExamList(string ExamID)
        {
            var q = db.Tbl_ExamInPerson.Where(a => a.EIP_Guid.ToString() == ExamID).SingleOrDefault();

            var t = db.Tbl_SubExamInPerson.Where(a => a.Tbl_ExamInPerson.EIP_ID == q.EIP_ID).ToList();
            var md = t.Select(a => new { id = a.SEIP_Guid, text = a.SEIP_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

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