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
        public string User { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "وضعیت")]
        public string State { get; set; }

        [Display(Name = "روش")]
        public string Way { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت (تومان)")]
        public int Cost { get; set; }

        [Display(Name = "تخفیف (تومان)")]
        public int Discount { get; set; }

        [Display(Name = "باقی مانده اعتبار")]
        public int RemaingWallet { get; set; }

        [Display(Name = "کد رهگیری")]
        public string TrackingToken { get; set; }

        [Display(Name = "ضمیمه")]
        public string Document { get; set; }

        [Display(Name = "تاریخ ایجاد" )]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime ModifiedDate { get; set; }
    }
}