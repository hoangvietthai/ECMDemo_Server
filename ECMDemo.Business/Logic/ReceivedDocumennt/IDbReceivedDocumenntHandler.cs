using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbReceivedDocumenntHandler
    {
        Response<ReceivedDocumentModel> GetById(int Id);
        Response<List<BaseReceivedDocumentModel>> GetAllBase();
        Response<List<ReceivedDocumentDisplayModel>> GetAll(int UserId);
        Response<ReceivedDocumentModel> Create(ReceivedDocumentCreateModel createModel);
        Response<ReceivedDocumentModel> Update(int Id, ReceivedDocumentUpdateModel updateModel);
        Response<BaseReceivedDocumentModel> Delete(int Id);
        Response<ReceivedDocumentModel> Register(int Id);
    }
}