using ESL.DataLayer.Domain;
using ESL.Web.Areas.Dashboard.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private ESLEntities db = new ESLEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var q = db.Tbl_Wallet.Select(x => new Model_Wallet
            {
                ID = x.Wallet_ID,
                User = x.Tbl_User.User_FirstName + " " + x.Tbl_User.User_lastName,
                Credit = x.Wallet_Credit,
                CreationDate = x.Wallet_CreationDate,
                ModifiedDate = x.Wallet_ModifiedDate

            }).ToList();

            return View(q);
        }
    }
}