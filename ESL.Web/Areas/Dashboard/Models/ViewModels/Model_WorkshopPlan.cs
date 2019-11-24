﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_WorkshopPlan
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        public string Workshop { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "مکان")]
        public string Location { get; set; }

        [Display(Name = "وضعیت نمایش")]
        public bool Activeness { get; set; }

        [Display(Name = "ظرفیت")]
        public int Capacity { get; set; }

        [Display(Name = "تاریخ برگزاری")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
    }
}