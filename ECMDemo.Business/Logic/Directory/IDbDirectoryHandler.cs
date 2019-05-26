using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDirectoryHandler
    {
        Response<DirectoryModel> GetById(int Id);
        Response<List<DirectoryModel>> GetAll();
        Response<List<DirectoryModel>> GetAll(int UserId);
        Response<List<DirectoryModel>> GetAllByModule(int moduleid);
        Response<List<DirectoryModel>> GetByParentId(int ParentId,int ModuleId);
        Response<DirectoryModel> Create(DirectoryCreateModel createModel);
        Response<DirectoryModel> Update(int Id, DirectoryUpdateModel userUpdateModel);
        Response<DirectoryModel> Delete(int Id);
        Response<List<DirectoryNodeModel>> GetNodes(int ModuleId,int UserId);
    }
}