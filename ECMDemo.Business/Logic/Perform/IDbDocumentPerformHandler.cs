using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentPerformHandler
    {
        Response<DocumentPerformModel> GetById(int Id);
        Response<List<DocumentPerformModel>> GetAll();
        Response<DocumentPerformModel> Create(DocumentPerformCreateModel createModel);
        Response<DocumentPerformModel> Update(int Id, DocumentPerformUpdateModel updateModel);
        Response<DocumentPerformModel> Delete(int Id);
        Response<DocumentPerformResponseModel> CreateResponse(DocumentPerformResponseModel createModel);
        Response<DocumentPerformResponseModel> GetResponse(int UserId, int DocumentPerformId);
        Response<List<DocumentPerformResponseDisplayModel>> GetAllResponses(int Id);
        Response<DocumentPerformModel> ReCreate(int PerformId, RecreateDocumentPerformModel createModel);
        Response<DocumentPerformModel> Finish(int Id, FinishPerformModel model);

    }
}