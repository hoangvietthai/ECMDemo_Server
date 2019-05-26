using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbInternalDocumentHandler
    {
        Response<InternalDocumentModel> GetById(int Id);
        Response<List<InternalDocumentDisplayModel>> GetAll(int UserId);
        Response<List<InternalDocumentDisplayModel>> GetAllInDirectory(int UserId, int DirectoryId);
        Response<InternalDocumentModel> Create(InternalDocumentCreateModel createModel);
        Response<InternalDocumentModel> Update(int Id, InternalDocumentUpdateModel userModel);
        Response<InternalDocumentModel> Delete(int Id);
        Response<InternalDocumentModel> Register(int Id);
    }
}