using ESL.Web.Areas.Dashboard.Models.ViewModels;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private static Model_ExamInPersonCertificate data;

        public ActionResult PrintExamInPersonCertificate(Model_ExamInPersonCertificate model)
        {
            data = model;
            return View();
        }

        [HttpPost]
        public ActionResult PrintExamInPersonCertificate()
        {
            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/ExamInPersonCertificate.mrt"));
            report.Compile();
            report.RegBusinessObject("dt", data);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(HttpContext);
        }
    }
}
