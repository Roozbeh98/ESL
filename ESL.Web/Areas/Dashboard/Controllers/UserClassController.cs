using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
using ESL.Services.Services;
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
    public class UserClassController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

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
                var _User = db.Tbl_User.Where(x => x.User_Guid == model.UserGuid && x.User_IsDelete == false).SingleOrDefault();

                if (db.Tbl_UserClassPlan.Where(x => x.UCP_UserID == _User.User_ID && x.UCP_CPID == model.ClassID && x.UCP_IsDelete == false).FirstOrDefault() != null)
                {
                    TempData["TosterState"] = "info";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                    return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                };

                var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ClassID).SingleOrDefault();

                if (_ClassPlan != null)
                {
                    Tbl_Payment _Payment = Purchase(_User, _ClassPlan.CP_CostPerSession, out int state);

                    db.Tbl_Payment.Add(_Payment);

                    Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                    _Wallet.Wallet_Credit = _Payment.Payment_RemaingWallet - _Payment.Payment_Cost;
                    _Wallet.Wallet_ModifiedDate = DateTime.Now;

                    db.Entry(_Wallet).State = EntityState.Modified;

                    Tbl_UserClassPlan _UserClassPlan = new Tbl_UserClassPlan()
                    {
                        UCP_Guid = Guid.NewGuid(),
                        UCP_UserID = _User.User_ID,
                        UCP_CPID = model.ClassID,
                        Tbl_Payment = _Payment,
                        UCP_IsActive = true,
                        UCP_CreationDate = DateTime.Now,
                        UCP_ModifiedDate = DateTime.Now
                    };

                    db.Tbl_UserClassPlan.Add(_UserClassPlan);

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        if (state == 1)
                        {
                            TempData["TosterState"] = "success";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شد";
                        }
                        else if (state == 0)
                        {
                            TempData["TosterState"] = "error";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "موجودی کیف پول کاربر مورد نظر کافی نمی باشد";
                        }
                        else if (state == -1)
                        {
                            TempData["TosterState"] = "warning";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "خطا در ارسال پیامک";
                        }

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                    }

                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";
                    

                    return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private Tbl_Payment Purchase(Tbl_User user, int cost, out int state)
        {
            int credit = new Rep_Wallet().Get_WalletCreditWithUserID(user.User_ID);

            if (credit + 30000 < cost)
            {
                if (new SMSPortal().SendServiceable(user.User_Mobile, ".", "", "", user.User_FirstName + " " + user.User_lastName, SMSTemplate.Charge) != "ارسال به مخابرات")
                {
                    state = -1;
                }
                else
                {
                    state = 0;
                }
            }
            else
            {
                state = 1;
            }

            Tbl_Payment _Payment = new Tbl_Payment
            {
                Payment_Guid = Guid.NewGuid(),
                Payment_UserID = user.User_ID,
                Payment_TitleCodeID = (int)PaymentTitle.Class,
                Payment_WayCodeID = (int)PaymentWay.Internet,
                Payment_StateCodeID = (int)PaymentState.Confirmed,
                Payment_Cost = cost,
                Payment_Discount = 0,
                Payment_RemaingWallet = credit,
                Payment_TrackingToken = "ESL-" + new Random().Next(100000, 999999).ToString(),
                Payment_CreateDate = DateTime.Now,
                Payment_ModifiedDate = DateTime.Now
            };

            return _Payment;
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