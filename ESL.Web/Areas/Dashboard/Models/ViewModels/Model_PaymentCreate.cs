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
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid User { get; set; }

        [Display(Name = "عنوان تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Title { get; set; }

        [Display(Name = "روش تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "تخفیف")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Discount { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string TrackingToken { get; set; }
    }
}