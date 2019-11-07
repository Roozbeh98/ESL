using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClass
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string User { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "وضعیت ثبت نام")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public bool IsDelete { get; set; }
    }
}