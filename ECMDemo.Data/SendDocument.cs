//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECMDemo.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SendDocument
    {
        public int SendDocumentId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public int CategoryId { get; set; }
        public int SenderId { get; set; }
        public int ResponseForRDocId { get; set; }
        public int WrittenByUserId { get; set; }
        public int DepartmentId { get; set; }
        public int SignedByUserId { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverContactPersonId { get; set; }
        public int DeliveryMethodId { get; set; }
        public System.DateTime ResponseDeadline { get; set; }
        public int SecretLevel { get; set; }
        public int DocumentStatusId { get; set; }
        public Nullable<int> DocumentProcessId { get; set; }
        public int ResponsibleUserId { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public string AttachedFileUrl { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
