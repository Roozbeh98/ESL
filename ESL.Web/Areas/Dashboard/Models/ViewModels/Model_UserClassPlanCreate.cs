using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPlanCreate
    {
        [Display(Name = "شناسه کلاس")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int ClassID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid UserGuid { get; set; }
    }
}