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

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class ExamController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var examList = db.Tbl_Exam.Select(x => new Model_Exam
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

            return View(model);
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
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Tbl_Exam tbl_Exam = await db.Tbl_Exam.FindAsync(id);
            //db.Tbl_Exam.Remove(tbl_Exam);
            //await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
