using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_PaymentCreate
    {
        [Display(Name = "شناسه کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid User { get; set; }

        [Display(Name = "نوع پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Type { get; set; }

        [Display(Name = "روش پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "وضعیت پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid State { get; set; }

        [Display(Name = "بها")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string TrackingToken { get; set; }

    }
}