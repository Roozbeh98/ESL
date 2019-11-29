using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPlanPresence
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "قیمت (تومان)")]
        public int Cost { get; set; }

        [Display(Name = "تخفیف (تومان)")]
        public int Discount { get; set; }

        [Display(Name = "وضعیت حضور")]
        public bool Presence { get; set; }

        [Display(Name = "تاریخ و زمان")]
        public DateTime Date { get; set; }
    }
}