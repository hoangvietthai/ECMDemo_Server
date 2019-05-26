using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentGroupCateHandler
    {
        Response<DocumentGroupCateModel> GetById(int Id);
        Response<DocumentGroupCateModel> GetActiveGroup();
        Response<DocumentGroupCateModel> ChangeActiveGroup(int Id);
        Response<DocumentGroupCateDetailModel> GetDetail(int Id);
        Response<List<DocumentGroupCateModel>> GetAll();
        Response<DocumentGroupCateModel> Create(DocumentGroupCateCreateModel createModel);
        Response<DocumentGroupCateModel> Update(int Id, DocumentGroupCateUpdateModel userUpdateModel);
        Response<DocumentGroupCateModel> Delete(int Id);
    }
}