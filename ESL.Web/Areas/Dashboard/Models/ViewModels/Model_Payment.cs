using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Payment
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string User { get; set; }

        [Display(Name = "نوع پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Type { get; set; }

        [Display(Name = "روش پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Way { get; set; }

        [Display(Name = "وضعیت پرداخت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string State { get; set; }

        [Display(Name = "بها")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string TrackingToken { get; set; }

        [Display(Name = "ضمیمه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Document { get; set; }

        [Display(Name = "تاریخ" )]
        public DateTime Date { get; set; }
    }
}