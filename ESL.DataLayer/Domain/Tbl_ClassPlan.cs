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
    
    public partial class Tbl_ClassPlan
    {
        public int CP_ID { get; set; }
        public System.Guid CP_Guid { get; set; }
        public int CP_ClassID { get; set; }
        public int CP_Capacity { get; set; }
        public string CP_Description { get; set; }
        public int CP_Cost { get; set; }
        public string CP_Location { get; set; }
        public System.DateTime CP_DateTime { get; set; }
        public byte CP_SessionsNum { get; set; }
        public byte CP_SessionsLength { get; set; }
        public System.DateTime CP_ExamDateTime { get; set; }
        public System.DateTime CP_CreationDate { get; set; }
        public System.DateTime CP_ModifiedDate { get; set; }
        public bool CP_IsDelete { get; set; }
    
        public virtual Tbl_Class Tbl_Class { get; set; }
    }
}
