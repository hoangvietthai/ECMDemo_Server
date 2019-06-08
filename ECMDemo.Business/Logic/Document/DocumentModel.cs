using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class DocumentDisplayModel
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string FileCates { get; set; }
        public string Description { get; set; }
        public string CreatedByUserName { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public bool DocumentType { get; set; }
    }
    public class DocumentModel
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string FileCates { get; set; }
        public bool DocumentType { get; set; }
        public int DirectoryId { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public bool IsDelete { get; set; }
    }
    public class DocumentCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileCates { get; set; }
        public string FileUrl { get; set; }
        public int FileTypeId { get; set; }
        public int CreatedByUserId { get; set; }
        public int DirectoryId { get; set; }
        public bool DocumentType { get; set; }
    }
    public class DocumentUpdateModel
    {
        public string Description { get; set; }
        public string FileCates { get; set; }
        public string Name { get; set; }
        public int FileTypeId { get; set; }
        public int LastModifiedByUserId { get; set; }
        public int DirectoryId { get; set; }
        public bool DocumentType { get; set; }
    }
    public class FilterDocument
    {
        public int? DepartmentId { get; set; }
        public int? DirectoryId { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class ShareDocumentModel
    {
        public int DocumentId { get; set; }
        public int DepartmentId { get; set; }
        public string Message { get; set; }
    }
}