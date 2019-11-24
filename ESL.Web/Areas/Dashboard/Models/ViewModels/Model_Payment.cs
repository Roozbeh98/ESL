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
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string User { get; set; }

        [Display(Name = "عنوان تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "وضعیت تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string State { get; set; }

        [Display(Name = "روش تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Way { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "تخفیف")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Discount { get; set; }

        [Display(Name = "باقی مانده اعتبار")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int RemaingWallet { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string TrackingToken { get; set; }

        [Display(Name = "ضمیمه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Document { get; set; }

        [Display(Name = "تاریخ ایجاد" )]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime ModifiedDate { get; set; }
    }
}