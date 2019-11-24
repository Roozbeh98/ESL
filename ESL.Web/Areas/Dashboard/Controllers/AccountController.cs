using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using ESL.Services.BaseRepository;
using ESL.Services.Services;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        ESLEntities db = new ESLEntities();

        [HttpGet]
        public ActionResult Login()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Dashboard", "Dashboard");

            //}
            return View();
        }

        [HttpPost]
        public ActionResult Login (Model_Login model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Dashboard");

            }


            if (!ModelState.IsValid)
            {

                ViewBag.State = "Error";

                return View("Login", model);



            }
            var q = db.Tbl_User.Where(a => a.User_Email == model.Username || a.User_Mobile == model.Username).SingleOrDefault();


            var SaltPassword = model.Password + q.User_PasswordSalt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));


            if (q.User_PasswordHash == SaltPasswordHush)
            {
                string s = string.Empty;



                s = Rep_UserRole.Get_RoleNameWithID(q.User_RoleID);


                var Ticket = new FormsAuthenticationTicket(0, model.Username, DateTime.Now, model.RemenberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1), true, s);
                var EncryptedTicket = FormsAuthentication.Encrypt(Ticket);
                var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket)
                {
                    Expires = Ticket.Expiration
                    // Domain =

                };
                Response.Cookies.Add(Cookie);
                return RedirectToAction("index", "Dashboard" , new { area = "Dashboard" });
            }
            else
            {
                //err
                ViewBag.Message = "پسورد نادرست است !";
                ViewBag.State = "Error";
                return View();

            }

        }


     
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            Response.Cookies.Add(Cookie);
            Session.RemoveAll();

            return RedirectToAction("Login", "Account", new { area = "Dashboard" });
        }

        [HttpPost]
        public ActionResult _Register(Model_Register model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }


            Tbl_User _User = new Tbl_User();

            _User.User_Guid = Guid.NewGuid();
            _User.User_Email = model.Email;
            _User.User_FirstName = model.Name;
            _User.User_lastName = model.Family;
            _User.User_Mobile = model.Mobile;
            _User.User_IdentityNumber = model.IdentityNumber;
            _User.User_RoleID = 1;
            _User.User_GenderCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(Guid.Parse( model.Gender));

            var Salt = Guid.NewGuid().ToString("N");
            var SaltPassword = model.Password + Salt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));

            _User.User_PasswordHash = SaltPasswordHush;
            _User.User_PasswordSalt = Salt;

            db.Tbl_User.Add(_User);

            Tbl_Wallet _Wallet = new Tbl_Wallet()
            {
                Wallet_Guid = Guid.NewGuid(),
                Wallet_CreationDate = DateTime.Now,
                Wallet_ModifiedDate = DateTime.Now,
                Tbl_User = _User
            };

            db.Tbl_Wallet.Add(_Wallet);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                if (new SMSPortal().SendServiceable(model.Mobile, model.Mobile, model.Password, "", model.Name + " " + model.Family, SMSTemplate.Register) != "ارسال به مخابرات")
                {
                    TempData["TosterState"] = "warning";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا در ارسال پیامک";

                    return RedirectToAction("Login");
                };

                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شد";

                return RedirectToAction("Login");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                return View();
            }
        }
    }
}






