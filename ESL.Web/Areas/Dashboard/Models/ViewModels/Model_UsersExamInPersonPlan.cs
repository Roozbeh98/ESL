using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UsersExamInPersonPlan
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string User { get; set; }

        [Display(Name = "شماره صندلی")]
        public int? SeatNumber { get; set; }

        [Display(Name = "نمره")]
        public int? Mark { get; set; }

        [Display(Name = "وضعیت حضور")]
        public bool Presence { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "وضعیت ثبت نام")]
        public bool Activeness { get; set; }
    }
}