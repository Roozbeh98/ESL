using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserExamInPersonPlanSetMark
    {
        [Display(Name = "شناسه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int ID { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Mark { get; set; }
    }
}