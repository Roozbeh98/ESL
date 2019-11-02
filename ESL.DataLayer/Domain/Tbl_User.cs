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
    
    public partial class Tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_User()
        {
            this.Tbl_News = new HashSet<Tbl_News>();
            this.Tbl_UserExam = new HashSet<Tbl_UserExam>();
            this.Tbl_UserExamAccess = new HashSet<Tbl_UserExamAccess>();
            this.Tbl_UserExamInPerson = new HashSet<Tbl_UserExamInPerson>();
        }
    
        public int User_ID { get; set; }
        public System.Guid User_Guid { get; set; }
        public int User_RoleID { get; set; }
        public int User_GenderCodeID { get; set; }
        public Nullable<int> User_LevelCodeID { get; set; }
        public string User_Email { get; set; }
        public Nullable<bool> User_EmailConfirmed { get; set; }
        public string User_PasswordSalt { get; set; }
        public string User_PasswordHash { get; set; }
        public string User_FirstName { get; set; }
        public string User_lastName { get; set; }
        public string User_IdentityNumber { get; set; }
        public string User_Mobile { get; set; }
        public bool User_IsBan { get; set; }
        public bool User_IsDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_News> Tbl_News { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_UserExam> Tbl_UserExam { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_UserExamAccess> Tbl_UserExamAccess { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_UserExamInPerson> Tbl_UserExamInPerson { get; set; }
    }
}
