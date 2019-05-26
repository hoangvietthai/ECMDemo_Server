using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentCategoryHandler
    {
        Response<DocumentCategoryModel> GetById(int Id);
        Response<List<DocumentCategoryModel>> GetAll();
        Response<List<DocumentCategoryModel>> GetAll(int groupId);
        Response<DocumentCategoryModel> Create(DocumentCategoryCreateModel createModel);
        Response<DocumentCategoryModel> Update(int Id, DocumentCategoryUpdateModel userUpdateModel);
        Response<DocumentCategoryModel> Delete(int Id);
    }
}