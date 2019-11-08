using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_PaymentEdit
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "شناسه کاربر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid User { get; set; }

        [Display(Name = "عنوان تراکنش")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Title { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Type { get; set; }

        [Display(Name = "روش تراکنش")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Description { get; set; }

        [Display(Name = "بها")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Cost { get; set; }

        [Display(Name = "کد رهگیری")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string TrackingToken { get; set; }
    }
}