using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ExamInPerson
    {
        [Display(Name = "شناسه")]
        public int? ID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "نمره")]
        public int Mark { get; set; }

        [Display(Name = "حداقل نمره قبولی")]
        public int PassMark { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}
