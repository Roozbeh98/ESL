using System;
using System.ComponentModel.DataAnnotations;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Question
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Type { get; set; }

        [Display(Name = "گروه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Group { get; set; }

        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Response { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Mark { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}