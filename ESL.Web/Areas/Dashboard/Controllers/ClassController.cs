using ESL.Common.Plugins;
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
    public class ClassController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false).Select(x => new Model_Class
            {
                ID = x.CP_ID,
                Class = x.Tbl_Class.Class_Title,
                Type = x.Tbl_Code.Code_Display,
                Description = x.CP_Description,
                CostPerSession = x.CP_CostPerSession,
                Location = x.CP_Location,
                Activeness = x.CP_IsActive,
                Capacity = x.CP_Capacity,
                Time = x.CP_Time,
                SessionsLength = x.CP_Capacity,
                ExamDate = x.CP_ExamDate,
                CreationDate = x.CP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ClassCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ClassPlan q = new Tbl_ClassPlan
                {
                    CP_Guid = Guid.NewGuid(),
                    CP_ClassID = db.Tbl_Class.Where(x => x.Class_Guid.ToString() == model.Class).SingleOrDefault().Class_ID,
                    CP_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type),
                    CP_Description = model.Description,
                    CP_CostPerSession = model.CostPerSession,
                    CP_Location = model.Location,
                    CP_Capacity = model.Capacity,
                    CP_Time = model.Time,
                    CP_SessionsLength = model.SessionsLength,
                    CP_ExamDate = DateConverter.ToGeorgianDateTime(model.ExamDate),
                    CP_IsActive = model.Activeness,
                    CP_CreationDate = DateTime.Now,
                    CP_ModifiedDate = DateTime.Now,
                };

                db.Tbl_ClassPlan.Add(q);

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

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.CP_Description;
                    model.Description = "آیا از حذف کلاس مورد نظر اطمینان دارید ؟";

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
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.CP_IsDelete = true;

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

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_ClassPlan.Any(x => x.CP_ID == id))
            {
                var q = db.Tbl_UserClass.Where(x => x.UC_CPID == id).Select(x => new Model_UserClass
                {
                    ID = x.UC_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    CreationDate = x.UC_CreationDate,
                    IsDelete = x.UC_IsDelete,

                }).ToList();

                ViewBag.ClassID = id;

                return View(q);
            }

            return HttpNotFound();
        }

        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.CP_IsActive
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetActiveness(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.CP_IsActive = model.Activeness;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "Class", new { area = "Dashboard" });
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
    }
}