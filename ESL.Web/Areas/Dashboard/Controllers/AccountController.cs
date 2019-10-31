using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ESL.DataLayer.Domain;
using ESL.DataLayer.ViewModels;
using ESL.Web.Areas.Dashboard.Models.ViewModels;

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


            return View();
        }


    }
}






