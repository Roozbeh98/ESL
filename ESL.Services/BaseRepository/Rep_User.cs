using ESL.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESL.DataLayer.Models;

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
                infoModel.Name = q.User_FirstName + " " + q.User_lastName;
                infoModel.Role = q.Tbl_Role.Role_Display;
                return infoModel;
            }
            else
            {
                return null;

            }

        }
    }
}
