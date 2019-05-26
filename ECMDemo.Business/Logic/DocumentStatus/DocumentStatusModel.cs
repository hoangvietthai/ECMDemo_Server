using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{

    public class BaseDocumentStatusModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class DocumentStatusModel: BaseDocumentStatusModel
    {
        public int UnifyStatus { get; set; }
        public Nullable<int> UnifyRelatedId { get; set; }
        public int ConfirmStatus { get; set; }
        public Nullable<int> ConfirmRelatedId { get; set; }
        public int PerformStatus { get; set; }
        public Nullable<int> PerformRelatedId { get; set; }
        public int RegisterStatus { get; set; }
        public Nullable<int> RegisterRelatedId { get; set; }

    }
    public class DocumentStatusCreateModel
    {
        public int UnifyStatus { get; set; }
        public int ConfirmStatus { get; set; }
        public int PerformStatus { get; set; }
        public int RegisterStatus { get; set; }
        public string DisplayName { get; set; }
    }
    public class DocumentStatusUpdateModel
    {
        public int UnifyStatus { get; set; }
        public Nullable<int> UnifyRelatedId { get; set; }
        public int ConfirmStatus { get; set; }
        public Nullable<int> ConfirmRelatedId { get; set; }
        public int PerformStatus { get; set; }
        public Nullable<int> PerformRelatedId { get; set; }
        public int RegisterStatus { get; set; }
        public Nullable<int> RegisterRelatedId { get; set; }
        public string DisplayName { get; set; }
    }
}