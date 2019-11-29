using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserClassPlan
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string User { get; set; }

        [Display(Name = "تاریخ و زمان ثبت نام")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تعداد جلسات حضور")]
        public int PresenceSessions { get; set; }

        [Display(Name = "وضعیت ثبت نام")]
        public bool Activeness { get; set; }
    }
}