using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Exam
    {
        [Display(Name = "شناسه")]
        public int? ID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Mark { get; set; }

        [Display(Name = "حداقل نمره قبولی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int PassMark { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}
