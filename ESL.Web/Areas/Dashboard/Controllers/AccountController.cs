using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ESL.DataLayer.Domain;

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
    }
}






