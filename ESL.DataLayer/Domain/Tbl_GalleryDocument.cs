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
    
    public partial class Tbl_GalleryDocument
    {
        public int GD_ID { get; set; }
        public System.Guid GD_Guid { get; set; }
        public int GD_GalleryID { get; set; }
        public int GD_DocumentID { get; set; }
        public int GD_Title { get; set; }
        public System.DateTime GD_CreationDate { get; set; }
    
        public virtual Tbl_Document Tbl_Document { get; set; }
        public virtual Tbl_Gallery Tbl_Gallery { get; set; }
    }
}
