using ESL.DataLayer.Domain;
using ESL.Services.BaseRepository;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false).Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Type = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                State = x.Tbl_Code.Code_Display,
                Cost = x.Payment_Cost,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                Date = x.Payment_Date

            }).ToList();

            return View(q);
        }

        public ActionResult Suspended()
        {
            var q = db.Tbl_Payment.Where(x => x.Payment_IsDelete == false && x.Tbl_Code.Code_Name == "Suspended").Select(x => new Model_Payment
            {
                ID = x.Payment_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Type = x.Tbl_Code1.Code_Display,
                Way = x.Tbl_Code2.Code_Display,
                State = x.Tbl_Code.Code_Display,
                Cost = x.Payment_Cost,
                TrackingToken = x.Payment_TrackingToken,
                Document = x.Tbl_Document.Document_Path,
                Date = x.Payment_Date

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

                Tbl_Document p = new Tbl_Document
                {
                    Document_Guid = Guid.NewGuid(),
                    Document_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithName(filetype),
                    Document_Path = "Payment/" + document.FileName
                };

                db.Tbl_Document.Add(p);

                Tbl_Payment q = new Tbl_Payment
                {
                    Payment_Guid = Guid.NewGuid(),
                    Payment_UserID = new Rep_User().Get_UserIDWithGUID(model.User),
                    Payment_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Type),
                    Payment_WayCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Way),
                    Payment_StateCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.State),
                    Payment_Cost = model.Cost,
                    Payment_TrackingToken = model.TrackingToken,
                    Payment_Date = DateTime.Now
                };

                q.Tbl_Document = p;
                db.Tbl_Payment.Add(q);

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

    }
}