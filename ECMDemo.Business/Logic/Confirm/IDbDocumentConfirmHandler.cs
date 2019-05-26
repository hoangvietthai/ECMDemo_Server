using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentConfirmHandler
    {
        Response<DocumentConfirmModel> GetById(int Id);
        Response<List<DocumentConfirmModel>> GetAll();
        Response<DocumentConfirmModel> Create(DocumentConfirmCreateModel createModel);
        Response<DocumentConfirmModel> Update(int Id, DocumentConfirmUpdateModel updateModel);
        Response<DocumentConfirmModel> Delete(int Id);
        Response<DocumentConfirmResponseModel> CreateResponse(DocumentConfirmResponseModel createModel);
        Response<DocumentConfirmResponseDisplayModel> GetResponse(int UserId, int DocumentConfirmId);
        Response<DocumentConfirmModel> ReCreate(int UnifyId, RecreateDocumentConfirmModel createModel);
        Response<DocumentConfirmModel> Finish(int Id, FinishConfirmModel model);
        Response<DocumentConfirmResponseDisplayModel> GetResponse(int Id);
    }
}