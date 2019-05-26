using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class BaseInternalDocumentModel
    {
        public int InternalDocumentId { get; set; }
        public string Name { get; set; }
    }
    public class InternalDocumentModel: BaseInternalDocumentModel
    {
        public string Summary { get; set; }
        public int DirectoryId { get; set; }
        public int CategoryId { get; set; }
        public int SecretLevel { get; set; }
        public int ProjectId { get; set; }
        public int DepartmentId { get; set; }
        public int DocumentStatusId { get; set; }
        public Nullable<int> DocumentProcessId { get; set; }
        public int WrittenByUserId { get; set; }
        public int ResponsibleUserId { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public bool IsDelete { get; set; }
        public string AttachedFileUrl { get; set; }
    }
    public class InternalDocumentDisplayModel
    {
        public int InternalDocumentId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ResignedNumber { get; set; }
        public Nullable<System.DateTime> ResignedOnDate { get; set; }
        //Người ký
        public string WrittenByUserFullName { get; set; }
        //Busines Partner
        public System.DateTime CreatedOnDate { get; set; }
        public int DocumentStatusId { get; set; }
        public Nullable<int> DocumentProcessId { get; set; }
    }
    public class InternalDocumentCreateModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public int DirectoryId { get; set; }
        public int CategoryId { get; set; }
        public int SecretLevel { get; set; }
        public int ProjectId { get; set; }
        public int DepartmentId { get; set; }
        public int DocumentStatusId { get; set; }
        public int WrittenByUserId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public string AttachedFileUrl { get; set; }
    }
    public class InternalDocumentUpdateModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public int DirectoryId { get; set; }
        public int CategoryId { get; set; }
        public int SecretLevel { get; set; }
        public int ProjectId { get; set; }
        public int DepartmentId { get; set; }
        public int DocumentStatusId { get; set; }
        public int WrittenByUserId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int LastModifiedByUserId { get; set; }
        public string AttachedFileUrl { get; set; }
    }
}