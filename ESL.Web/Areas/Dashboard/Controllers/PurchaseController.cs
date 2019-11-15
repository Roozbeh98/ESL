using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Student")]
    public class PurchaseController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            List<Model_Purchase> model = new List<Model_Purchase>();
            
            var _Workshop = db.Tbl_WorkshopPlan.Where(x => x.WP_IsDelete == false).Select(x => new Model_Purchase
            {
                ID = x.WP_ID,
                Name = x.Tbl_SubWorkshop.Tbl_Workshop.Workshop_Title,
                SubName = x.Tbl_SubWorkshop.SW_Title,
                Description = x.WP_Description,
                Capacity = x.WP_Capacity,
                Location = x.WP_Location,
                Date = x.WP_Date,
                Cost = x.WP_Cost,
                Type = (int)ProductType.Workshop

            }).ToList();

            var _Class = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false).Select(x => new Model_Purchase
            {
                ID = x.CP_ID,
                Name = x.Tbl_Class.Class_Title,
                Description = x.CP_Description,
                Capacity = x.CP_Capacity,
                Location = x.CP_Location,
                Time = x.CP_Time,
                Date = x.CP_StartDate,
                Cost = x.CP_CostPerSession,
                Type = (int)ProductType.Class

            }).ToList();

            var _ExamInPerson = db.Tbl_ExamInPerson.Where(x => x.EIP_IsDelete == false).Select(x => new Model_Purchase
            {
                ID = x.EIP_ID,
                Name = x.Tbl_SubExam.Tbl_Exam.Exam_Title,
                SubName = x.Tbl_SubExam.SE_Title,
                Description = x.EIP_Description,
                Capacity = x.EIP_Capacity,
                Location = x.EIP_Location,
                TotalMark = x.EIP_TotalMark,
                PassMark = x.EIP_PassMark,
                Date = x.EIP_Date,
                Cost = x.EIP_Cost,
                Type = (int)ProductType.ExamInPerson

            }).ToList();

            //var _ExamRemotely = db.Tbl_ExamRemotely.Where(x => x.ER_IsDelete == false).Select(x => new Model_Purchase
            //{
            //    ID = x.EIP_ID,
            //    Name = x.Tbl_SubExam.Tbl_Exam.Exam_Title,
            //    SubName = x.Tbl_SubExam.SE_Title,
            //    Description = x.EIP_Description,
            //    Capacity = x.EIP_Capacity,
            //    Location = x.EIP_Location,
            //    TotalMark = x.EIP_TotalMark,
            //    PassMark = x.EIP_PassMark,
            //    Date = x.EIP_Date,
            //    Cost = x.EIP_CostS,
            //    Type = (int)ProductType.ExamRemotely

            //}).ToList();

            model.AddRange(_Workshop);
            model.AddRange(_Class);
            model.AddRange(_ExamInPerson);
            //model.AddRange(_ExamRemotely);

            return View(model);
        }

        public ActionResult Pay(int? id, int type)
        {
            if (id != null)
            {
                switch ((ProductType)type)
                {
                    case ProductType.Workshop:
                        break;

                    case ProductType.Class:
                        break;

                    case ProductType.ExamInPerson:
                        break;

                    case ProductType.ExamRemotely:
                        break;

                    default:
                        break;
                }

                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
                    model.Description = "آیا از خرید اطمینان دارید ؟";

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
        public ActionResult Pay(Model_Purchase model)
        {
            return View();
        }
    }
}
