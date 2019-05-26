using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentStatusHandler
    {
        Response<DocumentStatusModel> GetById(int Id);
        Response<BaseDocumentStatusModel> GetNameById(int Id);
        Response<List<BaseDocumentStatusModel>> GetAll();
        Response<DocumentStatusModel> Create(DocumentStatusCreateModel createModel);
        Response<DocumentStatusModel> Update(int Id, DocumentStatusUpdateModel updateModel);
        Response<BaseDocumentStatusModel> Delete(int Id);
      
    }
}