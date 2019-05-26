using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class DocumentGroupCateModel
    {
        public int DocumentCateGroupId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
    public class DocumentGroupCateDetailModel
    {
        public int DocumentCateGroupId { get; set; }
        public string Name { get; set; }
        public List<DocumentCategoryModel> Categories { get; set; }
    }
    public class DocumentGroupCateCreateModel
    {
        public string Name { get; set; }
    }
    public class DocumentGroupCateUpdateModel
    {
        public string Name { get; set; }
    }
}