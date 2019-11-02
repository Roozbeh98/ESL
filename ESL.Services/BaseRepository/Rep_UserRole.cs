using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESL.DataLayer.Domain;

namespace ESL.Services.BaseRepository
{
    public static class Rep_UserRole
    {
        private static readonly ESLEntities db = new ESLEntities();
        public static string Get_RoleNameWithID(int id)
        {
            return db.Tbl_Role.Where(a => a.Role_ID == id).SingleOrDefault().Role_Name;
        }
    }
}
