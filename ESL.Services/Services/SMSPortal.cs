using ESL.DataLayer.Domain;
using ESL.Web;
using Kavenegar;
using Kavenegar.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Services.Services
{
    public class SMSPortal
    {
        private ESLEntities db = new ESLEntities();
        private string apikey;

        public SMSPortal()
        {
            apikey = db.Tbl_SMSProviderConfiguration.ToList().First().SPC_ApiKey;
        }

        private string GetTemplate(SMSTemplate template)
        {
            return db.Tbl_SMSTemplate.Where(x => x.ST_ID == (int)template).SingleOrDefault().ST_Name;
        }

        public int VerifyLookup(string receptor, string token, string token2, string token3, SMSTemplate template)
        {
            try
            {
                var api = new KavenegarApi(apikey);



                var result = api.VerifyLookup(receptor, token, token2, token3, GetTemplate(template));

                return result.Status;
            }
            catch (ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                Console.Write("Message : " + ex.Message);
            }
            catch (HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                Console.Write("Message : " + ex.Message);
            }

            return -1;
        }


    }
}
