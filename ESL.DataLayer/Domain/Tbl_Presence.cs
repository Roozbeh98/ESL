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
    
    public partial class Tbl_Presence
    {
        public int Presence_ID { get; set; }
        public System.Guid Presence_Guid { get; set; }
        public int Presence_UserID { get; set; }
        public int Presence_CPID { get; set; }
        public int Presence_PaymentID { get; set; }
        public bool Presence_IsPresent { get; set; }
        public System.DateTime Presence_CreationDate { get; set; }
        public System.DateTime Presence_ModifiedDate { get; set; }
        public bool Presence_IsDelete { get; set; }
    
        public virtual Tbl_ClassPlan Tbl_ClassPlan { get; set; }
        public virtual Tbl_Payment Tbl_Payment { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
    }
}
