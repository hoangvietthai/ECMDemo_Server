using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentUnifyHandler
    {
        Response<DocumentUnifyModel> GetById(int Id);
        Response<List<DocumentUnifyModel>> GetAll();
        Response<DocumentUnifyModel> Create(DocumentUnifyCreateModel createModel);
        Response<DocumentUnifyModel> Update(int Id, DocumentUnifyUpdateModel updateModel);
        Response<DocumentUnifyModel> Delete(int Id);
        Response<DocumentUnifyResponseModel> CreateResponse(DocumentUnifyResponseModel createModel);
        Response<DocumentUnifyResponseModel> GetResponse(int UserId, int DocumentUnifyId);
        Response<List<DocumentUnifyResponseDisplayModel>> GetAllResponses(int Id);
        Response<DocumentUnifyModel> ReCreate(int UnifyId, RecreateDocumentUnifyModel createModel);
        Response<DocumentUnifyModel> Finish(int Id, FinishUnifyModel model);

    }
}