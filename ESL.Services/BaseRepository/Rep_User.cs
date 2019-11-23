using ESL.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESL.DataLayer.Models;
using System.Web.Mvc;

namespace ESL.Services.BaseRepository
{
    public class Rep_User
    {
        private readonly ESLEntities db = new ESLEntities();


        public Rep_User()
        {

        }

        public Model_AccountInfo GetInfoForNavbar(string Username)
        {

            var q = db.Tbl_User.Where(a => a.User_Email == Username || a.User_Mobile == Username).SingleOrDefault();

            if (q != null)
            {
                Model_AccountInfo infoModel = new Model_AccountInfo();
                infoModel.UserGuid = q.User_Guid;
                infoModel.Name = q.User_FirstName + " " + q.User_lastName;
                infoModel.Role = q.Tbl_Role.Role_Display;
                return infoModel;
            }
            else
            {
                return null;
            }
        }

        public int Get_UserIDWithGUID(Guid guid)
        {
            return db.Tbl_User.Where(x => x.User_Guid == guid).SingleOrDefault().User_ID;
        }

        public SelectListItem Get_UserSelectListItemWithGUID(Guid guid)
        {
            var q = db.Tbl_User.Where(x => x.User_Guid == guid).SingleOrDefault();
            return new SelectListItem() { Value = q.User_Guid.ToString(), Text = q.User_FirstName + " " + q.User_lastName };
        }
    }
}
