using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class ReceivedDocumentDisplayModel
    {
        public int ReceivedDocumentId { get; set; }
        public string Name { get; set; }
        
        public string Sender { get; set; }
        public string ReceiverUserFullName { get; set; }
   
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int DocumentIndex { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int DocumentStatusId { get; set; }
        public Nullable<int> DocumentProcessId { get; set; }
    }
    public class BaseReceivedDocumentModel
    {
        public int ReceivedDocumentId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
    }
    public class ReceivedDocumentModel : BaseReceivedDocumentModel 
    {
        public int CategoryId { get; set; }
        public int SenderId { get; set; }
        public int SignedByUserId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int DocumentIndex { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverUserId { get; set; }
        public int DepartmentId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int SecretLevel { get; set; }
        public int DocumentStatusId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public int LastModifiedByUserId { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public string AttachedFileUrl { get; set; }
        public Nullable<int> DocumentProcessId { get; set; }
    }
    public class ReceivedDocumentCreateModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public int CategoryId { get; set; }
        public int SenderId { get; set; }
        public int SignedByUserId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int DocumentIndex { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverUserId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int DepartmentId { get; set; }
        public int SecretLevel { get; set; }
        public int DocumentStatusId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public string AttachedFileUrl { get; set; }
    }
    public class ReceivedDocumentUpdateModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public int CategoryId { get; set; }
        public int SenderId { get; set; }
        public int SignedByUserId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public int DocumentIndex { get; set; }
        public int ReceiverId { get; set; }
        public int ReceiverUserId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int DepartmentId { get; set; }
        public int SecretLevel { get; set; }
        public int DocumentStatusId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public string AttachedFileUrl { get; set; }
    }
}