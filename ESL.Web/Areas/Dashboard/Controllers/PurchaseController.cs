using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
using ESL.Services.Services;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Student")]
    public class PurchaseController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            List<Model_Purchase> model = new List<Model_Purchase>();

            var _Workshop = db.Tbl_WorkshopPlan.Where(x => x.WP_IsDelete == false && x.WP_IsActive == true).Select(x => new Model_Purchase
            {
                ID = x.WP_ID,
                Name = x.Tbl_SubWorkshop.Tbl_Workshop.Workshop_Title,
                SubName = x.Tbl_SubWorkshop.SW_Title,
                Description = x.WP_Description,
                Capacity = x.WP_Capacity,
                Location = x.WP_Location,
                Date = x.WP_Date,
                Cost = x.WP_Cost,
                Type = (int)ProductType.Workshop

            }).ToList();

            var _Class = db.Tbl_ClassPlan.Where(x => x.CP_IsDelete == false && x.CP_IsActive == true).Select(x => new Model_Purchase
            {
                ID = x.CP_ID,
                Name = x.Tbl_Class.Class_Title,
                Description = x.CP_Description,
                Capacity = x.CP_Capacity,
                Location = x.CP_Location,
                Time = x.CP_Time,
                Date = x.CP_StartDate,
                Cost = x.CP_CostPerSession,
                Type = (int)ProductType.Class

            }).ToList();

            var _ExamInPerson = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_IsDelete == false && x.EIPP_IsActive == true).Select(x => new Model_Purchase
            {
                ID = x.EIPP_ID,
                Name = x.Tbl_SubExamInPerson.Tbl_ExamInPerson.EIP_Title,
                SubName = x.Tbl_SubExamInPerson.SEIP_Title,
                Description = x.EIPP_Description,
                Capacity = x.EIPP_Capacity,
                Location = x.EIPP_Location,
                TotalMark = x.EIPP_TotalMark,
                PassMark = x.EIPP_PassMark,
                Date = x.EIPP_Date,
                Cost = x.EIPP_Cost,
                Type = (int)ProductType.ExamInPerson

            }).ToList();

            //var _ExamRemotely = db.Tbl_ExamRemotely.Where(x => x.ER_IsDelete == false).Select(x => new Model_Purchase
            //{
            //    ID = x.EIP_ID,
            //    Name = x.Tbl_SubExam.Tbl_Exam.Exam_Title,
            //    SubName = x.Tbl_SubExam.SE_Title,
            //    Description = x.EIP_Description,
            //    Capacity = x.EIP_Capacity,
            //    Location = x.EIP_Location,
            //    TotalMark = x.EIP_TotalMark,
            //    PassMark = x.EIP_PassMark,
            //    Date = x.EIP_Date,
            //    Cost = x.EIP_CostS,
            //    Type = (int)ProductType.ExamRemotely

            //}).ToList();

            model.AddRange(_Workshop);
            model.AddRange(_Class);
            model.AddRange(_ExamInPerson);
            //model.AddRange(_ExamRemotely);

            return View(model);
        }

        public ActionResult Pay(int? id, int type)
        {
            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                switch ((ProductType)type)
                {
                    case ProductType.Workshop:

                        var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == id).SingleOrDefault();

                        if (_WorkshopPlan != null)
                        {
                            model.ID = id.Value;
                            model.Name = _WorkshopPlan.Tbl_SubWorkshop.SW_Title;
                            model.Description = "آیا از خرید کارگاه مورد نظر اطمینان دارید ؟";

                            return PartialView(model);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }

                    case ProductType.Class:

                        var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == id).SingleOrDefault();

                        if (_ClassPlan != null)
                        {
                            model.ID = id.Value;
                            model.Name = _ClassPlan.Tbl_Class.Class_Title;
                            model.Description = "آیا از خرید کلاس مورد نظر اطمینان دارید ؟";

                            return PartialView(model);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }

                    case ProductType.ExamInPerson:

                        var _UserExamInPerson = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == id).SingleOrDefault(); ;

                        if (_UserExamInPerson != null)
                        {
                            model.ID = id.Value;
                            model.Name = _UserExamInPerson.Tbl_SubExamInPerson.SEIP_Title;
                            model.Description = "آیا از خرید آزمون حضوری مورد نظر اطمینان دارید ؟";

                            return PartialView(model);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }

                    case ProductType.ExamRemotely:

                        var _UserExam = db.Tbl_ExamRemotelyPlan.Where(x => x.ERP_ID  == id).SingleOrDefault();

                        if (_UserExam != null)
                        {
                            model.ID = id.Value;
                            model.Name = _UserExam.ERP_Title;
                            model.Description = "آیا از خرید آزمون غیر حضوری مورد نظر اطمینان دارید ؟";

                            return PartialView(model);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }

                    default:
                        return HttpNotFound();
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(Model_PurchasePay model)
        {
            if (ModelState.IsValid)
            {
                var _User = db.Tbl_User.Where(x => x.User_IsDelete == false && x.User_Mobile == User.Identity.Name).SingleOrDefault();

                if (_User != null)
                {
                    switch ((ProductType)model.Type)
                    {
                        case ProductType.Workshop:

                            if (db.Tbl_UserWorkshopPlan.Where(x => x.UWP_UserID == _User.User_ID && x.UWP_WPID == model.ID && x.UWP_IsDelete == false).FirstOrDefault() != null)
                            {
                                TempData["TosterState"] = "info";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                                return RedirectToAction("Index");
                            };

                            var _WorkshopPlan = db.Tbl_WorkshopPlan.Where(x => x.WP_ID == model.ID).SingleOrDefault();

                            if (_WorkshopPlan != null)
                            {
                                Tbl_Payment _Payment = StudentPurchase(_User, _WorkshopPlan.WP_Cost);

                                if (_Payment != null)
                                {
                                    db.Tbl_Payment.Add(_Payment);

                                    Tbl_UserWorkshopPlan _UserWorkshopPlan = new Tbl_UserWorkshopPlan()
                                    {
                                        UWP_Guid = Guid.NewGuid(),
                                        UWP_UserID = _User.User_ID,
                                        UWP_WPID = model.ID,
                                        Tbl_Payment = _Payment,
                                        UWP_CreationDate = DateTime.Now,
                                        UWP_ModifiedDate = DateTime.Now
                                    };

                                    db.Tbl_UserWorkshopPlan.Add(_UserWorkshopPlan);

                                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                                    {
                                        TempData["TosterState"] = "success";
                                        TempData["TosterType"] = TosterType.Maseage;
                                        TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال شد.";

                                        return RedirectToAction("Index");
                                    }

                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال نشد.";
                                }
                                else
                                {
                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "موجودی کیف پول شما کافی نمی باشد.";
                                }

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }

                        case ProductType.Class:

                            if (db.Tbl_UserClassPlan.Where(x => x.UCP_UserID == _User.User_ID && x.UCP_CPID == model.ID && x.UCP_IsDelete == false).FirstOrDefault() != null)
                            {
                                TempData["TosterState"] = "info";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                                return RedirectToAction("Index");
                            };

                            var _ClassPlan = db.Tbl_ClassPlan.Where(x => x.CP_ID == model.ID).SingleOrDefault();

                            if (_ClassPlan != null)
                            {
                                Tbl_Payment _Payment = StudentPurchase(_User, _ClassPlan.CP_CostPerSession);

                                if (_Payment != null)
                                {
                                    db.Tbl_Payment.Add(_Payment);

                                    Tbl_UserClassPlan _UserClassPlan = new Tbl_UserClassPlan()
                                    {
                                        UCP_Guid = Guid.NewGuid(),
                                        UCP_UserID = _User.User_ID,
                                        UCP_CPID = model.ID,
                                        Tbl_Payment = _Payment,
                                        UCP_CreationDate = DateTime.Now,
                                        UCP_ModifiedDate = DateTime.Now
                                    };

                                    db.Tbl_UserClassPlan.Add(_UserClassPlan);

                                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                                    {
                                        TempData["TosterState"] = "success";
                                        TempData["TosterType"] = TosterType.Maseage;
                                        TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال شد.";

                                        return RedirectToAction("Index");
                                    }

                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال نشد.";
                                }
                                else
                                {
                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "موجودی کیف پول شما کافی نمی باشد.";
                                }

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }

                        case ProductType.ExamInPerson:

                            if (db.Tbl_UserExamInPersonPlan.Where(x => x.UEIPP_UserID == _User.User_ID && x.UEIPP_EIPPID == model.ID && x.UEIPP_IsDelete == false).FirstOrDefault() != null)
                            {
                                TempData["TosterState"] = "info";
                                TempData["TosterType"] = TosterType.Maseage;
                                TempData["TosterMassage"] = "قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                                return RedirectToAction("Index");
                            };

                            var _ExamInPersonPlan = db.Tbl_ExamInPersonPlan.Where(x => x.EIPP_ID == model.ID).SingleOrDefault();

                            if (_ExamInPersonPlan != null)
                            {
                                Tbl_Payment _Payment = StudentPurchase(_User, _ExamInPersonPlan.EIPP_Cost);

                                if (_Payment != null)
                                {
                                    db.Tbl_Payment.Add(_Payment);

                                    Tbl_UserExamInPersonPlan _UserExamInPersonPlan = new Tbl_UserExamInPersonPlan()
                                    {
                                        UEIPP_Guid = Guid.NewGuid(),
                                        UEIPP_UserID = _User.User_ID,
                                        UEIPP_EIPPID = model.ID,
                                        UEIPP_SeatNumber = new Random().Next(10000, 99999),
                                        Tbl_Payment = _Payment,
                                        UEIPP_CreationDate = DateTime.Now,
                                        UEIPP_ModifiedDate = DateTime.Now
                                    };

                                    db.Tbl_UserExamInPersonPlan.Add(_UserExamInPersonPlan);

                                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                                    {
                                        TempData["TosterState"] = "success";
                                        TempData["TosterType"] = TosterType.Maseage;
                                        TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال شد.";

                                        return RedirectToAction("Index");
                                    }

                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال نشد.";
                                }
                                else
                                {
                                    TempData["TosterState"] = "error";
                                    TempData["TosterType"] = TosterType.Maseage;
                                    TempData["TosterMassage"] = "موجودی کیف پول شما کافی نمی باشد.";
                                }

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }

                        case ProductType.ExamRemotely:

                            //if (db.Tbl_UserExamRemotelyPlan.Where(x => x.UERP_UserID == _User.User_ID && x.UERP_ERPID == model.ID && x.UERP_IsDelete == false).FirstOrDefault() != null)
                            //{
                            //    TempData["TosterState"] = "info";
                            //    TempData["TosterType"] = TosterType.Maseage;
                            //    TempData["TosterMassage"] = "قبلا خریداری شده است و هم اکنون فعال یا در انتظار تایید می باشد";

                            //    return RedirectToAction("Index");
                            //};

                            //var _ExamRemotelyPlan = db.Tbl_ExamRemotelyPlan.Where(x => x.ERP_ID == model.ID).SingleOrDefault();

                            //if (_ExamRemotelyPlan != null)
                            //{
                            //    Tbl_Payment _Payment = StudentPurchase(_User, _ExamRemotelyPlan.cost);

                            //    if (_Payment != null)
                            //    {
                            //        db.Tbl_Payment.Add(_Payment);

                            //        Tbl_UserClassPlan _UserClassPlan = new Tbl_UserClassPlan()
                            //        {
                            //            UCP_Guid = Guid.NewGuid(),
                            //            UCP_UserID = _User.User_ID,
                            //            UCP_CPID = model.ID,
                            //            Tbl_Payment = _Payment,
                            //            UCP_CreationDate = DateTime.Now,
                            //            UCP_ModifiedDate = DateTime.Now
                            //        };

                            //        db.Tbl_UserClassPlan.Add(_UserClassPlan);

                            //        if (Convert.ToBoolean(db.SaveChanges() > 0))
                            //        {
                            //            TempData["TosterState"] = "success";
                            //            TempData["TosterType"] = TosterType.Maseage;
                            //            TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال شد.";

                            //            return RedirectToAction("Index");
                            //        }

                            //        TempData["TosterState"] = "error";
                            //        TempData["TosterType"] = TosterType.Maseage;
                            //        TempData["TosterMassage"] = "درخواست خرید با موفقیت ارسال نشد.";
                            //    }
                            //    else
                            //    {
                            //        TempData["TosterState"] = "error";
                            //        TempData["TosterType"] = TosterType.Maseage;
                            //        TempData["TosterMassage"] = "موجودی کیف پول شما کافی نمی باشد.";
                            //    }

                            //    return RedirectToAction("Index");
                            //}
                            //else
                            //{
                            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            //}

                            break;

                        default:
                            return HttpNotFound();
                    }
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private Tbl_Payment StudentPurchase(Tbl_User user, int cost)
        {
            int credit = new Rep_Wallet().Get_WalletCreditWithUserID(user.User_ID);

            if (credit + 30000 < cost)
            {
                if (new SMSPortal().SendServiceable(user.User_Mobile, ".", "", "", user.User_FirstName + " " + user.User_lastName, SMSTemplate.Charge) != "ارسال به مخابرات")
                {
                    TempData["TosterState"] = "warning";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا در ارسال پیامک";
                };

                return null;
            }

            Tbl_Payment _Payment = new Tbl_Payment
            {
                Payment_Guid = Guid.NewGuid(),
                Payment_UserID = user.User_ID,
                Payment_TitleCodeID = (int)PaymentTitle.Class,
                Payment_WayCodeID = (int)PaymentWay.Internet,
                Payment_StateCodeID = (int)PaymentState.WaitForAcceptance,
                Payment_Cost = cost,
                Payment_Discount = 0,
                Payment_RemaingWallet = credit,
                Payment_TrackingToken = "ESL-" + new Random().Next(100000, 999999).ToString(),
                Payment_CreateDate = DateTime.Now,
                Payment_ModifiedDate = DateTime.Now
            };

            return _Payment;
        }
    }
}
