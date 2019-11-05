using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class PageController : Controller
    {
        // GET: Dashboard/Page
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}