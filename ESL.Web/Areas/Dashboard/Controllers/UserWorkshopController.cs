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
    public class UserWorkshopController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        public ActionResult Create(int id)
        {
            Model_UserWorkshopPlanCreate model = new Model_UserWorkshopPlanCreate()
            {
                WorkshopID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_UserWorkshopPlanCreate model)
        {
            if (ModelState.IsValid)
            {
                var _User = db.Tbl_User.Where(x => x.User_Guid == model.UserGuid && x.User_IsDelete == false).SingleOrDefault();

                if (db.Tbl_UserWorkshopPlan.Where(x => x.UWP_UserID == _User.User_ID && x.UWP_WPID == model.WorkshopID && x.UWP_IsDelete == false).FirstOrDefault() != null)
                {
                    TempData["TosterState"] = "info";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کارگاه مورد نظر قبلا خریداری شده است";

                    return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                };

                var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.WorkshopID).SingleOrDefault();

                if (_WorkshopPlan != null)
                {
                    if (_WorkshopPlan.WP_Capacity <= 0)
                    {
                        TempData["TosterState"] = "info";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ظرفیت کارگاه مورد نظر پر شده است";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                    }

                    bool smsResult = true;
                    Tbl_Payment _Payment = Purchase(_User, _WorkshopPlan.WP_Cost, ProductType.Workshop, out bool walletResult, ref smsResult);

                    if (_Payment != null)
                    {
                        if (!walletResult)
                        {
                            TempData["TosterState"] = "error";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "کمبود موجودی کیف پول کاربر";

                            return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                        }

                        db.Tbl_Payment.Add(_Payment);

                        Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                        _Wallet.Wallet_Credit = _Payment.Payment_RemaingWallet - _Payment.Payment_Cost;
                        _Wallet.Wallet_ModifiedDate = DateTime.Now;

                        db.Entry(_Wallet).State = EntityState.Modified;

                        Tbl_UserWorkshopPlan _UserWorkshopPlan = new Tbl_UserWorkshopPlan
                        {
                            UWP_Guid = Guid.NewGuid(),
                            UWP_UserID = new Rep_User().Get_UserIDWithGUID(model.UserGuid),
                            UWP_WPID = model.WorkshopID,
                            Tbl_Payment = _Payment,
                            UWP_IsActive = true,
                            UWP_CreationDate = DateTime.Now,
                            UWP_ModifiedDate = DateTime.Now
                        };

                        db.Tbl_UserWorkshopPlan.Add(_UserWorkshopPlan);

                        _WorkshopPlan.WP_Capacity -= 1;

                        db.Entry(_WorkshopPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            if (!smsResult)
                            {
                                TempData["TosterState"] = "warning";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "خطا در ارسال پیامک";
                            }
                            else
                            {
                                TempData["TosterState"] = "success";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شد";
                            }

                            return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = model.WorkshopID });
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetPresence(int id)
        {
            var _UserWorkshopPlan = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == id).SingleOrDefault();

            if (_UserWorkshopPlan != null)
            {
                Model_UserWorkshopPlanSetPresence model = new Model_UserWorkshopPlanSetPresence()
                {
                    ID = id,
                    Presence = _UserWorkshopPlan.UWP_IsPresent
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPresence(Model_UserWorkshopPlanSetPresence model)
        {
            if (ModelState.IsValid)
            {
                var _UserWorkshopPlan = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault();

                if (_UserWorkshopPlan != null)
                {
                    _UserWorkshopPlan.UWP_IsPresent = model.Presence;
                    _UserWorkshopPlan.UWP_ModifiedDate = DateTime.Now;

                    db.Entry(_UserWorkshopPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                        return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault().UWP_WPID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //public ActionResult UnRegister(int? id)
        //{
        //    if (id != null)
        //    {
        //        Model_Message model = new Model_Message();

        //        var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == id).SingleOrDefault();

        //        if (q != null)
        //        {
        //            model.ID = id.Value;
        //            model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
        //            model.Description = $"آیا از لغو ثبت نام کاربر { model.Name } اطمینان دارید ؟";

        //            return PartialView(model);
        //        }
        //        else
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //    }

        //    return HttpNotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UnRegister(Model_Message model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault();

        //        if (q != null)
        //        {
        //            q.UWP_IsDelete = true;

        //            db.Entry(q).State = EntityState.Modified;

        //            if (Convert.ToBoolean(db.SaveChanges() > 0))
        //            {
        //                TempData["TosterState"] = "success";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

        //                return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault().UWP_WPID });
        //            }
        //            else
        //            {
        //                TempData["TosterState"] = "error";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

        //                return HttpNotFound();
        //            }
        //        }
        //    }

        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}

        //public ActionResult Register(int? id)
        //{
        //    if (id != null)
        //    {
        //        Model_Message model = new Model_Message();

        //        var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == id).SingleOrDefault();

        //        if (q != null)
        //        {
        //            model.ID = id.Value;
        //            model.Name = q.Tbl_User.User_FirstName + " " + q.Tbl_User.User_lastName;
        //            model.Description = $"آیا از ثبت نام مجدد کاربر { model.Name } اطمینان دارید ؟";

        //            return PartialView(model);
        //        }
        //        else
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //    }

        //    return HttpNotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(Model_Message model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var q = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault();

        //        if (q != null)
        //        {
        //            q.UWP_IsDelete = false;

        //            db.Entry(q).State = EntityState.Modified;

        //            if (Convert.ToBoolean(db.SaveChanges() > 0))
        //            {
        //                TempData["TosterState"] = "success";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

        //                return RedirectToAction("Details", "Workshop", new { area = "Dashboard", id = db.Tbl_UserWorkshopPlan.Where(x => x.UWP_ID == model.ID).SingleOrDefault().UWP_WPID });
        //            }
        //            else
        //            {
        //                TempData["TosterState"] = "error";
        //                TempData["TosterType"] = TosterType.Maseage;
        //                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

        //                return HttpNotFound();
        //            }
        //        }
        //    }

        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}

        private Tbl_Payment Purchase(Tbl_User user, int cost, ProductType type, out bool walletResult, ref bool smsResult)
        {
            Tbl_Payment _Payment;
            int credit = new Rep_Wallet().Get_WalletCreditWithUserID(user.User_ID);

            if (credit + 30000 < cost)
            {
                if (new SMSPortal().SendServiceable(user.User_Mobile, ".", "", "", user.User_FirstName + " " + user.User_lastName, SMSTemplate.Charge) != "ارسال به مخابرات")
                {
                    smsResult = false;
                }

                walletResult = false;
            }
            else
            {
                walletResult = true;
            }

            switch (type)
            {
                case ProductType.ExamInPerson:

                    _Payment = new Tbl_Payment
                    {
                        Payment_Guid = Guid.NewGuid(),
                        Payment_UserID = user.User_ID,
                        Payment_TitleCodeID = (int)PaymentTitle.ExamInPerson,
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

                case ProductType.ExamRemotely:

                    return null;

                case ProductType.Workshop:

                    _Payment = new Tbl_Payment
                    {
                        Payment_Guid = Guid.NewGuid(),
                        Payment_UserID = user.User_ID,
                        Payment_TitleCodeID = (int)PaymentTitle.Workshop,
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

                case ProductType.Class:

                    _Payment = new Tbl_Payment
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

                default:
                    return null;
            }
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
