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

                Tbl_UserClassPlan _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_UserID == _User.User_ID && x.UCP_CPID == model.ClassID).FirstOrDefault();

                if (_UserClassPlan != null && !_UserClassPlan.UCP_IsDelete)
                {
                    TempData["TosterState"] = "info";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "کلاس مورد نظر قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                    return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                };

                var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ClassID).SingleOrDefault();

                if (_ClassPlan != null)
                {
                    bool smsResult = true;
                    Tbl_Payment _Payment = Purchase(_User, _ClassPlan.CP_CostPerSession, ProductType.Class, out bool walletResult, ref smsResult);

                    if (_Payment != null)
                    {
                        db.Tbl_Payment.Add(_Payment);

                        if (_UserClassPlan != null)
                        {
                            _UserClassPlan.UCP_IsDelete = false;
                            _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                            _UserClassPlan.Tbl_Payment = _Payment;

                            db.Entry(_UserClassPlan).State = EntityState.Modified;
                        }
                        else
                        {
                            _UserClassPlan = new Tbl_UserClassPlan()
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
                        }

                        _ClassPlan.CP_Capacity -= 1;

                        db.Entry(_ClassPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            if (_ClassPlan.CP_Capacity <= 0)
                            {
                                TempData["TosterState"] = "warning";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ظرفیت کلاس مورد نظر پر شده است";
                            }
                            else
                            {
                                TempData["TosterState"] = "success";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شد";
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
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = model.ClassID });
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult UnRegister(int? id)
        {
            if (id != null)
            {
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == id).SingleOrDefault();

                if (_UserClassPlan != null)
                {
                    Model_Message model = new Model_Message();

                    model.ID = id.Value;
                    model.Name = _UserClassPlan.Tbl_User.User_FirstName + " " + _UserClassPlan.Tbl_User.User_lastName;
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
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault();

                if (_UserClassPlan != null)
                {
                    var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == _UserClassPlan.UCP_PaymentID).SingleOrDefault();

                    if (_Payment != null)
                    {
                        _Payment.Payment_StateCodeID = (int)PaymentState.Rejected;
                        _Payment.Payment_WayCodeID = (int)PaymentWay.Internet;
                        _Payment.Payment_ModifiedDate = DateTime.Now;

                        db.Entry(_Payment).State = EntityState.Modified;

                        _UserClassPlan.UCP_IsActive = false;
                        _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                        _UserClassPlan.Tbl_ClassPlan.CP_Capacity += 1;

                        db.Entry(_UserClassPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            TempData["TosterState"] = "success";
                            TempData["TosterType"] = TosterType.Maseage;
                            TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت لغو شد";

                            return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت لغو نشد";

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
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
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == id).SingleOrDefault();

                if (_UserClassPlan != null)
                {
                    Model_Message model = new Model_Message();

                    model.ID = id.Value;
                    model.Name = _UserClassPlan.Tbl_User.User_FirstName + " " + _UserClassPlan.Tbl_User.User_lastName;
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
                var _UserClassPlan = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault();
                
                if (_UserClassPlan != null)
                {
                    var _Payment = db.Tbl_Payment.Where(x => x.Payment_ID == _UserClassPlan.UCP_PaymentID).SingleOrDefault();

                    if (_Payment != null)
                    {
                        _Payment.Payment_StateCodeID = (int)PaymentState.Confirmed;
                        _Payment.Payment_WayCodeID = (int)PaymentWay.Internet;
                        _Payment.Payment_ModifiedDate = DateTime.Now;

                        db.Entry(_Payment).State = EntityState.Modified;

                        _UserClassPlan.UCP_IsActive = true;
                        _UserClassPlan.UCP_ModifiedDate = DateTime.Now;
                        _UserClassPlan.Tbl_ClassPlan.CP_Capacity -= 1;

                        db.Entry(_UserClassPlan).State = EntityState.Modified;

                        if (Convert.ToBoolean(db.SaveChanges() > 0))
                        {
                            if (_UserClassPlan.Tbl_ClassPlan.CP_Capacity <= 0)
                            {
                                TempData["TosterState"] = "warning";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ظرفیت کلاس مورد نظر پر شده است";
                            }
                            else
                            {
                                TempData["TosterState"] = "success";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت انجام شد";
                            }

                            return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
                        }

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نام کاربر مورد نظر با موفقیت انجام نشد";

                        return RedirectToAction("Details", "Class", new { area = "Dashboard", id = db.Tbl_UserClassPlan.Where(x => x.UCP_ID == model.ID).SingleOrDefault().UCP_CPID });
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
