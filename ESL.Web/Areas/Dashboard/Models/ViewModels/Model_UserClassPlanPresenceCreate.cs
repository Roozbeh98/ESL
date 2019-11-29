using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPlanPresenceCreate
    {
        [Display(Name = "تاریخ و زمان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Date { get; set; }

        [Display(Name = "وضعیت حضور")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public bool Presence { get; set; }

        [Display(Name = "تخفیف (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Discount { get; set; }
    }
}