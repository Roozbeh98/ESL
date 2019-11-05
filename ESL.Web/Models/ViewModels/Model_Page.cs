using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Models.ViewModels
{
    public class Model_Page
    {
        [Display(Name = "کاور")]
        public string cover { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string titel { get; set; }
        [AllowHtml]
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string body { get; set; }
        [Display(Name = "پیوست")]
        public string [] attachment { get; set; }
        [Display(Name = "برچسب")]
        public string[] Tag { get; set; }

    }
}