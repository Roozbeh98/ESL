using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ClassPlanCreate
    {
        [Display(Name = "کلاس")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Class { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Type { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت هر جلسه (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int CostPerSession { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان برگزاری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Location { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Capacity { get; set; }

        [Display(Name = "ساعت برگزاری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public TimeSpan Time { get; set; }

        [Display(Name = "طول هر جلسه (دقیقه)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public float SessionsLength { get; set; }

        [Display(Name = "تاریخ و زمان شروع")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string StartDate { get; set; }

        [Display(Name = "وضعیت نمایش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public bool Activeness { get; set; }
    }
}