using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Documents
        public ActionResult Index(int id)
        {
            return View();
        }
    }
}