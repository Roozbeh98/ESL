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
    public class UserWorkshopController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Create(int id)
        {
            Model_UserWorkshopCreate model = new Model_UserWorkshopCreate()
            {
                WorkshopID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_UserWorkshopCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_UserWorkshop q = new Tbl_UserWorkshop
                {
                    UW_Guid = Guid.NewGuid(),
                    UW_UserID = new Rep_User().Get_UserIDWithGUID(model.UserGuid),
                    UW_WPID = model.WorkshopID,
                    UW_CreationDate = DateTime.Now,
                    UW_ModifiedDate = DateTime.Now
                };

                db.Tbl_UserWorkshop.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                    return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetPresence(int id)
        {
            var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_UserWorkshopSetPresence model = new Model_UserWorkshopSetPresence()
                {
                    ID = id,
                    Presence = q.UW_IsPresent
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPresence(Model_UserWorkshopSetPresence model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UW_IsPresent = model.Presence;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault().UW_WPID });
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

        public ActionResult UnRegister(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == id).SingleOrDefault();

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
        public ActionResult UnRegister(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UW_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault().UW_WPID });
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
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == id).SingleOrDefault();

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
        public ActionResult Register(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UW_IsDelete = false;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshop.Where(x => x.UW_ID == model.ID).SingleOrDefault().UW_WPID });
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