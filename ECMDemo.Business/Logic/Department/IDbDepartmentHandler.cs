using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbDepartmentHandler
    {
        Response<DepartmentModel> GetById(int Id);
        Response<List<DepartmentDisplayModel>> GetAll();
        Response<List<DepartmentModel>> GetByParentId(int ParentId);
        Response<DepartmentModel> Create(DepartmentCreateModel createModel);
        Response<DepartmentModel> Update(int Id, DepartmentupdateModel userUpdateModel);
        Response<DepartmentModel> Delete(int Id);
        Response<TreeNode> GetNodes();
    }
}