using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Register
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Family { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [MaxLength(10, ErrorMessage = "کدملی نامعتبر")]
        [MinLength(10, ErrorMessage = "کدملی نامعتبر")]
        public string IdentityNumber { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Range(1,31, ErrorMessage = "نامعتبر")]
        [MaxLength(2, ErrorMessage = "نامعتبر")]
        public string Day { get; set; }

        [Display(Name = " ")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Range(1, 12, ErrorMessage = "نامعتبر")]
        [MaxLength(2, ErrorMessage = "نامعتبر")]
        public string month { get; set; }

        [Display(Name = " ")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [Range(1300, 1400, ErrorMessage = "نامعتبر")]
        [MinLength(4, ErrorMessage = "نامعتبر")]
        [MaxLength(4, ErrorMessage = "نامعتبر")]
        public string Year { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Gender { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(11, ErrorMessage = "مقدار وارد شده بیش 11 کارکتراست")]
        public string Mobile { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(200, ErrorMessage = "مقدار وارد شده بیش 200 کارکتراست")]
        [EmailAddress(ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        //[Remote("EmailValid", "Account", HttpMethod = "Post", ErrorMessage = "این ایمیل قبلا ثبت شده است")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //  [Remote("PasswordMatch", "Account", HttpMethod = "Post",ErrorMessage ="پسورد ها برابر نیست")]
        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "پسورد ها برابر نیست")]
        public string PasswordVerify { get; set; }
    }
}