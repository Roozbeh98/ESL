using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESL.DataLayer.Domain;

namespace ESL.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Exam()
        {
            var report = new StiReport();
            report.Load(Server.MapPath("/Content/Reports/Report.mrt"));
            report.Compile();
            report.RegBusinessObject("dt", new ESLEntities().Tbl_Menu.ToList());
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(HttpContext);
        }
    }
}