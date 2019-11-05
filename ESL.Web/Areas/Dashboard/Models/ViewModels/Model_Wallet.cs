using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Wallet
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string User { get; set; }

        [Display(Name = "اعتبار")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Credit { get; set; }

        [Display(Name = "تاریخ اولین تراکنش")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public DateTime ModifiedDate { get; set; }
    }
}