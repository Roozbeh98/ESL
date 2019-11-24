using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_PaymentChangeState
    {
        [Display(Name = "شناسه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int ID { get; set; }

        [Display(Name = "وضعیت تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid State { get; set; }

        [Display(Name = "روش تراکنش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "تخفیف")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Discount { get; set; }
    }
}