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
    
    public partial class DocumentStatus
    {
        public int Id { get; set; }
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
