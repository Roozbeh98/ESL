using ESL.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ESL.Services.BaseRepository
{
    static public class Rep_Exam
    {
        private static readonly ESLEntities db = new ESLEntities();

        public static IEnumerable<SelectListItem> Get_AllExams()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_ExamInPerson.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.EIP_Guid.ToString(), Text = item.EIP_Title });
            }

            return list.AsEnumerable();
        }
    }
}
