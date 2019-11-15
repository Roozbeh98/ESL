using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Purchase
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "نوع")]
        public int Type { get; set; }

        [Display(Name = "عنوان")]
        public string Name { get; set; }

        [Display(Name = "عنوان")]
        public string SubName { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }

        [Display(Name = "مکان")]
        public string Location { get; set; }

        [Display(Name = "زمان")]
        public TimeSpan Time { get; set; }

        [Display(Name = "نمره کل")]
        public int TotalMark { get; set; }

        [Display(Name = "حداقل نمره قبولی")]
        public int PassMark { get; set; }

        [Display(Name = "زمان برگزاری")]
        public DateTime Date { get; set; }

        [Display(Name = "قیمت")]
        public int Cost { get; set; }
    }
}