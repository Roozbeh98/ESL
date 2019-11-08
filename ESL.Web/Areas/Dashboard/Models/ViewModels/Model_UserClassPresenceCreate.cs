using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPresenceCreate
    {
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Date { get; set; }

        [Display(Name = "وضعیت حضور")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public bool Presence { get; set; }

        [Display(Name = "تخفیف")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Discount { get; set; }
    }
}