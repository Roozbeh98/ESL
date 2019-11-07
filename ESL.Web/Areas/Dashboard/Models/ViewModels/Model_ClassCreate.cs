using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ClassCreate
    {
        [Display(Name = "عنوان")]
        public string Class { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "بها")]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان")]
        public string Location { get; set; }

        [Display(Name = "وضعیت نمایش")]
        public bool Activeness { get; set; }

        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }

        [Display(Name = "ساعت برگزاری")]
        public TimeSpan Time { get; set; }

        [Display(Name = "تعداد جلسات")]
        public int SessionsNum { get; set; }

        [Display(Name = "طول هر جلسه")]
        public int SessionsLength { get; set; }

        [Display(Name = "تاریخ امتحان")]
        public string ExamDate { get; set; }
    }
}