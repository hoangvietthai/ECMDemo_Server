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
    public class DocumentConfirmModel
    {
        public int ConfirmId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public System.DateTime FinishedOnDate { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime UpdatedOnDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public int RelatedDocumentId { get; set; }
        public int ModuleId { get; set; }
        public int IsFinished { get; set; }
        public int PriorityLevel { get; set; }
    }
    public class DocumentConfirmCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public System.DateTime FinishedOnDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int RelatedDocumentId { get; set; }
        public int ModuleId { get; set; }
        public int PriorityLevel { get; set; }
        public int ProcessId { get; set; }
    }
    public class DocumentConfirmUpdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public System.DateTime FinishedOnDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public int RelatedDocumentId { get; set; }
        public int ModuleId { get; set; }
        public int PriorityLevel { get; set; }
    }
    public class DocumentConfirmResponseModel
    {
        public int UserId { get; set; }
        public int ResponseStatus { get; set; }
        public int DocumentConfirmId { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedOnDate { get; set; }

    }
    public class DocumentConfirmResponseDisplayModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int DocumentConfirmId { get; set; }
        public int ResponseStatus { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOnDate { get; set; }

    }
    public class RecreateDocumentConfirmModel
    {
        public int ModuleId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int ExtraDays { get; set; }
    }
    public class FinishConfirmModel
    {
        public int UserId { get; set; }
        public int Status { get; set; }
        public int ProcessId { get; set; }
    }
}