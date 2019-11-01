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
        [Display(Name = "شناسه")]
        public int? ID { get; set; }

        [Display(Name = "شناسه آزمون")]
        public int ExamID { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Type { get; set; }

        [Display(Name = "گروه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Group { get; set; }



        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Response { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Mark { get; set; }
    }
}
