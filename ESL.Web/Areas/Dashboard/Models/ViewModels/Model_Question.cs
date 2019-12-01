using System;
using System.ComponentModel.DataAnnotations;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Question
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "نوع")]
        public string Type { get; set; }

        [Display(Name = "گروه")]
        public string Group { get; set; }

        [Display(Name = "پاسخ")]
        public string Response { get; set; }

        [Display(Name = "نمره")]
        public int Mark { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}