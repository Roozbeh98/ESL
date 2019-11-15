using ESL.Common.Plugins;
using ESL.DataLayer.Domain;
using ESL.Web;
using Kavenegar;
using Kavenegar.Exceptions;
using Kavenegar.Models;
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

        private bool InsertIntoDB(SendResult result, string token, string token1, string token2)
        {
            if (result != null)
            {
                Tbl_SMSResponse q = new Tbl_SMSResponse()
                {
                    SMS_Status = result.Status,
                    SMS_StatusText = result.StatusText,
                    SMS_MessageID = result.Messageid.ToString(),
                    SMS_Message = result.Message,
                    SMS_Token = token,
                    SMS_Token1 = token1,
                    SMS_Token2 = token2,
                    SMS_Sender = result.Sender,
                    SMS_Receptor = result.Receptor,
                    SMS_Date = DateConverter.UnixTimeStampToDateTime(result.Date),
                    SMS_Cost = result.Cost,
                    SMS_CreationDate = DateTime.Now,
                    SMS_ModifiedDate = DateTime.Now
                };

                db.Tbl_SMSResponse.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    return true;
                }

            }

            return false;
        }

        private bool InsertIntoDB(SendResult result)
        {
            if (result != null)
            {
                Tbl_SMSResponse q = new Tbl_SMSResponse()
                {
                    SMS_Guid = Guid.NewGuid(),
                    SMS_Status = result.Status,
                    SMS_StatusText = result.StatusText,
                    SMS_MessageID = result.Messageid.ToString(),
                    SMS_Message = result.Message,
                    SMS_Sender = result.Sender,
                    SMS_Receptor = result.Receptor,
                    SMS_Date = DateConverter.UnixTimeStampToDateTime(result.Date),
                    SMS_Cost = result.Cost,
                    SMS_CreationDate = DateTime.Now,
                    SMS_ModifiedDate = DateTime.Now
                };

                db.Tbl_SMSResponse.Add(q);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    return true;
                }

            }

            return false;
        }

        public string SendServiceable(string receptor, string token, string token2, string token3, SMSTemplate template)
        {
            try
            {
                var api = new KavenegarApi(apikey);
                SendResult result = api.VerifyLookup(receptor, token, token2, token3, GetTemplate(template));

                InsertIntoDB(result, token, token2, token3);

                return result.StatusText;
            }
            catch (ApiException ex)
            {
                return ex.Message;
            }
            catch (HttpException ex)
            {
                return ex.Message;
            }
        }

        public int SendAdvertising(string sender, string receptor, string message)
        {
            try
            {
                var api = new KavenegarApi(apikey);
                var result = api.Send(sender, receptor, message);

                InsertIntoDB(result);

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
