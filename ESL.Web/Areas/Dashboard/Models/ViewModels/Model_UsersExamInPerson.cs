using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_UsersExamInPerson
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string User { get; set; }

        [Display(Name = "شماره صندلی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int? SeatNumber { get; set; }

        [Display(Name = "نمره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Mark { get; set; }

        [Display(Name = "وضعیت حضور")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string IsPresent { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}