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
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Dashboard", "Dashboard");

            //}
            return View();
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

            _User.User_Email = model.Email;
            _User.User_FirstName = model.Name;
            _User.User_lastName = model.Family;
            _User.User_Mobile = model.Mobile;
            _User.User_IdentityNumber = model.IdentityNumber;
            _User.User_RoleID = 1;
            _User.User_GenderCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(model.Gender);

            var Salt = Guid.NewGuid().ToString("N");
            var SaltPassword = model.Password + Salt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));

            _User.User_PasswordHash = SaltPasswordHush;
            _User.User_PasswordSalt = Salt;

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {

                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شده";

                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }

         
        }


    }
}






