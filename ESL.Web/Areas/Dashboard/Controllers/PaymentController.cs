using ESL.Common.Plugins;
using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
using ESL.Services.Services;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin, Student")]
    public class PaymentController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        #region Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                State = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate,
                ModifiedDate = x.Payment_ModifiedDate

            }).ToList();

            return View(_Payments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult WaitForAcceptance()
        {
            var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code1.Code_ID == (int)PaymentState.WaitForAcceptance).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                State = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate,
                ModifiedDate = x.Payment_ModifiedDate

            }).ToList();

            return View(_Payments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Confirmed()
        {
            var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code1.Code_ID == (int)PaymentState.Confirmed).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                State = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate,
                ModifiedDate = x.Payment_ModifiedDate

            }).ToList();

            return View(_Payments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Suspended()
        {
            var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code1.Code_ID == (int)PaymentState.Suspended).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                State = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate,
                ModifiedDate = x.Payment_ModifiedDate

            }).ToList();

            return View(_Payments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Rejected()
        {
            var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code1.Code_ID == (int)PaymentState.Rejected).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                State = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate,
                ModifiedDate = x.Payment_ModifiedDate

            }).ToList();

            return View(_Payments);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_PaymentCreate model, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                Rep_Wallet _Wallet = new Rep_Wallet();
                var _User = db.Tbl_User.Where(x => x.User_Guid == model.User).SingleOrDefault();

                int credit = _Wallet.Get_WalletCreditWithUserGUID(_User.User_Guid), newCredit;
                int titleCodeId = Rep_CodeGroup.Get_CodeIDWithGUID(model.Title);

                switch ((PaymentTitle)titleCodeId)
                {
                    case PaymentTitle.Discharge:
                        newCredit = credit - model.Cost;
                        break;

                    case PaymentTitle.ReturnToAccount:
                        newCredit = credit + model.Cost;
                        break;

                    case PaymentTitle.ReturnToBankAccount:
                        newCredit = credit;
                        break;

                    case PaymentTitle.Charge:
                        newCredit = credit + model.Cost;
                        break;

                    default:
                        return View();
                }

                Tbl_Document _Document = null;

                if (document != null)
                {
                    string path = Path.Combine(Server.MapPath("~/App_Data/Payment/Admin"), Path.GetFileName(document.FileName));
                    document.SaveAs(path);

                    string extention = Path.GetExtension(document.FileName);
                    string filetype;

                    switch (extention)
                    {
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                            filetype = "Image";
                            break;

                        case ".mp3":
                        case ".m4a":
                        case ".wav":
                            filetype = "Audio";
                            break;

                        case ".mp4":
                        case ".avi":
                        case ".mov":
                            filetype = "Video";
                            break;

                        case ".pdf":
                        case ".doc":
                            filetype = "File";
                            break;

                        default:
                            filetype = null;
                            break;
                    }

                    _Document = new Tbl_Document
                    {
                        Document_Guid = Guid.NewGuid(),
                        Document_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithName(filetype),
                        Document_Path = "Payment/Admin/" + document.FileName
                    };

                    db.Tbl_Document.Add(_Document);
                }

                Tbl_Payment _Payment = new Tbl_Payment
                {
                    Payment_Guid = Guid.NewGuid(),
                    Payment_UserID = _User.User_ID,
                    Payment_TitleCodeID = titleCodeId,
                    Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way),
                    Payment_StateCodeID = (int)PaymentState.Confirmed,
                    Payment_Description = model.Description,
                    Payment_Cost = model.Cost,
                    Payment_Discount = 0,
                    Payment_RemaingWallet = newCredit,
                    Payment_TrackingToken = model.TrackingToken,
                    Payment_CreateDate = DateTime.Now,
                    Payment_ModifiedDate = DateTime.Now
                };

                if (document != null)
                {
                    _Payment.Tbl_Document = _Document;
                }

                db.Tbl_Payment.Add(_Payment);

                if (newCredit != credit)
                {
                    Tbl_Wallet w = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _User.User_ID).SingleOrDefault();
                    w.Wallet_Credit = newCredit;
                    w.Wallet_ModifiedDate = DateTime.Now;

                    db.Entry(w).State = EntityState.Modified;
                }

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    if ((PaymentTitle)titleCodeId == PaymentTitle.Charge &&
                        new SMSPortal().SendServiceable(_User.User_Mobile, (_Payment.Payment_Cost * 10).ToString(), PersianDateExtensionMethods.ToPeString(DateTime.Now, "yyyy/MM/dd"), credit.ToString(), "", SMSTemplate.Success) != "ارسال به مخابرات")
                    {
                        TempData["TosterState"] = "warning";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "خطا در ارسال پیامک";
                    }
                    else
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "تراکنش جدید با موفقیت ثبت شد";
                    }

                    return RedirectToAction("Index");
                }

                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "تراکنش جدید با موفقیت ثبت نشد";

                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeState(int id)
        {
            var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == id).SingleOrDefault();

            if (_Payment != null)
            {
                Model_PaymentChangeState model = new Model_PaymentChangeState()
                {
                    ID = id,
                    State = _Payment.Tbl_Code1.Code_Guid,
                    Description = _Payment.Payment_Description,
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeState(Model_PaymentChangeState model)
        {
            if (ModelState.IsValid)
            {
                var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == model.ID).SingleOrDefault();

                if (_Payment != null)
                {
                    int stateCodeId = Rep_CodeGroup.Get_CodeIDWithGUID(model.State), credit;
                    Tbl_Wallet _Wallet;
                    Tbl_UserClassPlan _UserClassPlan;

                    switch ((PaymentTitle)_Payment.Payment_TitleCodeID)
                    {
                        case PaymentTitle.Charge:

                            switch ((PaymentState)_Payment.Payment_StateCodeID)
                            {
                                case PaymentState.WaitForAcceptance:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Confirmed:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) + _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                if (new SMSPortal().SendServiceable(_Payment.Tbl_User.User_Mobile, (_Payment.Payment_Cost * 10).ToString(), PersianDateExtensionMethods.ToPeString(DateTime.Now, "yyyy/MM/dd"), credit.ToString(), "", SMSTemplate.Success) != "ارسال به مخابرات")
                                                {
                                                    TempData["TosterState"] = "warning";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "خطا در ارسال پیامک";
                                                }
                                                else
                                                {
                                                    TempData["TosterState"] = "success";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";
                                                }

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Confirmed:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) - _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Rejected:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) - _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) - _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Rejected:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) + _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                if (new SMSPortal().SendServiceable(_Payment.Tbl_User.User_Mobile, (_Payment.Payment_Cost * 10).ToString(), PersianDateExtensionMethods.ToPeString(DateTime.Now, "yyyy/MM/dd"), credit.ToString(), "", SMSTemplate.Success) != "ارسال به مخابرات")
                                                {
                                                    TempData["TosterState"] = "warning";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "خطا در ارسال پیامک";
                                                }
                                                else
                                                {
                                                    TempData["TosterState"] = "success";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";
                                                }

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Suspended:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Suspended:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            credit = new Rep_Wallet().Get_WalletCreditWithUserID(_Payment.Payment_UserID) + _Payment.Payment_Cost;

                                            _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _Payment.Payment_UserID).SingleOrDefault();
                                            _Wallet.Wallet_Credit = credit;
                                            _Wallet.Wallet_ModifiedDate = DateTime.Now;

                                            db.Entry(_Wallet).State = EntityState.Modified;

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_RemaingWallet = credit;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                if (new SMSPortal().SendServiceable(_Payment.Tbl_User.User_Mobile, (_Payment.Payment_Cost * 10).ToString(), PersianDateExtensionMethods.ToPeString(DateTime.Now, "yyyy/MM/dd"), credit.ToString(), "", SMSTemplate.Success) != "ارسال به مخابرات")
                                                {
                                                    TempData["TosterState"] = "warning";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "خطا در ارسال پیامک";
                                                }
                                                else
                                                {
                                                    TempData["TosterState"] = "success";
                                                    TempData["TosterType"] = TosterType.Maseage;
                                                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";
                                                }

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                    }

                                    break;
                            }

                            break;

                        case PaymentTitle.Class:

                            switch ((PaymentState)_Payment.Payment_StateCodeID)
                            {
                                case PaymentState.WaitForAcceptance:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Confirmed:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            _UserClassPlan.UCP_IsActive = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity -= 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            //_UserClassPlan.UCP_IsDelete = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Confirmed:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            _UserClassPlan.UCP_IsActive = false;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity += 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Rejected:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            _UserClassPlan.UCP_IsActive = false;
                                            //_UserClassPlan.UCP_IsDelete = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity += 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            _UserClassPlan.UCP_IsActive = false;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity += 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Rejected:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            //_UserClassPlan.UCP_IsDelete = false;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            //_UserClassPlan.UCP_IsDelete = false;
                                            _UserClassPlan.UCP_IsActive = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity -= 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });

                                        case PaymentState.Suspended:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            //_UserClassPlan.UCP_IsDelete = false;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }
                                    }

                                    break;

                                case PaymentState.Suspended:

                                    switch ((PaymentState)stateCodeId)
                                    {
                                        case PaymentState.WaitForAcceptance:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Confirmed:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            _UserClassPlan.UCP_IsActive = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                                            _UserClassPlan.Tbl_ClassPlan.CP_Capacity -= 1;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Rejected:

                                            _Payment.Payment_StateCodeID = stateCodeId;
                                            _Payment.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                                            _Payment.Payment_Description = model.Description;
                                            _Payment.Payment_ModifiedDate = DateTime.Now;

                                            db.Entry(_Payment).State = EntityState.Modified;

                                            _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();
                                            //_UserClassPlan.UCP_IsDelete = true;
                                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;

                                            db.Entry(_UserClassPlan).State = EntityState.Modified;

                                            if (Convert.ToBoolean(db.SaveChanges() > 0))
                                            {
                                                TempData["TosterState"] = "success";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                                                return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                            }
                                            else
                                            {
                                                TempData["TosterState"] = "error";
                                                TempData["TosterType"] = TosterType.Maseage;
                                                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                                                return HttpNotFound();
                                            }

                                        case PaymentState.Suspended:

                                            return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
                                    }

                                    break;
                            }

                            break;
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Student

        [Authorize(Roles = "Student")]
        public ActionResult List()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_UserID == _User.User_ID).Select(x => new Model_Payment
                {
                    ID = x.Payment_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Title = x.Tbl_Code.Code_Display,
                    State = x.Tbl_Code1.Code_Display,
                    Way = x.Tbl_Code2.Code_Display,
                    Description = x.Payment_Description,
                    Cost = x.Payment_Cost,
                    Discount = x.Payment_Discount,
                    RemaingWallet = x.Payment_RemaingWallet,
                    TrackingToken = x.Payment_TrackingToken,
                    Document = x.Tbl_Document.Document_Path,
                    CreateDate = x.Payment_CreateDate,
                    ModifiedDate = x.Payment_ModifiedDate

                }).ToList();

                return View(_Payments);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult WaitForAcceptanceList()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_UserID == _User.User_ID && x.Tbl_Code1.Code_ID == (int)PaymentState.WaitForAcceptance).Select(x => new Model_Payment
                {
                    ID = x.Payment_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Title = x.Tbl_Code.Code_Display,
                    State = x.Tbl_Code1.Code_Display,
                    Way = x.Tbl_Code2.Code_Display,
                    Description = x.Payment_Description,
                    Cost = x.Payment_Cost,
                    Discount = x.Payment_Discount,
                    RemaingWallet = x.Payment_RemaingWallet,
                    TrackingToken = x.Payment_TrackingToken,
                    Document = x.Tbl_Document.Document_Path,
                    CreateDate = x.Payment_CreateDate,
                    ModifiedDate = x.Payment_ModifiedDate

                }).ToList();

                return View(_Payments);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult ConfirmedList()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_UserID == _User.User_ID && x.Tbl_Code1.Code_ID == (int)PaymentState.Confirmed).Select(x => new Model_Payment
                {
                    ID = x.Payment_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Title = x.Tbl_Code.Code_Display,
                    State = x.Tbl_Code1.Code_Display,
                    Way = x.Tbl_Code2.Code_Display,
                    Description = x.Payment_Description,
                    Cost = x.Payment_Cost,
                    Discount = x.Payment_Discount,
                    RemaingWallet = x.Payment_RemaingWallet,
                    TrackingToken = x.Payment_TrackingToken,
                    Document = x.Tbl_Document.Document_Path,
                    CreateDate = x.Payment_CreateDate,
                    ModifiedDate = x.Payment_ModifiedDate

                }).ToList();

                return View(_Payments);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult SuspendedList()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_UserID == _User.User_ID && x.Tbl_Code1.Code_ID == (int)PaymentState.Suspended).Select(x => new Model_Payment
                {
                    ID = x.Payment_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Title = x.Tbl_Code.Code_Display,
                    State = x.Tbl_Code1.Code_Display,
                    Way = x.Tbl_Code2.Code_Display,
                    Description = x.Payment_Description,
                    Cost = x.Payment_Cost,
                    Discount = x.Payment_Discount,
                    RemaingWallet = x.Payment_RemaingWallet,
                    TrackingToken = x.Payment_TrackingToken,
                    Document = x.Tbl_Document.Document_Path,
                    CreateDate = x.Payment_CreateDate,
                    ModifiedDate = x.Payment_ModifiedDate

                }).ToList();

                return View(_Payments);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult RejectedList()
        {
            var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

            if (_User != null)
            {
                var _Payments = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_UserID == _User.User_ID && x.Tbl_Code1.Code_ID == (int)PaymentState.Rejected).Select(x => new Model_Payment
                {
                    ID = x.Payment_ID,
                    User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                    Title = x.Tbl_Code.Code_Display,
                    State = x.Tbl_Code1.Code_Display,
                    Way = x.Tbl_Code2.Code_Display,
                    Description = x.Payment_Description,
                    Cost = x.Payment_Cost,
                    Discount = x.Payment_Discount,
                    RemaingWallet = x.Payment_RemaingWallet,
                    TrackingToken = x.Payment_TrackingToken,
                    Document = x.Tbl_Document.Document_Path,
                    CreateDate = x.Payment_CreateDate,
                    ModifiedDate = x.Payment_ModifiedDate

                }).ToList();

                return View(_Payments);
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "Student")]
        public ActionResult ChargeWallet()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChargeWallet(Model_ChargeWallet model, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();
                Tbl_Document _Document = null;

                if (document != null)
                {
                    string path = Path.Combine(Server.MapPath("~/App_Data/Payment/Student"), Path.GetFileName(document.FileName));
                    document.SaveAs(path);

                    string extention = Path.GetExtension(document.FileName);
                    string filetype;

                    switch (extention)
                    {
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                            filetype = "Image";
                            break;

                        case ".mp3":
                        case ".m4a":
                        case ".wav":
                            filetype = "Audio";
                            break;

                        case ".mp4":
                        case ".avi":
                        case ".mov":
                            filetype = "Video";
                            break;

                        case ".pdf":
                        case ".doc":
                            filetype = "File";
                            break;

                        default:
                            filetype = null;
                            break;
                    }

                    _Document = new Tbl_Document
                    {
                        Document_Guid = Guid.NewGuid(),
                        Document_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithName(filetype),
                        Document_Path = "Payment/Student/" + document.FileName
                    };

                    db.Tbl_Document.Add(_Document);
                }

                Tbl_Payment _Payment = new Tbl_Payment
                {
                    Payment_Guid = Guid.NewGuid(),
                    Payment_UserID = _User.User_ID,
                    Payment_TitleCodeID = (int)PaymentTitle.Charge,
                    Payment_StateCodeID = (int)PaymentState.WaitForAcceptance,
                    Payment_Cost = model.Cost,
                    Payment_RemaingWallet = new Rep_Wallet().Get_WalletCreditWithUserGUID(_User.User_Guid),
                    Payment_TrackingToken = model.TrackingToken,
                    Payment_CreateDate = DateTime.Now,
                    Payment_ModifiedDate = DateTime.Now
                };

                if (document != null)
                {
                    _Payment.Tbl_Document = _Document;
                }

                db.Tbl_Payment.Add(_Payment);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "درخواست شارژ با موفقیت ارسال شد";

                    return RedirectToAction("Index", "Dashboard");
                }

                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "درخواست شارژ با موفقیت ارسال نشد";

                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Cancel(int? id)
        {
            if (id != null)
            {
                var _Payment = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Payment_ID == id && x.Payment_StateCodeID == (int)PaymentState.WaitForAcceptance).FirstOrDefault();

                if (_Payment != null)
                {
                    Model_Message model = new Model_Message
                    {
                        ID = id.Value,
                        Name = _Payment.Payment_TrackingToken,
                        Description = "آیا از لغو درخواست مورد نظر اطمینان دارید ؟"
                    };

                    return PartialView(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == model.ID).FirstOrDefault();

                if (_Payment != null)
                {
                    _Payment.Payment_IsDelete = true;
                    _Payment.Payment_ModifiedDate = DateTime.Now;

                    db.Entry(_Payment).State = EntityState.Modified;

                    var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_PaymentID == _Payment.Payment_ID).SingleOrDefault();

                    if (_UserClassPlan != null)
                    {
                        _UserClassPlan.UCP_IsDelete = true;
                        _UserClassPlan.UCP_ModifiedDate = DateTime.Now;

                        db.Entry(_UserClassPlan).State = EntityState.Modified;
                    }

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "درخواست مورد نظر با موفقیت لغو شد";

                        return RedirectToAction("List", "Payment", new { area = "Dashboard" });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "درخواست مورد نظر با موفقیت لغو نشد";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Functions

        [Authorize(Roles = "Admin")]
        public JsonResult Get_Users(string searchTerm)
        {
            var q = db.Tbl_User.Where(a => a.User_IsDelete == false && a.User_RoleID == 1).Select(a => new { id = a.User_Guid, text = a.User_FirstName + " " + a.User_lastName });

            if (searchTerm != null)
            {
                q = q.Where(a => a.text.Contains(searchTerm)).Select(a => new { id = a.id, text = a.text });
            }

            return Json(q, JsonRequestBehavior.AllowGet);
        }

        #endregion

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
