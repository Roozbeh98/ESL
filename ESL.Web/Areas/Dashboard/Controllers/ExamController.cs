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

        public ActionResult Details(int id)
        {
            var q = db.Tbl_Question.Where(x => x.Tbl_Exam.Exam_ID == id).Select(x => new Model_QuestionList
            {
                ID = x.Question_ID,
                Title = x.Question_Title,
                Type = x.Tbl_Code.Code_Display,
                Response = db.Tbl_Code.Where(y => y.Code_ID.Equals(x.Question_ResponseID)).FirstOrDefault().Code_Name,
                Mark = x.Question_Mark,
                CreationDate = x.Question_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult CreateOrEditQuestion(int? id)
        {
            Model_Question model = new Model_Question();

            if (id != null)
            {
                var q = db.Tbl_Question.Where(x => x.Question_ID == id).FirstOrDefault();

                if (q != null)
                {
                    model.ID = q.Question_ID;
                    //model.ExamID = q.Question_ExamID;
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
        public ActionResult CreateOrEditQuestion(Model_Question model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Question q = new Tbl_Question();

                if (model.ID != null)
                {
                    q = db.Tbl_Question.Where(x => x.Question_ID == model.ID).FirstOrDefault();

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
                    q.Question_ExamID = model.ExamID;
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

        //[HttpGet]
        //public JsonResult Get_AllStudentList(string searchTerm)
        //{
        //    List<DropDownModel> t = new List<DropDownModel>();

        //    var q = from a in db.Tbl_BaseRolesPermission where (a.Tbl_Permission.Permission_Name == "Student") select a;


        //    foreach (var item in q)
        //    {
        //        var p = from b in db.Tbl_Login where (b.Login_BaseRoleID == item.BRP_BaseRoleID) select b;

        //        if (p != null)
        //        {
        //            foreach (var Student in p)
        //            {
        //                if (!Student.Login_RegisterActive)
        //                {
        //                    t.Add(new DropDownModel((int)Student.Login_UserID, Student.Tbl_User.User_Name + " " + Student.Tbl_User.User_Family));
        //                }
        //            }
        //        }
        //    }

        //    var md = t.Select(a => new { id = a.id, text = a.name });

        //    if (searchTerm != null)
        //    {
        //        md = t.Where(a => a.name.Contains(searchTerm)).Select((a => new { id = a.id, text = a.name }));
        //    }



        //    return Json(md, JsonRequestBehavior.AllowGet);
        //}

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
