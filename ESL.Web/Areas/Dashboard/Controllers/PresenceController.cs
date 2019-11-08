using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PresenceController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_UserClass.Where(x => x.UC_IsDelete == false).Select(x => new Model_UserClasses
            {
                ID = x.UC_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Class = x.Tbl_ClassPlan.Tbl_Class.Class_Title,
                Type = x.Tbl_ClassPlan.Tbl_Code.Code_Display,
                Location = x.Tbl_ClassPlan.CP_Location,
                Time = x.Tbl_ClassPlan.CP_Time,
                CreationDate = x.UC_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_UserClass.Any(x => x.UC_ID == id))
            {
                var q = db.Tbl_Presence.Where(x => x.Presence_IsDelete == false).Select(x => new Model_UserClassPresence
                {
                    ID = x.Presence_ID,
                    Cost = x.Tbl_Payment.Payment_Cost,
                    Discount = x.Tbl_Payment.Payment_Discount,
                    Presence = x.Presence_IsPresent,
                    CreationDate = x.Presence_CreationDate

                }).ToList();

                var p = db.Tbl_UserClass.Where(x => x.UC_ID == id).SingleOrDefault();

                ViewBag.UserClassID = id;
                ViewBag.Title = "لیست حضور " + p.Tbl_User.User_FirstName + " " + p.Tbl_User.User_lastName + " - " + p.Tbl_ClassPlan.Tbl_Class.Class_Title;

                return View(q);
            }

            return HttpNotFound();
        }

    }
}