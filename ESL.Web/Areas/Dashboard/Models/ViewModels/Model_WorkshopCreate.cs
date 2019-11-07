using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_WorkshopCreate
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Workshop { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string SubWorkshop { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "بها")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Location { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Capacity { get; set; }

        [Display(Name = "تاریخ برگزاری")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Date { get; set; }

        [Display(Name = "وضعیت نمایش")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public bool Activeness { get; set; }
    }
}