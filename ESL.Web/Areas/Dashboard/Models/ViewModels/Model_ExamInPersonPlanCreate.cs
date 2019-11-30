using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ExamInPersonPlanCreate
    {
        [Display(Name = "آزمون")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Exam { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string SubExam { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت (تومان)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان برگزاری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Location { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Capacity { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int TotalMark { get; set; }

        [Display(Name = "حداقل نمره قبولی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int PassMark { get; set; }

        [Display(Name = "تاریخ و زمان برگزاری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Date { get; set; }

        [Display(Name = "وضعیت نمایش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public bool Activeness { get; set; }
    }
}