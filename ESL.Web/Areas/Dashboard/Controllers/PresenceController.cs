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
    public class PresenceController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var _UserClassPlans = db.Tbl_UserClassPlan.Where(x => x.UCP_IsActive == true && x.UCP_IsDelete == false).Select(x => new Model_UserClassPlans
            {
                ID = x.UCP_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Class = x.Tbl_ClassPlan.Tbl_Class.Class_Title,
                Type = x.Tbl_ClassPlan.Tbl_Code.Code_Display,
                Location = x.Tbl_ClassPlan.CP_Location,
                Time = x.Tbl_ClassPlan.CP_Time,
                CreationDate = x.UCP_CreationDate

            }).ToList();

            return View(_UserClassPlans);
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue && db.Tbl_UserClassPlan.Any(x => x.UCP_ID == id && x.UCP_IsActive == true))
            {
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_IsDelete == false && x.UCP_ID == id).SingleOrDefault();

                var _UserClassPlanPresences = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_IsDelete == false && x.Tbl_UserClassPlan.UCP_UserID == _UserClassPlan.UCP_UserID && x.Tbl_UserClassPlan.UCP_CPID == _UserClassPlan.UCP_CPID && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_TypeCodeID == _UserClassPlan.Tbl_ClassPlan.CP_TypeCodeID && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_Location == _UserClassPlan.Tbl_ClassPlan.CP_Location && x.Tbl_UserClassPlan.Tbl_ClassPlan.CP_Time == _UserClassPlan.Tbl_ClassPlan.CP_Time).Select(x => new Model_UserClassPlanPresence
                {
                    ID = x.UCPP_ID,
                    Cost = x.Tbl_Payment.Payment_Cost,
                    Discount = x.Tbl_Payment.Payment_Discount,
                    Presence = x.UCPP_IsPresent,
                    Date = x.UCPP_Date

                }).ToList();

                TempData["UserClassPlanID"] = id;

                return View(_UserClassPlanPresences);
            }

            return HttpNotFound();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_UserClassPlanPresenceCreate model)
        {
            if (ModelState.IsValid)
            {
                int userClassId = (int)TempData["UserClassPlanID"];

                var _UserClass = db.Tbl_UserClassPlan.Where(x => x.UCP_IsDelete == false && x.UCP_ID == userClassId).SingleOrDefault();

                var _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _UserClass.UCP_UserID).SingleOrDefault();
                _Wallet.Wallet_Credit = _Wallet.Wallet_Credit - _UserClass.Tbl_ClassPlan.CP_CostPerSession + model.Discount;

                Tbl_Payment _Payment = new Tbl_Payment()
                {
                    Payment_Guid = Guid.NewGuid(),
                    Payment_UserID = _UserClass.UCP_UserID,
                    Payment_TitleCodeID = (int)PaymentTitle.Presence,
                    Payment_WayCodeID = (int)PaymentWay.InPerson,
                    Payment_StateCodeID = (int)PaymentState.Confirmed,
                    Payment_Cost = _UserClass.Tbl_ClassPlan.CP_CostPerSession,
                    Payment_Discount = model.Discount,
                    Payment_RemaingWallet = _Wallet.Wallet_Credit,
                    Payment_TrackingToken = "ESL-" + new Random().Next(100000, 999999).ToString(),
                    Payment_CreateDate = DateTime.Now,
                    Payment_ModifiedDate = DateTime.Now
                };

                Tbl_UserClassPlanPresence _UserClassPlanPresence = new Tbl_UserClassPlanPresence
                {
                    UCPP_Guid = Guid.NewGuid(),
                    UCPP_UCPID = userClassId,
                    UCPP_IsPresent = model.Presence,
                    UCPP_Date = DateConverter.ToGeorgianDateTime(model.Date),
                    UCPP_CreationDate = DateTime.Now,
                    UCPP_ModifiedDate = DateTime.Now
                };

                _UserClassPlanPresence.Tbl_Payment = _Payment;

                db.Tbl_Payment.Add(_Payment);
                db.Tbl_UserClassPlanPresence.Add(_UserClassPlanPresence);
                db.Entry(_Wallet).State = EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "حضور با موفقیت ثبت شد";

                    return RedirectToAction("Details", new { id = userClassId });
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "حضور با موفقیت ثبت نشد";

                    return RedirectToAction("Details", new { id = userClassId });
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Model_Message model = new Model_Message();

                var _UserClassPlanPresence = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == id).SingleOrDefault();

                if (_UserClassPlanPresence != null)
                {
                    model.ID = id.Value;
                    model.Name = _UserClassPlanPresence.Tbl_UserClassPlan.Tbl_User.User_FirstName + " " + _UserClassPlanPresence.Tbl_UserClassPlan.Tbl_User.User_lastName;
                    model.Description = "آیا از حذف حضور مورد نظر اطمینان دارید ؟";

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
        public ActionResult Delete(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var _UserClassPlanPresence = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault();

                if (_UserClassPlanPresence != null)
                {
                    var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == _UserClassPlanPresence.UCPP_PaymentID).SingleOrDefault();
                    
                    if (_Payment != null)
                    {
                        _UserClassPlanPresence.UCPP_IsDelete = true;
                        _UserClassPlanPresence.UCPP_ModifiedDate = DateTime.Now;
                        _Payment.Payment_IsDelete = true;
                        _Payment.Payment_ModifiedDate = DateTime.Now;

                        var _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _UserClassPlanPresence.Tbl_UserClassPlan.UCP_UserID).SingleOrDefault();
                        _Wallet.Wallet_Credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) + _Payment.Payment_Cost - _Payment.Payment_Discount;

                        db.Entry(_UserClassPlanPresence).State = EntityState.Modified;
                        db.Entry(_Payment).State = EntityState.Modified;
                        db.Entry(_Wallet).State = EntityState.Modified;
                    }

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "حضور مورد نظر با موفقیت حذف شد";

                        return RedirectToAction("Details", "Presence", new { area = "Dashboard", id = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault().Tbl_UserClassPlan.UCP_ID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "حضور مورد نظر با موفقیت حذف نشد";

                        return RedirectToAction("Details", "Presence", new { area = "Dashboard", id = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault().Tbl_UserClassPlan.UCP_ID });
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetPresence(int id)
        {
            var _UserClassPlanPresence = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == id).SingleOrDefault();

            if (_UserClassPlanPresence != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = _UserClassPlanPresence.UCPP_IsPresent
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPresence(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var _UserClassPlanPresence = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault();

                if (_UserClassPlanPresence != null)
                {
                    _UserClassPlanPresence.UCPP_IsPresent = model.Activeness;
                    _UserClassPlanPresence.UCPP_ModifiedDate = DateTime.Now;

                    db.Entry(_UserClassPlanPresence).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت حضور با موفقیت انجام شد";

                        return RedirectToAction("Details", "Presence", new { area = "Dashboard", id = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault().Tbl_UserClassPlan.UCP_ID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت حضور با موفقیت انجام نشد";

                        return RedirectToAction("Details", "Presence", new { area = "Dashboard", id = db.Tbl_UserClassPlanPresence.Where(x => x.UCPP_ID == model.ID).SingleOrDefault().Tbl_UserClassPlan.UCP_ID });
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
