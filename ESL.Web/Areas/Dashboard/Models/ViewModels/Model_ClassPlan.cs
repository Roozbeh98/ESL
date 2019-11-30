using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ClassPlan
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کلاس")]
        public string Class { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت هر جلسه (تومان)")]
        public int CostPerSession { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان برگزاری")]
        public string Location { get; set; }

        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }

        [Display(Name = "ساعت برگزاری")]
        public TimeSpan Time { get; set; }

        [Display(Name = "طول هر جلسه (دقیقه)")]
        public double SessionsLength { get; set; }

        [Display(Name = "تاریخ و زمان شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "وضعیت نمایش")]
        public bool Activeness { get; set; }
    }
}