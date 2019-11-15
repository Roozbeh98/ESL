using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESL.Web
{
    public enum Role
    {
        Student = 1,
        Admin = 2
    }

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

    public enum PaymentType
    {
        WaitForAcceptance = 24,
        Paid = 25,
        ReturnToAccount = 26,
        ReturnToBankAccount = 52,
        Suspended = 53
    }

    public enum PaymentWay
    {
        Internet = 20,
        InPerson = 21,
        DepositToAccount = 22,
        DepositToCard = 23
    }

    public enum PaymentTitle
    {
        Charge = 44,
        Workshop = 45,
        Presence = 46,
        Absence = 47,
        Exam = 48,
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
        Verify = 1,
        Register = 2,
        Birthday = 3,
        Card = 4,
        Charge = 5,
        Class = 6,
        Exam = 7,
        Success = 8,
        Welcome = 9,
        Workshop = 10
    }

    public enum ProductType
    {
        ExamInPerson,
        ExamRemotely,
        Workshop,
        Class
    }
}
