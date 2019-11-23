using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ChargeWallet
    {
        [Display(Name = "بها")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string TrackingToken { get; set; }
    }
}