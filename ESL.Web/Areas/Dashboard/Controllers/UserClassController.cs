using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
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
    public class UserClassController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Create(int id)
        {
            Model_UserClassPlanCreate model = new Model_UserClassPlanCreate()
            {
                ClassID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_UserClassPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_UserClassPlan q = new Tbl_UserClassPlan
                {
                    UCP_Guid = Guid.NewGuid(),
                    UCP_UserID = new Rep_User().Get_UserIDWithGUID(model.UserGuid),
                    UCP_CPID = model.ClassID,
                    UCP_CreationDate = DateTime.Now,
                    UCP_ModifiedDate = DateTime.Now
                };

                db.Tbl_UserClassPlan.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                    return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult UnRegister(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var q = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
                    model.Description = $"آیا از لغو ثبت نام کاربر { model.Name } اطمینان دارید ؟";

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
        public ActionResult UnRegister(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UCP_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
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

        public ActionResult Register(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var q = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
                    model.Description = $"آیا از ثبت نام مجدد کاربر { model.Name } اطمینان دارید ؟";

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
        public ActionResult Register(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UCP_IsDelete = false;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
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

        public JsonResult Get_Users(string searchTerm)
        {
            var q = db.Tbl_User.Where(a => a.User_IsDelete == false && a.User_RoleID == 1).Select(a => new { id = a.User_Guid, text = a.User_FirstName + " " + a.User_lastName });

            if (searchTerm != null)
            {
                q = q.Where(a => a.text.Contains(searchTerm)).Select(a => new { id = a.id, text = a.text });
            }

            return Json(q, JsonRequestBehavior.AllowGet);
        }
    }
}