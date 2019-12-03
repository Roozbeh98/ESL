using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ExamsInPersonPlan
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "آزمون")]
        public string Exam { get; set; }

        [Display(Name = "عنوان")]
        public string SubExam { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت (تومان)")]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان برگزاری")]
        public string Location { get; set; }

        [Display(Name = "وضعیت نمایش")]
        public bool Activeness { get; set; }

        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }

        [Display(Name = "نمره")]
        public int TotalMark { get; set; }

        [Display(Name = "حداقل نمره قبولی")]
        public int PassMark { get; set; }

        [Display(Name = "تاریخ و زمان برگزاری")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime ModifiedDate { get; set; }
    }
}