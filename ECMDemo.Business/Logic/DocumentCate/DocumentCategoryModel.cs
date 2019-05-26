using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class DocumentCategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryGroupId { get; set; }
    }
    public class DocumentCategoryCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryGroupId { get; set; }
    }
    public class DocumentCategoryUpdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}