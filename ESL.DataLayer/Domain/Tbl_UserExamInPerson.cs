//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESL.DataLayer.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_UserExamInPerson
    {
        public int UEIP_ID { get; set; }
        public System.Guid UEIP_Guid { get; set; }
        public int UEIP_UserID { get; set; }
        public int UEIP_EIPID { get; set; }
        public Nullable<int> UEIP_SeatNumber { get; set; }
        public int UEIP_Mark { get; set; }
        public Nullable<int> UEIP_PaymentID { get; set; }
        public bool UEIP_IsPresent { get; set; }
        public bool UEIP_IsDelete { get; set; }
    
        public virtual Tbl_ExamInPerson Tbl_ExamInPerson { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
    }
}
