using ESL.DataLayer.Domain;
using ESL.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Services.BaseRepository
{
    public class Rep_Wallet
    {
        private ESLEntities db = new ESLEntities();

        public bool Set_Credit(Guid wallet, Guid type, int cost, Guid title)
        {
            Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_Guid == wallet).SingleOrDefault();

            //switch (Rep_CodeGroup.Get_CodeNameWithGUID(state))
            //{
            //    case "Paid":
            //        _Wallet.Wallet_Credit = Rep_CodeGroup.Get_CodeNameWithGUID(type) == "Crediting -> Enum" ? _Wallet.Wallet_Credit + cost : _Wallet.Wallet_Credit - cost;
            //        break;

            //    case "Suspended":
            //    case "ReturnToAccount":
            //        _Wallet.Wallet_Credit = Rep_CodeGroup.Get_CodeNameWithGUID(type) == "Crediting -> Enum" ? _Wallet.Wallet_Credit + cost : _Wallet.Wallet_Credit - cost;
            //        break;

            //    default:
            //        break;
            //}

            db.Entry(_Wallet).State = EntityState.Modified;

            return Convert.ToBoolean(db.SaveChanges() > 0);
        }

        private int Get_WalletCreditWithGUID(Guid guid)
        {
            return db.Tbl_Wallet.Where(x => x.Wallet_Guid == guid).SingleOrDefault().Wallet_Credit;
        }

        public Guid Get_WalletGUIDWithUserGUID(Guid guid)
        {
            return db.Tbl_Wallet.Where(x => x.Tbl_User.User_Guid == guid).SingleOrDefault().Wallet_Guid;
        }

        public WalletAction Get_WalletAction(string current, string next)
        {
            // enum
            if (current == next)
            {
                return WalletAction.NoAction;
            }
            else if (current == "Paid" && next == "Suspended")
            {
                return WalletAction.Decrease;
            }
            else if (current == "Paid" && next == "ReturnToAccount")
            {
                return WalletAction.Decrease;
            }
            else if (current == "Suspended" && next == "Paid")
            {
                return WalletAction.Increase;
            }
            else if (current == "Suspended" && next == "ReturnToAccount")
            {
                return WalletAction.Decrease;
            }
            else if (current == "ReturnToAccount" && next == "Paid")
            {
                return WalletAction.Decrease;
            }
            else if (current == "ReturnToAccount" && next == "Suspended")
            {
                return WalletAction.Decrease;
            }


            return WalletAction.NoAction;
        }

    }
}
