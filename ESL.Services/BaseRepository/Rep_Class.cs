using ESL.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ESL.Services.BaseRepository
{
    static public class Rep_Class
    {
        private static readonly ESLEntities db = new ESLEntities();

        public static IEnumerable<SelectListItem> Get_AllClasses()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_Class.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.Class_Guid.ToString(), Text = item.Class_Title });
            }

            return list.AsEnumerable();
        }
    }
}
