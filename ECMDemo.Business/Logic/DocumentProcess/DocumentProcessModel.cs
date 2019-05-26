using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{

    public class DocumentProcessModel
    {
        public int Id { get; set; }
        public int RelatedId { get; set; }
        public int TaskType { get; set; }
        public int OrderIndex { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
    }
    public class DocumentProcessCreateModel
    {
        public int RelatedId { get; set; }
        public int TaskType { get; set; }
        public int OrderIndex { get; set; }
    }
    public enum DocumentProcessStatus
    {
        NOTSET = -1,
        WAITING = 0,
        INPROCESS = 1,
        FINISHED = 2
    }
}