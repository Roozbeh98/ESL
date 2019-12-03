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
    public class UserExamInPersonController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

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
                var _User = db.Tbl_User.Where(x => x.User_Guid == model.UserGuid && x.User_IsDelete == false).SingleOrDefault();

                if (db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_UserID == _User.User_ID && x.UEIPP_EIPPID == model.ExamID && x.UEIPP_IsDelete == false).FirstOrDefault() != null)
                {
                    TempData["TosterState"] = "info";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "آزمون مورد نظر قبلا خریداری شده است";

                    return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                };

                var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ExamID).SingleOrDefault();

                if (_ExamInPersonPlan != null)
                {
                    bool smsResult = true;
                    Tbl_Payment _Payment = Purchase(_User, _ExamInPersonPlan.EIPP_Cost, ProductType.ExamInPerson, out bool walletResult, ref smsResult);

                    if (_Payment != null)
                    {
                        db.Tbl_Payment.Add(_Payment);

                        Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                        _Wallet.Wallet_Credit = _Payment.Payment_RemaingWallet - _Payment.Payment_Cost;
                        _Wallet.Wallet_ModifiedDate = DateTime.Now;

                        db.Entry(_Wallet).State = EntityState.Modified;

                        Tbl_UserExamInPersonPlan _UserExamInPersonPlan = new Tbl_UserExamInPersonPlan
                        {
                            UEIPP_Guid = Guid.NewGuid(),
                            UEIPP_UserID = new Rep_User().Get_UserIDWithGUID(model.UserGuid),
                            UEIPP_EIPPID = model.ExamID,
                            UEIPP_SeatNumber = 0,
                            Tbl_Payment = _Payment,
                            UEIPP_IsActive = true,
                            UEIPP_CreationDate = DateTime.Now,
                            UEIPP_ModifiedDate = DateTime.Now
                        };

                        db.Tbl_UserExamInPersonPlan.Add(_UserExamInPersonPlan);

                        _ExamInPersonPlan.EIPP_Capacity -= 1;

                        db.Entry(_ExamInPersonPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            //if (_ExamInPersonPlan.EIPP_Capacity <= 0)
                            //{
                            //    TempData["TosterState"] = "warning";
                            //    TempData["TosterType"] = TosterType.Maseage;
                            //    TempData["TosterMassage"] = "ظرفیت آزمون مورد نظر پر شده است";

                            //    return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                            //}

                            if (!walletResult && !smsResult)
                            {
                                TempData["TosterState"] = "warning";
                                TempData["TosterType"] = TosterType.WithTitel;
                                TempData["TosterTitel"] = "خطا در ارسال پیامک";
                                TempData["TosterMassage"] = "کمبود موجودی کیف پول کاربر";
                            }
                            else if (!walletResult)
                            {
                                TempData["TosterState"] = "warning";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "کمبود موجودی کیف پول کاربر";
                            }
                            else if (!smsResult)
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

                            return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = model.ExamID });
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetMark(int id)
        {
            var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (_UserExamInPersonPlan != null)
            {
                Model_UserExamInPersonPlanSetMark model = new Model_UserExamInPersonPlanSetMark()
                {
                    ID = id,
                    Mark = _UserExamInPersonPlan.UEIPP_Mark
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
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    _UserExamInPersonPlan.UEIPP_Mark = model.Mark;
                    _UserExamInPersonPlan.UEIPP_ModifiedDate = DateTime.Now;

                    db.Entry(_UserExamInPersonPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر نمره با موفقیت انجام شد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر نمره با موفقیت انجام نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetSeatNumber(int id)
        {
            var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (_UserExamInPersonPlan != null)
            {
                Model_UserExamInPersonPlanSetSeatNumber model = new Model_UserExamInPersonPlanSetSeatNumber()
                {
                    ID = id,
                    SeatNumber = _UserExamInPersonPlan.UEIPP_SeatNumber
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
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    _UserExamInPersonPlan.UEIPP_SeatNumber = model.SeatNumber;
                    _UserExamInPersonPlan.UEIPP_ModifiedDate = DateTime.Now;

                    db.Entry(_UserExamInPersonPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر شماره صندلی با موفقیت انجام شد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر شماره صندلی با موفقیت انجام نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetPresence(int id)
        {
            var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

            if (_UserExamInPersonPlan != null)
            {
                Model_UserExamInPersonPlanSetPresence model = new Model_UserExamInPersonPlanSetPresence()
                {
                    ID = id,
                    Presence = _UserExamInPersonPlan.UEIPP_IsPresent
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
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    _UserExamInPersonPlan.UEIPP_IsPresent = model.Presence;
                    _UserExamInPersonPlan.UEIPP_ModifiedDate = DateTime.Now;

                    db.Entry(_UserExamInPersonPlan).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام شد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تغییر وضعیت نمایش با موفقیت انجام نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GenerateSeatNumbers(int? id)
        {
            if (id != null)
            {
                var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault();

                if (_ExamInPersonPlan != null)
                {
                    Model_Message model = new Model_Message
                    {
                        ID = id.Value,
                        Name = _ExamInPersonPlan.Tbl_SubExamInPerson.SEIP_Title,
                        Description = "آیا از تخصیص شماره صندلی به افراد شرکت کننده در این آزمون اطمینان دارید ؟"
                    };

                    TempData["ExamInPersonPlanID"] = id.Value;

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
        public ActionResult GenerateSeatNumbers()
        {
            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                int examInPersonPlanId = (int)TempData["ExamInPersonPlanID"], seatNumber = 1;

                var _UsersExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_EIPPID == examInPersonPlanId).OrderBy(x => x.Tbl_User.User_lastName).ToList();

                if (_UsersExamInPersonPlan != null)
                {
                    foreach (var user in _UsersExamInPersonPlan)
                    {
                        user.UEIPP_SeatNumber = seatNumber++;
                        user.UEIPP_ModifiedDate = now;

                        db.Entry(user).State = EntityState.Modified;
                    }

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تخصیص شماره صندلی با موفقیت انجام شد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == examInPersonPlanId).SingleOrDefault().UEIPP_EIPPID });
                    }

                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "تخصیص شماره صندلی با موفقیت انجام نشد";

                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ExamInPersonCertificate(int id)
        {
            return PartialView();
        }

        public ActionResult UnRegister(int? id)
        {
            if (id != null)
            {
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    Model_Message model = new Model_Message();

                    model.ID = id.Value;
                    model.Name = _UserExamInPersonPlan.Tbl_User.User_FirstName + " " + _UserExamInPersonPlan.Tbl_User.User_lastName;
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
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == _UserExamInPersonPlan.UEIPP_PaymentID).SingleOrDefault();

                    if (_Payment != null)
                    {
                        int newCredit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) + _Payment.Payment_Cost;

                        _Payment.Payment_StateCodeID = (int)PaymentState.Rejected;
                        _Payment.Payment_WayCodeID = (int)PaymentWay.Internet;
                        _Payment.Payment_RemaingWallet = newCredit;
                        _Payment.Payment_ModifiedDate = DateTime.Now;

                        db.Entry(_Payment).State = EntityState.Modified;

                        Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                        _Wallet.Wallet_Credit = newCredit;
                        _Wallet.Wallet_ModifiedDate = DateTime.Now;

                        db.Entry(_Wallet).State = EntityState.Modified;

                        _UserExamInPersonPlan.UEIPP_IsActive = false;
                        _UserExamInPersonPlan.UEIPP_ModifiedDate = DateTime.Now;
                        _UserExamInPersonPlan.Tbl_ExamInPersonPlan.EIPP_Capacity += 1;

                        db.Entry(_UserExamInPersonPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            TempData["TosterState"] = "success";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت لغو شد";

                            return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت لغو نشد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                    }
                }

                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت لغو نشد";

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Register(int? id)
        {
            if (id != null)
            {
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == id).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    Model_Message model = new Model_Message();

                    model.ID = id.Value;
                    model.Name = _UserExamInPersonPlan.Tbl_User.User_FirstName + " " + _UserExamInPersonPlan.Tbl_User.User_lastName;
                    model.Description = $"آیا از ثبت نام کاربر { model.Name } اطمینان دارید ؟";

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
                var _UserExamInPersonPlan = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault();

                if (_UserExamInPersonPlan != null)
                {
                    var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == _UserExamInPersonPlan.UEIPP_PaymentID).SingleOrDefault();

                    if (_Payment != null)
                    {
                        int credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID);
                        int newCredit = credit - _Payment.Payment_Cost;

                        _Payment.Payment_StateCodeID = (int)PaymentState.Confirmed;
                        _Payment.Payment_WayCodeID = (int)PaymentWay.Internet;
                        _Payment.Payment_RemaingWallet = newCredit;
                        _Payment.Payment_ModifiedDate = DateTime.Now;

                        db.Entry(_Payment).State = EntityState.Modified;

                        Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                        _Wallet.Wallet_Credit = newCredit;
                        _Wallet.Wallet_ModifiedDate = DateTime.Now;

                        db.Entry(_Wallet).State = EntityState.Modified;

                        _UserExamInPersonPlan.UEIPP_IsActive = true;
                        _UserExamInPersonPlan.UEIPP_ModifiedDate = DateTime.Now;
                        _UserExamInPersonPlan.Tbl_ExamInPersonPlan.EIPP_Capacity -= 1;

                        db.Entry(_UserExamInPersonPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            //if (_UserExamInPersonPlan.Tbl_ExamInPersonPlan.EIPP_Capacity <= 0)
                            //{
                            //    TempData["TosterState"] = "warning";
                            //    TempData["TosterType"] = TosterType.Maseage;
                            //    TempData["TosterMassage"] = "ظرفیت آزمون مورد نظر پر شده است";

                            //    return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                            //}

                            if (credit + 30000 < _Payment.Payment_Cost)
                            {
                                if (new SMSPortal().SendServiceable(_Payment.Tbl_User.User_Mobile, ".", "", "", _Payment.Tbl_User.User_FirstName + " " + _Payment.Tbl_User.User_lastName, SMSTemplate.Charge) != "ارسال به مخابرات")
                                {
                                    TempData["TosterState"] = "warning";
                                    TempData["TosterType"] = TosterType.WithTitel;
                                    TempData["TosterTitel"] = "خطا در ارسال پیامک";
                                    TempData["TosterMassage"] = "کمبود موجودی کیف پول کاربر";
                                }
                                else
                                {
                                    TempData["TosterState"] = "warning";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "کمبود موجودی کیف پول کاربر";
                                }
                            }
                            else
                            {
                                TempData["TosterState"] = "success";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت انجام شد";
                            }

                            return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت انجام نشد";

                        return RedirectToAction("Details", "ExamInPerson", new { area = "Dashboard", id = db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_ID == model.ID).SingleOrDefault().UEIPP_EIPPID });
                    }
                }

                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت انجام نشد";

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

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
                        Payment_RemaingWallet = credit - cost,
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
                        Payment_RemaingWallet = credit - cost,
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
