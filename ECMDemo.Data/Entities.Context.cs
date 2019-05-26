﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BlackTokenList> BlackTokenLists { get; set; }

        public virtual DbSet<BusinessPartner> BusinessPartners { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        public virtual DbSet<DocumentCategoryGroup> DocumentCategoryGroups { get; set; }
        public virtual DbSet<DocumentConfirm> DocumentConfirms { get; set; }
        public virtual DbSet<DocumentProcess> DocumentProcesses { get; set; }
        public virtual DbSet<DocumentStatus> DocumentStatus { get; set; }
        public virtual DbSet<DocumentUnify> DocumentUnifies { get; set; }
        public virtual DbSet<EnterpriseUnit> EnterpriseUnits { get; set; }
        public virtual DbSet<InternalDocument> InternalDocuments { get; set; }
        public virtual DbSet<QH_BusinessPartner_ContactPerson> QH_BusinessPartner_ContactPerson { get; set; }
        public virtual DbSet<QH_DocumentConfirm_User> QH_DocumentConfirm_User { get; set; }
        public virtual DbSet<QH_DocumentPerform_User> QH_DocumentPerform_User { get; set; }
        public virtual DbSet<QH_DocumentUnify_User> QH_DocumentUnify_User { get; set; }
        public virtual DbSet<QH_ReceivedDocument_File> QH_ReceivedDocument_File { get; set; }
        public virtual DbSet<QH_SendDocument_File> QH_SendDocument_File { get; set; }
        public virtual DbSet<ReceivedDocument> ReceivedDocuments { get; set; }
        public virtual DbSet<SendDocument> SendDocuments { get; set; }
        public virtual DbSet<TaskMessage> TaskMessages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<DocumentPerform> DocumentPerforms { get; set; }
    }
}
