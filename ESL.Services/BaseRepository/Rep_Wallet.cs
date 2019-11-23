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

        public Tbl_Wallet Set_Credit(int user, int credit)
        {
            Tbl_Wallet _Wallet = db.Tbl_Wallet.Where(x => x.Wallet_UserID == user).SingleOrDefault();
            _Wallet.Wallet_Credit = credit;

            return _Wallet;
        }

        public Guid Get_WalletGUIDWithUserGUID(Guid guid)
        {
            return db.Tbl_Wallet.Where(x => x.Tbl_User.User_Guid == guid).SingleOrDefault().Wallet_Guid;
        }

        public int Get_WalletCreditWithUserGUID(Guid guid)
        {
            return db.Tbl_Wallet.Where(x => x.Tbl_User.User_Guid == guid).SingleOrDefault().Wallet_Credit;
        }

        public int Get_WalletCreditWithUserID(int id)
        {
            return db.Tbl_Wallet.Where(x => x.Tbl_User.User_ID == id).SingleOrDefault().Wallet_Credit;
        }

        public int? Get_CreditBeforeTransaction(Tbl_Payment _Payment)
        {
            int _Credit = Get_WalletCreditWithUserID(_Payment.Payment_UserID);

            switch ((PaymentTitle)_Payment.Payment_TitleCodeID)
            {
                case PaymentTitle.Discharge:
                    return _Credit + _Payment.Payment_Cost;

                case PaymentTitle.ReturnToAccount:
                    return _Credit + _Payment.Payment_Cost;

                case PaymentTitle.ReturnToBankAccount:
                    return _Credit;

                case PaymentTitle.Charge:
                    return _Credit - _Payment.Payment_Cost;

                default:
                    return null;
            }
        }
    }
}
