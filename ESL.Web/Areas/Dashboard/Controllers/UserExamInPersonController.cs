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
    [Authorize(Roles = "Admin")]
    public class UserExamInPersonController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Create(int id)
        {
            Model_UserExamInPersonPlanCreate model = new Model_UserExamInPersonPlanCreate()
            {
                ExamID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_UserExamInPersonPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_UserExamInPersonPlan q = new Tbl_UserExamInPersonPlan
                {
                    UEIPP_Guid = Guid.NewGuid(),
                    UEIPP_UserID = new Rep_User().Get_UserIDWithGUID(model.UserGuid),
                    UEIPP_EIPPID = model.ExamID,
                    UEIPP_CreationDate = DateTime.Now,
                    UEIPP_ModifiedDate = DateTime.Now
                };

                db.Tbl_UserExamInPersonPlan.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                    return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetMark(int id)
        {
            var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_UserExamInPersonPlanSetMark model = new Model_UserExamInPersonPlanSetMark()
                {
                    ID = id,
                    Mark = q.UEIPP_Mark
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetMark(Model_UserExamInPersonPlanSetMark model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UEIPP_Mark = model.Mark;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
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

        public ActionResult SetSeatNumber(int id)
        {
            var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_UserExamInPersonPlanSetSeatNumber model = new Model_UserExamInPersonPlanSetSeatNumber()
                {
                    ID = id,
                    SeatNumber = q.UEIPP_SeatNumber
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSeatNumber(Model_UserExamInPersonPlanSetSeatNumber model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UEIPP_SeatNumber = model.SeatNumber;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
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

        public ActionResult SetPresence(int id)
        {
            var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_UserExamInPersonPlanSetPresence model = new Model_UserExamInPersonPlanSetPresence()
                {
                    ID = id,
                    Presence = q.UEIPP_IsPresent
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPresence(Model_UserExamInPersonPlanSetPresence model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UEIPP_IsPresent = model.Presence;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
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

                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

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
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UEIPP_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
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

                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

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
                var q = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.UEIPP_IsDelete = false;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
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