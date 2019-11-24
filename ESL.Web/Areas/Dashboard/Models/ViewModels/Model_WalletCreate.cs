using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_WalletCreate
    {
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public Guid User { get; set; }

        [Display(Name = "اعتبار")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public int Credit { get; set; }
    }
}