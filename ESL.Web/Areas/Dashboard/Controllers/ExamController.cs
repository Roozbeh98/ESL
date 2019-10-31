using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESL.DataLayer.Domain;
using ESL.DataLayer.ViewModels;
using System.IO;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class ExamController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var examList = db.Tbl_Exam.Where(x => x.Exam_IsDelete == false).Select(x => new Model_Exam
            {
                ID = x.Exam_ID,
                Title = x.Exam_Title,
                Mark = x.Exam_Mark,
                PassMark = x.Exam_PassMark,
                CreationDate = x.Exam_CreationDate

            }).ToList();

            return View(examList);
        }

        public ActionResult CreateOrEdit(int? id)
        {
            Model_Exam model = new Model_Exam();

            if (id != null)
            {
                var exam = db.Tbl_Exam.Where(x => x.Exam_ID == id).FirstOrDefault();

                if (exam != null)
                {
                    model.ID = exam.Exam_ID;
                    model.Title = exam.Exam_Title;
                    model.Mark = exam.Exam_Mark;
                    model.PassMark = exam.Exam_PassMark;
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit(Model_Exam model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Exam exam = new Tbl_Exam();

                if (model.ID != null)
                {
                    exam = db.Tbl_Exam.Where(x => x.Exam_ID == model.ID).FirstOrDefault();

                    if (exam != null)
                    {
                        exam.Exam_Title = model.Title;
                        exam.Exam_Mark = model.Mark;
                        exam.Exam_PassMark = model.PassMark;
                        exam.Exam_ModifiedDate = DateTime.Now;

                        db.Entry(exam).State = EntityState.Modified;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    exam.Exam_Title = model.Title;
                    exam.Exam_Mark = model.Mark;
                    exam.Exam_PassMark = model.PassMark;
                    exam.Exam_CreationDate = exam.Exam_ModifiedDate = DateTime.Now;

                    db.Tbl_Exam.Add(exam);
                }

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

            return View();
        }

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












        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Server.MapPath($"/App_Data/{file.FileName}"));
                    return Json(file.FileName);
                }
            }

            return Json(false);
        }

        [HttpDelete]
        public JsonResult RevertFile()
        {
            string res;

            MemoryStream memstream = new MemoryStream();
            Request.InputStream.CopyTo(memstream);
            memstream.Position = 0;
            using (StreamReader reader = new StreamReader(memstream))
            {
                res = reader.ReadToEnd();
            }

            res = res.Remove(res.Length - 1);
            string filename = res.Substring(1);

            string source = Request.MapPath($"/App_Data/{filename}");

            if (System.IO.File.Exists(source))
            {
                System.IO.File.Delete(source);
            }

            return Json(true);
        }
    }
}
