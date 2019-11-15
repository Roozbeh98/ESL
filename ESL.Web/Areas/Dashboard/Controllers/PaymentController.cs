using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
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
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                Type = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate

            }).ToList();

            return View(q);
        }

        public ActionResult Suspended()
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code.Code_Name == "Suspended").Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Title = x.Tbl_Code.Code_Display,
                Type = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                Description = x.Payment_Description,
                Cost = x.Payment_Cost,
                Discount = x.Payment_Discount,
                RemaingWallet = x.Payment_RemaingWallet,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                CreateDate = x.Payment_CreateDate

            }).ToList();

            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_PaymentCreate model, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                Rep_Wallet _Wallet = new Rep_Wallet();
                int _Credit = _Wallet.Get_WalletCreditWithUserGUID(model.User), _NewCredit/*, _CostAfterDiscount = model.Cost - model.Discount*/;
                int _TitleCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Title);

                switch ((PaymentTitle)_TitleCodeID)
                {
                    case PaymentTitle.Discharge:

                        //if (_Credit + 50000 >= _CostAfterDiscount)
                        //{
                        //    TempData["TosterState"] = "error";
                        //    TempData["TosterType"] = TosterType.Maseage;
                        //    TempData["TosterMassage"] = "موجودی کیف پول کاربر مورد نظر کافی نمی باشد.";

                        //    return View();
                        //}

                        _NewCredit = _Credit - model.Cost;

                        break;

                    case PaymentTitle.ReturnToAccount:
                        _NewCredit = _Credit + model.Cost;
                        break;

                    case PaymentTitle.ReturnToBankAccount:
                        _NewCredit = _Credit;
                        break;

                    case PaymentTitle.Charge:
                        _NewCredit = _Credit + model.Cost;
                        break;

                    default:

                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return View();
                }

                Tbl_Document _Document = null;

                if (document != null)
                {
                    string path = Path.Combine(Server.MapPath("~/App_Data/Payment"), Path.GetFileName(document.FileName));
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
                        Document_Path = "Payment/" + document.FileName
                    };

                    db.Tbl_Document.Add(_Document);
                }

                int _UserID = new Rep_User().Get_UserIDWithGUID(model.User);

                Tbl_Payment _Payment = new Tbl_Payment
                {
                    Payment_Guid = Guid.NewGuid(),
                    Payment_UserID = _UserID,
                    Payment_TitleCodeID = _TitleCodeID,
                    Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way),
                    Payment_TypeCodeID = (int)PaymentType.Confirmed,
                    Payment_Description = model.Description,
                    Payment_Cost = model.Cost,
                    Payment_RemaingWallet = _NewCredit,
                    Payment_Discount = model.Discount,
                    Payment_TrackingToken = model.TrackingToken,
                    Payment_CreateDate = DateTime.Now
                };

                if (document != null)
                {
                    _Payment.Tbl_Document = _Document;
                }

                db.Tbl_Payment.Add(_Payment);

                if (_NewCredit != _Credit)
                {
                    Tbl_Wallet w = db.Tbl_Wallet.Where(x => x.Wallet_UserID == _UserID).SingleOrDefault();
                    w.Wallet_Credit = _NewCredit;
                    w.Wallet_ModifiedDate = DateTime.Now;

                    db.Entry(w).State = EntityState.Modified;
                }

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Index");
                }

                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Edit(int id)
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_PaymentEdit model = new Model_PaymentEdit()
                {
                    ID = q.Payment_ID,
                    User = q.Tbl_User.User_Guid,
                    Type = q.Tbl_Code.Code_Guid,
                    Way = q.Tbl_Code2.Code_Guid,
                    Cost = q.Payment_Cost,
                    TrackingToken = q.Payment_TrackingToken
                };

                return View(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model_PaymentEdit model, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                Tbl_Payment q = db.Tbl_Payment.Where(x => x.Payment_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    Tbl_Document p = null;

                    if (document != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/App_Data/Payment"), Path.GetFileName(document.FileName));
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

                        p = new Tbl_Document
                        {
                            Document_Guid = Guid.NewGuid(),
                            Document_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithName(filetype),
                            Document_Path = "Payment/" + document.FileName
                        };

                        db.Tbl_Document.Add(p);
                    }

                    q.Payment_UserID = new Rep_User().Get_UserIDWithGUID(model.User);
                    q.Payment_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);
                    q.Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way);
                    q.Payment_Cost = model.Cost;
                    //Payment_Discount = ,
                    //Payment_RemaingWallet = ,
                    q.Payment_TrackingToken = model.TrackingToken;

                    q.Payment_ModifiedDate = DateTime.Now;

                    if (document != null)
                    {
                        q.Tbl_Document = p;
                    }

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

                var exam = db.Tbl_Payment.Where(x => x.Payment_ID == id).FirstOrDefault();

                if (exam != null)
                {
                    model.ID = id.Value;
                    model.Name = exam.Payment_TrackingToken;
                    model.Description = "آیا از حذف تراکنش مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var exam = db.Tbl_Payment.Where(x => x.Payment_ID == model.ID).FirstOrDefault();

                if (exam != null)
                {
                    exam.Payment_IsDelete = true;

                    db.Entry(exam).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
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

        public ActionResult ChangeType(int id)
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_PaymentChangeType model = new Model_PaymentChangeType()
                {
                    ID = id,
                    Type = q.Tbl_Code.Code_Guid
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeState(Model_PaymentChangeType model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_Payment.Where(x => x.Payment_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Payment_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type);

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", "Payment", new { area = "Dashboard" });
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

        [Authorize(Roles = "Student")]
        public ActionResult ChargeWallet()
        {
            return View();
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