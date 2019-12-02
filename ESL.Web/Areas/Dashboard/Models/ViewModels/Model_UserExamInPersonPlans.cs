using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UserExamInPersonPlans
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string User { get; set; }

        [Display(Name = "آزمون")]
        public string Exam { get; set; }

        [Display(Name = "عنوان")]
        public string SubExam { get; set; }

        [Display(Name = "شماره صندلی")]
        public int SeatNumber { get; set; }

        [Display(Name = "نمره")]
        public int Mark { get; set; }

        [Display(Name = "وضعیت حضور")]
        public bool Presence { get; set; }

        [Display(Name = "مکان برگزاری")]
        public string Location { get; set; }

        [Display(Name = "تاریخ و زمان برگزاری")]
        public DateTime Date { get; set; }

        [Display(Name = "قیمت (تومان)")]
        public int Cost { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreationDate { get; set; }
    }
}