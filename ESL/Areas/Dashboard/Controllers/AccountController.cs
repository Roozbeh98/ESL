using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESL.DataLayer.Domain;

namespace ESL.Areas.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        ESLEntities db = new ESLEntities();

        #region log in/out

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Dashboard" });

            }

            return View();
        }

        //[HttpPost]
        //public ActionResult Login(LoginModel model)
        //{
        //    //if (Session.Count > 0)
        //    //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Dashboard", "Dashboard");

        //    }

        //    //if (Session["User"] != null)
        //    //{
        //    //    return RedirectToAction("Dashboard", "Dashboard");
        //    //}
        //    //}

        //    if (!ModelState.IsValid)
        //    {

        //        ViewBag.State = "Error";

        //        return View("Login", model);
        //    }


        //    var qlogin = (from a in db.Tbl_Login where a.Login_UserName == model.Username select a).SingleOrDefault();
        //    if (qlogin != null)
        //    {
        //        if (qlogin.Login_UserActive)
        //        {
        //            if (qlogin.Login_RegisterActive)
        //            {

        //                if (qlogin.Tbl_RegisterCode.RegisterCode_Code == model.Password)
        //                {
        //                    if (qlogin.Tbl_RegisterCode.RegisterCode_Date.AddDays(-5) < DateTime.Now)
        //                    {
        //                        Session["User"] = qlogin.Login_UserName;
        //                        Session["Role"] = qlogin.Login_BaseRoleID;
        //                        Session["Register"] = "Active";

        //                        return RedirectToAction("Register", "Account");

        //                    }
        //                    else
        //                    {
        //                        //err
        //                        ViewBag.Message = "کد ثبت نام منقضی شده است ! لطفا برای دریافت کد جدید به کتاب خونه مراجعه کنید";
        //                        ViewBag.State = "Error";
        //                        return View();
        //                    }
        //                }

        //                else
        //                {
        //                    //err
        //                    ViewBag.Message = "کد ثبت نام نادرست است!";
        //                    ViewBag.State = "Error";
        //                    return View();


        //                }
        //            }
        //            else
        //            {

        //                var SaltPassword = model.Password + qlogin.Login_PasswordSalt;
        //                var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
        //                var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));


        //                if (qlogin.Login_PasswordHush == SaltPasswordHush)
        //                {
        //                    string s = string.Empty;

        //                    Models.UserManagement.Membership Role = new Models.UserManagement.Membership();

        //                    var r = Role.GetRoles(model.Username);

        //                    if (r.Count > 0)
        //                    {
        //                        s = string.Join(",", r);
        //                    }



        //                    var Ticket = new FormsAuthenticationTicket(0, model.Username, DateTime.Now, model.RemenberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1), true, s);
        //                    var EncryptedTicket = FormsAuthentication.Encrypt(Ticket);
        //                    var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket)
        //                    {
        //                        Expires = Ticket.Expiration
        //                        // Domain =

        //                    };
        //                    Response.Cookies.Add(Cookie);
        //                    return RedirectToAction("Dashboard", "Dashboard");
        //                }
        //                else
        //                {
        //                    //err
        //                    ViewBag.Message = "پسورد نادرست است !";
        //                    ViewBag.State = "Error";
        //                    return View();

        //                }
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "این کاربر غیر فعال است ! لطفا به کتابخوانه مراجعه فرمایید";
        //            ViewBag.State = "Error";
        //            return View();
        //        }

        //    }
        //    else
        //    {
        //        //err
        //        ViewBag.Message = "نام کاربری نادرست است !";
        //        ViewBag.State = "Error";
        //        return View();

        //    }

        //}


        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
        //    {
        //        Expires = DateTime.Now.AddDays(-1)
        //    };

        //    Response.Cookies.Add(Cookie);
        //    Session.RemoveAll();

        //    return RedirectToAction("Login", "Account");

        //}


        #endregion
    }
}