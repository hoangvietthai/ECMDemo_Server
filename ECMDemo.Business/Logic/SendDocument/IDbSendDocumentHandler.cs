using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbSendDocumentHandler
    {
        Response<SendDocumentModel> GetById(int Id);
        Response<List<SendDocumentDisplayModel>> GetAll(int UserId);
        //Response<List<DocumentModel>> GetByType(int TypeId);
        //Response<List<DocumentModel>> GetByUser(int UserId);
        //Response<List<DocumentDisplayModel>> GetDocsInDirectory(int DirectoryId);
        Response<SendDocumentModel> Create(SendDocumentCreateModel createModel);
        Response<SendDocumentModel> Update(int Id, SendDocumentUpdateModel userUpdateModel);
        Response<SendDocumentModel> Delete(int Id);
        Response<SendDocumentModel> Register(int Id);
    }
}