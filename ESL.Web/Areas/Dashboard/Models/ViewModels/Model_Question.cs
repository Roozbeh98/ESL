using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Question
    {
        [Display(Name = "شناسه")]
        public int? ID { get; set; }

        [Display(Name = "شناسه امتحان")]
        public int ExamID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Type { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Response { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Mark { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}
