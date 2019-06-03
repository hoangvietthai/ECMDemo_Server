using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class TaskMessageDisplayModel
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public System.DateTime Deadline { get; set; }
        public int Status { get; set; }
        public int TaskType { get; set; }
        public int CreatedByUserId { get; set; }
        public int RelatedId { get; set; }
        public int ModuleId { get; set; }
        public bool IsMyTask { get; set; }
    }
    public class TaskMessageModel
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public System.DateTime Deadline { get; set; }
        public int Status { get; set; }
        public int TaskType { get; set; }
        public int UserId { get; set; }
        public int RelatedId { get; set; }
        public int ModuleId { get; set; }
        public bool IsMyTask { get; set; }
    }
    public class TaskMessageCreateModel
    {
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime Deadline { get; set; }
        public int TaskType { get; set; }
        public int UserId { get; set; }
        public int RelatedId { get; set; }
        public int ModuleId { get; set; }
        public bool IsMyTask { get; set; }
    }
    public enum TaskType
    {
        UNIFY=1,
        CONFIRM=2,
        PERFORM = 3,
        REGISTER =4,
       
    }
    public enum Module
    {
        DOCUMENT = 1,
        SEND = 2,
        RECEIVE = 3,
        INTERNAL = 4 
    }
    public class PendingTaskModel
    {
        public int ModuleId { get; set; }
        public int RelatedId { get; set; }
        public int TaskType { get; set; }
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
        public string CreatedByUser { get; set; }
    }
    public class PendingTaskDetailModel
    {
        public int ModuleId { get; set; }
        public int TaskType { get; set; }
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
        public string CreatedByUser { get; set; }
    }
    //Tất cả văn bản quá hạn
    public class ExpiredTaskModule
    {
        public int ModuleId { get; set; }
        public int RelatedDocumentId { get; set; }
        public int TaskType { get; set; }
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
        public string CreatedByUser { get; set; }

    }
}