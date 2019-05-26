using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDocumentProcessHandler
    {
        Response<DocumentProcessModel> GetById(int Id,int order);
        Response<List<DocumentProcessModel>> GetListProcess(int Id);
        Response<int> Create(int DocumentId,int ModuleType,List<DocumentProcessCreateModel> list);
        Response<DocumentProcessModel> GetCurrentProcess(int Id);
        Response<int> CreateAuto(int DocumentId, int ModuleType);
        Response<DocumentProcessModel> ChangeStatus(int Id,int OrderIndex,int DocumentId, int Status);
        Response<DocumentProcessModel> GetNext(int Id);
    }
}