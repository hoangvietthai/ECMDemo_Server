using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    /*
     Status 
     0 - In process
     1 - Accepted
     2 - Redo
     -1 - Rejected
         */
    public class DocumentPerformModel
    {
        public int PerformId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserList { get; set; }
        public DateTime FinishedOnDate { get; set; }
        public int TaskType { get; set; }
        public int PriorityLevel { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public System.DateTime UpdatedOnDate { get; set; }
        public int Status { get; set; }
        public int RelatedDocumentId { get; set; }
        public int ModuleId { get; set; }
        public int IsFinished { get; set; }
    }
    public class DocumentPerformCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserList { get; set; }
        public DateTime FinishedOnDate { get; set; }
        public int TaskType { get; set; }
        public int PriorityLevel { get; set; }
        public int RelatedDocumentId { get; set; }
        public int CreatedByUserId { get; set; }
        public int ModuleId { get; set; }
        public int ProcessId { get; set; }

    }
    public class DocumentPerformUpdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserList { get; set; }
        public DateTime FinishedOnDate { get; set; }
        public int TaskType { get; set; }
        public int PriorityLevel { get; set; }
        public int RelatedDocumentId { get; set; }
        public int UpdatedByUserId { get; set; }
    }
    public class DocumentPerformResponseModel
    {
        public int UserId { get; set; }
        public int DocumentPerformId { get; set; }
        public int ResponseStatus { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOnDate { get; set; }
    }
    public class DocumentPerformResponseDisplayModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int DocumentPerformId { get; set; }
        public int ResponseStatus { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOnDate { get; set; }

    }
    public class RecreateDocumentPerformModel
    {
        public int ModuleId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int ExtraDays { get; set; }
    }
    public class FinishPerformModel
    {
        public int UserId { get; set; }
        public int Status { get; set; }
        public int ProcessId { get; set; }
    }
}