using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserWorkshopCreate
    {
        [Display(Name = "شناسه کارگاه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int WorkshopID { get; set; }

        [Display(Name = "شناسه کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid UserGuid { get; set; }
    }
}