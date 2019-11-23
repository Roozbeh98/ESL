using ESL.DataLayer.Domain;
using ESL.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ESL.Services.BaseRepository
{
    public static class Rep_CodeGroup
    {
        private static readonly ESLEntities db = new ESLEntities();

        public static IEnumerable<SelectListItem> Get_AllCodesWithGroupGUID(Guid guid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_CodeGroup.Where(x => x.CG_Guid.Equals(guid)).SingleOrDefault().Tbl_Code.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.Code_CGID.ToString(), Text = item.Code_Display });
            }

            return list.AsEnumerable();
        }

        public static IEnumerable<SelectListItem> Get_AllCodesWithGroupGUID(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_CodeGroup.Where(x => x.CG_ID.Equals(id)).SingleOrDefault().Tbl_Code.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.Code_ID.ToString(), Text = item.Code_Display });
            }

            return list.AsEnumerable();
        }

        public static IEnumerable<SelectListItem> Get_AllCodesWithGroupGUID(CodeGroup cg)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var q = db.Tbl_CodeGroup.Where(x => x.CG_ID == (int)cg).SingleOrDefault().Tbl_Code.ToList();

            foreach (var item in q)
            {
                list.Add(new SelectListItem() { Value = item.Code_Guid.ToString(), Text = item.Code_Display });
            }

            return list.AsEnumerable();
        }

        public static int Get_CodeIDWithGUID(Guid guid)
        {
            return db.Tbl_Code.Where(x => x.Code_Guid.Equals(guid)).SingleOrDefault().Code_ID;
        }

        public static string Get_CodeNameWithGUID(Guid guid)
        {
            return db.Tbl_Code.Where(x => x.Code_Guid.Equals(guid)).SingleOrDefault().Code_Name;
        }

        public static Guid Get_CodeGUIDWithName(string name)
        {
            return db.Tbl_Code.Where(x => x.Code_Name.Equals(name)).SingleOrDefault().Code_Guid;
        }

        public static int Get_CodeIDWithName(string name)
        {
            return db.Tbl_Code.Where(x => x.Code_Name.Equals(name)).SingleOrDefault().Code_ID;
        }
    }
}
