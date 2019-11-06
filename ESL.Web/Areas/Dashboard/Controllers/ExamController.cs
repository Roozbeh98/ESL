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
using System.IO;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using ESL.Services.BaseRepository;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class ExamController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var examList = db.Tbl_ExamRemotely.Where(x => x.ER_IsDelete == false).Select(x => new Model_Exam
            {
                ID = x.ER_ID,
                Title = x.ER_Title,
                Mark = x.ER_Mark,
                PassMark = x.ER_PassMark,
                CreationDate = x.ER_CreationDate

            }).ToList();

            return View(examList);
        }

        public ActionResult CreateOrEdit(int? id)
        {
            Model_Exam model = new Model_Exam();

            if (id != null)
            {
                var exam = db.Tbl_ExamRemotely.Where(x => x.ER_ID == id).FirstOrDefault();

                if (exam != null)
                {
                    model.ID = exam.ER_ID;
                    model.Title = exam.ER_Title;
                    model.Mark = exam.ER_Mark;
                    model.PassMark = exam.ER_PassMark;
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
                Tbl_ExamRemotely exam = new Tbl_ExamRemotely();

                if (model.ID != null)
                {
                    exam = db.Tbl_ExamRemotely.Where(x => x.ER_ID == model.ID).FirstOrDefault();

                    if (exam != null)
                    {
                        exam.ER_Title = model.Title;
                        exam.ER_Mark = model.Mark;
                        exam.ER_PassMark = model.PassMark;
                        exam.ER_ModifiedDate = DateTime.Now;

                        db.Entry(exam).State = EntityState.Modified;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    exam.ER_Title = model.Title;
                    exam.ER_Mark = model.Mark;
                    exam.ER_PassMark = model.PassMark;
                    exam.ER_CreationDate = exam.ER_ModifiedDate = DateTime.Now;

                    db.Tbl_ExamRemotely.Add(exam);
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
                Model_MessageModal model = new Model_MessageModal();

                var exam = db.Tbl_ExamRemotely.Where(x => x.ER_ID == id).FirstOrDefault();

                if (exam != null)
                {
                    model.ID = id.Value;
                    model.Name = exam.ER_Title;
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
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var exam = db.Tbl_ExamRemotely.Where(x => x.ER_ID == model.ID).FirstOrDefault();

                if (exam != null)
                {
                    exam.ER_IsDelete = true;

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

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Model_Questions model = new Model_Questions();
            model.ExamID = id.Value;

            var q = db.Tbl_Question.Where(x => x.Tbl_ExamRemotely.ER_ID == id).ToList();

            if (q.Count > 0)
            {
                model.Questions = q.Select(x => new Model_Question
                {
                    ID = x.Question_ID,
                    Title = x.Question_Title,
                    Type = x.Tbl_Code.Code_Display,
                    Group = db.Tbl_Code.Where(y => y.Code_ID.Equals(x.Question_GroupCodeID)).FirstOrDefault().Code_Name,
                    Response = db.Tbl_Code.Where(y => y.Code_ID.Equals(x.Question_ResponseID)).FirstOrDefault().Code_Name,
                    Mark = x.Question_Mark,
                    CreationDate = x.Question_CreationDate

                }).ToList();
            }

            return View(model);
        }

        public ActionResult CreateQuestion(int id)
        {
            Model_QuestionCreate model = new Model_QuestionCreate
            {
                ExamID = id
            };

            return View(model);
        }

        public ActionResult EditQuestion(int? id)
        {
            Model_QuestionCreate model = new Model_QuestionCreate();

            if (id != null)
            {
                var q = db.Tbl_Question.Where(x => x.Question_ID == id).FirstOrDefault();

                if (q != null)
                {
                    //model.ID = q.Question_ID;
                    model.Title = q.Question_Title;
                    model.Type = q.Tbl_Code.Code_Guid;
                    model.Group = db.Tbl_Code.Where(y => y.Code_ID.Equals(q.Question_GroupCodeID)).FirstOrDefault().Code_Guid;
                    model.Response = q.Tbl_Response.Response_Guid;
                    model.Mark = q.Question_Mark;
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuestion(Model_QuestionCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Question q = new Tbl_Question();
                q.Question_ERID = model.ExamID;
                q.Question_Title = model.Title;
                q.Question_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                q.Question_GroupCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Group);
                //Question_ResponseID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Response);
                q.Question_Mark = model.Mark;
                q.Question_Order = db.Tbl_Question.Any() ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;
                q.Question_CreationDate = q.Question_ModifiedDate = DateTime.Now;

                Tbl_Response p1 = new Tbl_Response();
                p1.Response_QuestionID = q.Question_ID;
                p1.Response_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                p1.Response_Title = model.Filepond1;
                p1.Response_Order = db.Tbl_Response.Any(x => x.Response_QuestionID.Equals(q.Question_ID)) ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;

                //p1.Tbl_Question = q;

                Tbl_Response p2 = new Tbl_Response();
                p1.Response_QuestionID = q.Question_ID;
                p1.Response_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                p1.Response_Title = model.Filepond2;
                p1.Response_Order = db.Tbl_Response.Any(x => x.Response_QuestionID.Equals(q.Question_ID)) ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;

                Tbl_Response p3 = new Tbl_Response();
                p1.Response_QuestionID = q.Question_ID;
                p1.Response_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                p1.Response_Title = model.Filepond3;
                p1.Response_Order = db.Tbl_Response.Any(x => x.Response_QuestionID.Equals(q.Question_ID)) ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;

                Tbl_Response p4 = new Tbl_Response();
                p1.Response_QuestionID = q.Question_ID;
                p1.Response_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                p1.Response_Title = model.Filepond4;
                p1.Response_Order = db.Tbl_Response.Any(x => x.Response_QuestionID.Equals(q.Question_ID)) ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;

                db.Tbl_Response.Add(p1);
                db.Tbl_Response.Add(p2);
                db.Tbl_Response.Add(p3);
                db.Tbl_Response.Add(p4);

                switch (Rep_CodeGroup.Get_CodeNameWithGUID(model.Response))
                {
                    case "1":
                        q.Question_ResponseID = p1.Response_ID;
                        break;

                    case "2":
                        q.Question_ResponseID = p2.Response_ID;
                        break;

                    case "3":
                        q.Question_ResponseID = p3.Response_ID;
                        break;

                    case "4":
                        q.Question_ResponseID = p4.Response_ID;
                        break;

                    default:
                        break;
                }

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

                    return RedirectToAction("Details");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(Model_QuestionCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Question q = new Tbl_Question();

                if (model.ExamID != null)  // ID
                {
                    q = db.Tbl_Question.Where(x => x.Question_ID == model.ExamID).FirstOrDefault();  // ID

                    if (q != null)
                    {
                        q.Question_Title = model.Title;
                        q.Question_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                        q.Question_GroupCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Group);
                        q.Question_ResponseID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Response);
                        q.Question_Mark = model.Mark;
                        q.Question_ModifiedDate = DateTime.Now;

                        db.Entry(q).State = EntityState.Modified;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    q.Question_ERID = model.ExamID;
                    q.Question_Title = model.Title;
                    q.Question_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                    q.Question_GroupCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Group);
                    q.Question_ResponseID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Response);
                    q.Question_Mark = model.Mark;
                    q.Question_Order = db.Tbl_Question.Any() ? db.Tbl_Question.OrderByDescending(x => x.Question_Order).First().Question_Order : 1;
                    q.Question_CreationDate = q.Question_CreationDate = DateTime.Now;

                    db.Tbl_Question.Add(q);
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
    }
}
