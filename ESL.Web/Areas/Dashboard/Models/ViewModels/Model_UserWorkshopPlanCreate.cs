using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserWorkshopPlanCreate
    {
        [Display(Name = "شناسه کارگاه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int WorkshopID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid UserGuid { get; set; }
    }
}