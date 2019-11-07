using ESL.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ESL.Services.BaseRepository
{
    static public class Rep_Workshop
    {
        private static readonly ESLEntities db = new ESLEntities();

        public static IEnumerable<SelectListItem> Get_AllWorkshops()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_Workshop.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.Workshop_Guid.ToString(), Text = item.Workshop_Title });
            }

            return list.AsEnumerable();
        }
    }
}
