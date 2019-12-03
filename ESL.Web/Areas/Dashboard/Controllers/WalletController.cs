using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WalletController : Controller
    {
        private readonly ESLEntities db = new ESLEntities();

        public ActionResult Index()
        {
            var q = db.Tbl_Wallet.Where(x => x.Tbl_User.User_RoleID == (int)Role.Student).Select(x => new Model_Wallet
            {
                ID = x.Wallet_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Credit = x.Wallet_Credit,
                CreationDate = x.Wallet_CreationDate,
                ModifiedDate = x.Wallet_ModifiedDate

            }).ToList();

            return View(q);
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
