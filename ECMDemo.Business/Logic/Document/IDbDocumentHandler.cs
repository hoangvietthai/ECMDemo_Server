using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentHandler
    {
        Response<DocumentModel> GetById(int Id);
        Response<List<DocumentDisplayModel>> GetAll();
        Response<List<DocumentDisplayModel>> GetAll(int UserId);
        Response<List<DocumentModel>> GetByType(int TypeId);
        Response<List<DocumentModel>> GetByUser(int UserId);
        Response<List<DocumentDisplayModel>> GetDocsInDirectory(int DirectoryId);
        Response<List<DocumentDisplayModel>> GetAllInDepartment(int Department);
        Response<DocumentModel> Create(int UserId,DocumentCreateModel createModel);
        Response<DocumentModel> Update(int Id, DocumentUpdateModel userUpdateModel);
        Response<DocumentModel> Delete(int Id);
        Response<List<DocumentDisplayModel>> GetAll(FilterDocument filter);
        Response<DocumentModel> ShareToDepartment(int UserId, ShareDocumentModel model);
        Response<List<DocumentDisplayModel>> GetShareDocuments(int UserId);


    }
}