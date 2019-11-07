using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_SetActiveness
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = " ")]
        public bool Activeness { get; set; }
    }
}