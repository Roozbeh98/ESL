using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserWorkshopPlanSetPresence
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = " ")]
        public bool Presence { get; set; }
    }
}