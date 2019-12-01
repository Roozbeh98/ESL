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
        public int ID { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid State { get; set; }

        [Display(Name = "روش")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid Way { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}