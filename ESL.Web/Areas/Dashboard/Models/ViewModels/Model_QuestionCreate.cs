using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_QuestionCreate
    {
        [Display(Name = "شناسه آزمون")]
        public int ExamID { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Type { get; set; }

        [Display(Name = "گروه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Group { get; set; }

        [Display(Name = "ضمیمه 1")]
        public string Filepond1 { get; set; }

        [Display(Name = "ضمیمه 2")]
        public string Filepond2 { get; set; }

        [Display(Name = "ضمیمه 3")]
        public string Filepond3 { get; set; }

        [Display(Name = "ضمیمه 4")]
        public string Filepond4 { get; set; }

        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Response { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Mark { get; set; }
    }
}
