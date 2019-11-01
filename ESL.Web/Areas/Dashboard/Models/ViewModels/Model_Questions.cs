using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESL.Web.Areas.Dashboard.Models.ViewModels
{
    public class Model_Questions
    {
        [Display(Name = "شناسه آزمون")]
        public int ExamID { get; set; }

        public IEnumerable<Model_Question> Questions { get; set; }
    }
}