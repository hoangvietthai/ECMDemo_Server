using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbUserHandler
    {
        Response<UserModel> GetById(int Id);
        Response<UserDetailModel> GetDetail(int Id);
        Response<List<UserDisplayModel>> GetAll(int userId);

        Response<List<BaseUserModel>> GetAllBase(int userId);
        Response<List<BaseUserModel>> GetConfirmUsers(int userId);
        Response<List<BaseUserModel>> GetByDepartment(int DepartmentId);
        Response<UserModel> Create(UserCreateModel createModel);
        Response<UserModel> Update(int Id,UserUpdateModel userUpdateModel);
        Response<UserModel> Delete(int Id);
       
    }
}