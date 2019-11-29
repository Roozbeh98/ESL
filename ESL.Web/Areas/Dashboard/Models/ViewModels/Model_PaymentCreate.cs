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
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid User { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Title { get; set; }

        [Display(Name = "روش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string TrackingToken { get; set; }
    }
}