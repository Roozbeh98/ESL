using ESL.DataLayer.Domain;
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
    public class ExamInPersonController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_IsDelete == false).Select(x => new Model_ExamsInPerson
            {
                ID = x.EIP_ID,
                Title = x.EIP_Title,
                Cost = x.EIP_Cost,
                Location = x.EIP_Location,
                Capacity = x.EIP_Capacity,
                TotalMark = x.EIP_TotalMark,
                PassMark = x.EIP_PassMark,
                CreationDate = x.EIP_CreationDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_ExamInPersonCreate model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPerson q = new Tbl_ExamInPerson
                {
                    EIP_Guid = Guid.NewGuid(),
                    EIP_Title = model.Title,
                    EIP_Cost = model.Cost,
                    EIP_Location = model.Location,
                    EIP_Capacity = model.Capacity,
                    EIP_TotalMark = model.TotalMark,
                    EIP_PassMark = model.PassMark,
                    EIP_CreationDate = DateTime.Now,
                    EIP_ModifiedDate = DateTime.Now
                };

                db.Tbl_ExamInPerson.Add(q);

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

        public ActionResult Edit(int id)
        {
            var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_ExamInPersonEdit model = new Model_ExamInPersonEdit()
                {
                    ID = q.EIP_ID,
                    Title = q.EIP_Title,
                    Cost = q.EIP_Cost,
                    Location = q.EIP_Location,
                    Capacity = q.EIP_Capacity,
                    TotalMark = q.EIP_TotalMark,
                    PassMark = q.EIP_PassMark
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model_ExamInPersonEdit model)
        {
            if (ModelState.IsValid)
            {
                Tbl_ExamInPerson q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIP_Title = model.Title;
                    q.EIP_Cost = model.Cost;
                    q.EIP_Capacity = model.Capacity;
                    q.EIP_TotalMark = model.TotalMark;
                    q.EIP_PassMark = model.PassMark;

                    q.EIP_ModifiedDate = DateTime.Now;

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

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.EIP_Title;
                    model.Description = "آیا از حذف آزمون مورد نظر اطمینان دارید ؟";

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
                var q = db.Tbl_ExamInPerson.Where(x => x.EIP_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.EIP_IsDelete = true;

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
            if (id.HasValue && db.Tbl_ExamInPerson.Any(x => x.EIP_ID == id))
            {
                var q = db.Tbl_UserExamInPerson.Where(x => x.UEIP_EIPID == id).Select(x => new Model_UsersExamInPerson
                {
                    ID = x.UEIP_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    SeatNumber = x.UEIP_SeatNumber,
                    Mark = x.UEIP_Mark,
                    IsPresent = x.UEIP_IsPresent,
                    CreationDate = x.UEIP_CreationDate,
                    IsDelete = x.UEIP_IsDelete

                }).ToList();

                ViewBag.ExamID = id;

                return View(q);
            }

            return HttpNotFound();
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