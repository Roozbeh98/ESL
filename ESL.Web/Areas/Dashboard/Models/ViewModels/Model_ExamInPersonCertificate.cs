using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_ExamInPersonCertificate
    {
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Display(Name = "نمره Writing")]
        public int WritingMark{ get; set; }

        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Display(Name = "نمره Speaking")]
        public int SpeakingMark{ get; set; }

        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Display(Name = "نمره Reading")]
        public int ReadingMark{ get; set; }

        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Display(Name = "نمره Listening")]
        public int ListeningMark{ get; set; }

        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Display(Name = "نمره Final")]
        public int FinalMark{ get; set; }
    }
}
