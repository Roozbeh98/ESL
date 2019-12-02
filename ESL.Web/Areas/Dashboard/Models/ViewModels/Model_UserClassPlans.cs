using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPlans
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string User { get; set; }

        [Display(Name = "کلاس")]
        public string Class { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Display(Name = "مکان برگزاری")]
        public string Location { get; set; }

        [Display(Name = "زمان شروع")]
        public TimeSpan Time { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreationDate { get; set; }
    }
}