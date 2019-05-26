using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class DepartmentDisplayModel
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Leader { get; set; }
        public int ParentId { get; set; }
    }
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? LeaderId { get; set; }
        public int ParentId { get; set; }
    }
    public class DepartmentCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? LeaderId { get; set; }
        public int ParentId { get; set; }
    }
    public class DepartmentupdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? LeaderId { get; set; }
        public int ParentId { get; set; }
    }
    public class TreeNode
    {
        public DepartmentDisplayModel data { get; set; }
        public List<TreeNode> children { get; set; }
    }
}