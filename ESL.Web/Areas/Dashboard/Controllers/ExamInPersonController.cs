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
    public class ExamInPersonController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_IsDelete == false).Select(x => new Model_ExamsInPerson
            {
                ID = x.EIP_ID,
                Title = x.EIP_Title,
                Cost = x.EIP_Cost,
                Capacity = x.EIP_Capacity,
                TotalMark = x.EIP_TotalMark,
                PassMark = x.EIP_PassMark,
                CreationDate = x.EIP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ExamInPersonCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPerson q = new Tbl_ExamInPerson
                {
                    EIP_Title = model.Title,
                    EIP_Cost = model.Cost,
                    EIP_Capacity = model.Capacity,
                    EIP_TotalMark = model.TotalMark,
                    EIP_PassMark = model.PassMark
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

        //public ActionResult Edit(int id)
        //{
        //    Model_Exam model = new Model_Exam();

        //    if (id != null)
        //    {
        //        var exam = db.Tbl_Exam.Where(x => x.Exam_ID == id).FirstOrDefault();

        //        if (exam != null)
        //        {
        //            model.ID = exam.Exam_ID;
        //            model.Title = exam.Exam_Title;
        //            model.Mark = exam.Exam_Mark;
        //            model.PassMark = exam.Exam_PassMark;
        //        }
        //    }

        //    return PartialView(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Model_Exam model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Tbl_Exam exam = new Tbl_Exam();

        //        if (model.ID != null)
        //        {
        //            exam = db.Tbl_Exam.Where(x => x.Exam_ID == model.ID).FirstOrDefault();

        //            if (exam != null)
        //            {
        //                exam.Exam_Title = model.Title;
        //                exam.Exam_Mark = model.Mark;
        //                exam.Exam_PassMark = model.PassMark;
        //                exam.Exam_ModifiedDate = DateTime.Now;

        //                db.Entry(exam).State = EntityState.Modified;
        //            }
        //            else
        //            {
        //                return HttpNotFound();
        //            }
        //        }
        //        else
        //        {
        //            exam.Exam_Title = model.Title;
        //            exam.Exam_Mark = model.Mark;
        //            exam.Exam_PassMark = model.PassMark;
        //            exam.Exam_CreationDate = exam.Exam_ModifiedDate = DateTime.Now;

        //            db.Tbl_Exam.Add(exam);
        //        }

        //        if (Convert.ToBoolean(db.SaveChanges() > 0))
        //        {
        //            TempData["TosterState"] = "success";
        //            TempData["TosterType"] = TosterType.Maseage;
        //            TempData["TosterTitel"] = "2";
        //            TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }

        //    return View();
        //}

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_DeleteModal model = new Model_DeleteModal();

                var exam = db.Tbl_Exam.Where(x => x.Exam_ID == id).FirstOrDefault();

                if (exam != null)
                {
                    model.ID = id.Value;
                    model.Name = exam.Exam_Title;
                    model.Description = "آیا از حذف آزمون مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Model_DeleteModal model)
        {
            if (ModelState.IsValid)
            {
                var exam = db.Tbl_Exam.Where(x => x.Exam_ID == model.ID).FirstOrDefault();

                if (exam != null)
                {
                    exam.Exam_IsDelete = true;

                    db.Entry(exam).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {

                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterTitel"] = "2";
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
            }

            return View();
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