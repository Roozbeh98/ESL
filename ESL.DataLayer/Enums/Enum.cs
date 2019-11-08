using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Web
{
    public enum TosterType
    {
        JustTitel,
        WithTitel,
        Maseage
    }

    public enum CodeGroup
    {
        Gender = 1,
        QuestionGroup = 4,
        QuestionType = 5,
        QuestionOption = 6,
        PaymentType = 10,
        PaymentWay = 9,
        DocumentType = 11,
        UploadFolder = 12,
        PaymentTitle = 15,
        ClassType = 17
    }

    public enum CodeID
    {
        WaitForAcceptance = 24,
        Paid = 25,
        ReturnToAccount = 26,
        ReturnToBankAccount = 52,
        Suspended = 53,

        Charge = 44,
        Discharge = 51
    }

    public enum WalletAction
    {
        Increase,
        Decrease,
        NoAction
    }

    public enum SMSTemplate
    {
        VerifyAccount = 1
    }
}
