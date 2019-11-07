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
    [Authorize(Roles = "Admin")]
    public class ExamInPersonController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_IsDelete == false).Select(x => new Model_ExamsInPerson
            {
                ID = x.EIP_ID,
                Exam = x.Tbl_SubExam.SE_Title,
                Description = x.EIP_Description,
                Cost = x.EIP_Cost,
                Location = x.EIP_Location,
                Activeness = x.EIP_IsActive,
                Capacity = x.EIP_Capacity,
                TotalMark = x.EIP_TotalMark,
                PassMark = x.EIP_PassMark,
                Date = x.EIP_Date,
                CreationDate = x.EIP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ExamInPersonCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPerson q = new Tbl_ExamInPerson
                {
                    EIP_Guid = Guid.NewGuid(),
                    EIP_SEID = db.Tbl_SubExam.Where(x => x.SE_Guid.ToString() == model.SubExam).SingleOrDefault().SE_ID,
                    EIP_Description = model.Description,
                    EIP_Cost = model.Cost,
                    EIP_Location = model.Location,
                    EIP_Capacity = model.Capacity,
                    EIP_TotalMark = model.TotalMark,
                    EIP_PassMark = model.PassMark,
                    EIP_Date = DateConverter.ToGeorgianDateTime(model.Date),
                    EIP_IsActive = model.Activeness,
                    EIP_CreationDate = DateTime.Now,
                    EIP_ModifiedDate = DateTime.Now,
                };

                db.Tbl_ExamInPerson.Add(q);

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

        public ActionResult Edit(int id)
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_ExamInPersonEdit model = new Model_ExamInPersonEdit()
                {
                    ID = q.EIP_ID,
                    Exam = q.Tbl_SubExam.Tbl_Exam.Exam_Guid.ToString(),
                    SubExam = q.Tbl_SubExam.SE_Guid.ToString(),
                    Description = q.EIP_Description,
                    Cost = q.EIP_Cost,
                    Location = q.EIP_Location,
                    Capacity = q.EIP_Capacity,
                    TotalMark = q.EIP_TotalMark,
                    PassMark = q.EIP_PassMark,
                    Date = q.EIP_Date
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model_ExamInPersonEdit model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPerson q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIP_Description = model.Description;
                    q.EIP_Cost = model.Cost;
                    q.EIP_Capacity = model.Capacity;
                    q.EIP_TotalMark = model.TotalMark;
                    q.EIP_PassMark = model.PassMark;

                    q.EIP_ModifiedDate = DateTime.Now;

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

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.EIP_Description;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIP_IsDelete = true;

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

        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_ExamInPerson.Any(x => x.EIP_ID == id))
            {
                var q = db.Tbl_UserExamInPerson.Where(x => x.UEIP_EIPID == id).Select(x => new Model_UsersExamInPerson
                {
                    ID = x.UEIP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    SeatNumber = x.UEIP_SeatNumber,
                    Mark = x.UEIP_Mark,
                    IsPresent = x.UEIP_IsPresent,
                    CreationDate = x.UEIP_CreationDate,
                    IsDelete = x.UEIP_IsDelete

                }).ToList();

                ViewBag.ExamID = id;

                return View(q);
            }

            return HttpNotFound();
        }

        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.EIP_IsActive
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetActiveness(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIP_IsActive = model.Activeness;

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

        public JsonResult Get_ExamList(string searchTerm)
        {
            var q = db.Tbl_Exam.ToList();

            if (searchTerm != null)
            {
                q = db.Tbl_Exam.Where(a => a.Exam_Title.Contains(searchTerm)).ToList();
            }

            var md = q.Select(a => new { id = a.Exam_ID, text = a.Exam_Title });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_SubExamList(string ExamID)
        {
            var q = db.Tbl_Exam.Where(a => a.Exam_Guid.ToString() == ExamID).SingleOrDefault();

            var t = db.Tbl_SubExam.Where(a => a.Tbl_Exam.Exam_ID == q.Exam_ID).ToList();
            var md = t.Select(a => new { id = a.SE_Guid, text = a.SE_Title });

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