using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class DirectoryNodeModel
    {
        public int data { get; set; }
        public string label { get; set; }
        public int ParentId { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public IList<DirectoryNodeModel> children { get; set; } = new List<DirectoryNodeModel>();
    }
    public class DirectoryModel
    {
        public int DirectoryId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int DepartmentId { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public System.DateTime LastModifiedOnDate { get; set; }
        public int LastModifiedByUserId { get; set; }
        public int ModuleId { get; set; }
    }
    public class DirectoryCreateModel
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int CreatedByUserId { get; set; }
        public int ModuleId { get; set; }
        public int DepartmentId { get; set; }
    }
    public class DirectoryUpdateModel
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int LastModifiedByUserId { get; set; }
    }
   
}