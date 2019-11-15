using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserWorkshops
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string User { get; set; }

        [Display(Name = "کارگاه")]
        public string Workshop { get; set; }

        [Display(Name = "کارگاه")]
        public string SubWorkshop { get; set; }

        [Display(Name = "مکان")]
        public string Location { get; set; }

        [Display(Name = "زمان برگزاری")]
        public DateTime Date { get; set; }

        [Display(Name = "قیمت")]
        public int Cost { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "وضعیت ثبت نام")]
        public bool RegisterationState { get; set; }
    }
}