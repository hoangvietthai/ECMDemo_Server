using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Model;
using ECMDemo.Data;
using ECMDemo.Common;
namespace ECMDemo.Business.Handler
{
    public class DbUserHandler : IDbUserHandler
    {
        public Response<UserModel> Create(UserCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<User>().GetAll().OrderByDescending(c => c.UserId).FirstOrDefault();
                    if ((createModel.Password.Length < 6) || (createModel.Password.Length > 18))
                        return new Response<UserModel>(0, "Mật khẩu phải có từ 6-18 ký tự",null);
                    if ((createModel.UserName.Length < 6) || (createModel.UserName.Length > 18))
                        return new Response<UserModel>(0, "Tên tài khoản phải có từ 6-18 ký tự", null);
                    var check_leader = unitOfWork.GetRepository<Department>().GetById(createModel.DepartmentId);
                    User user = new User
                    {
                        DepartmentId = createModel.DepartmentId,
                        FullName = createModel.FullName,
                        IsDelete = false,
                        UserId = 1,
                        CreateOnDate=DateTime.Now,
                        Password=EncryptionLib.EncryptText(createModel.Password),
                        UserName=createModel.UserName,
                        UserRoleId=2
                    };
                    if (last != null) user.UserId = last.UserId + 1;
                    if (check_leader.LeaderId != 0) user.UserRoleId = 3;
                    unitOfWork.GetRepository<User>().Add(user);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(user.UserId);
                    }
                    return new Response<UserModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserModel>(-1, ex.ToString(), null);
            }
        }

        public Response<UserModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(Id);
                    if (user != null)
                    {
                        user.IsDelete = true;
                        unitOfWork.GetRepository<User>().Update(user);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(user.UserId);
                        }
                        return new Response<UserModel>(0, "Lưu thông tin không thành công", null);
                    }
                    else
                        return new Response<UserModel>(0, "Không tìm thấy người dùng", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserModel>(-1, ex.ToString(), null);
            }
        }
        public Response<List<UserDisplayModel>> GetAll(int userId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(userId);
                    if (user == null) return new Response<List<UserDisplayModel>>(0, "User doesn't exsist", null);
                    var list = unitOfWork.GetRepository<User>().GetMany(u => u.IsDelete == false);
                    if (user.UserRoleId > 0)
                    {
                        list = list.Where(c => c.UserRoleId > 0);
                    }
                    var result = list
                        .Join(unitOfWork.GetRepository<Department>().GetAll(),
                        u=>u.DepartmentId,
                        d=>d.DepartmentId,
                        (u,d)=> new UserDisplayModel
                        {
                            Department = d.Name,
                            FullName = u.FullName,
                            UserId = u.UserId,
                            UserName = u.UserName,
                            CreatedOnDate = u.CreateOnDate,
                            UserRoleId=u.UserRoleId
                        })
                         .OrderBy(u => u.UserId)
                         .ToList();
                    return new Response<List<UserDisplayModel>>(1, "", result, result.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<UserDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<BaseUserModel>> GetAllBase(int userId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(userId);
                    if (user == null) return new Response<List<BaseUserModel>>(0, "User doesn't exsist", null);
                    var list = unitOfWork.GetRepository<User>().GetMany(u => u.IsDelete == false);
                    if (user.UserRoleId > 0)
                    {
                        list = list.Where(c => c.UserRoleId > 0);
                    }
                    var result= list
                         .Select(u => new BaseUserModel
                         {
                             DepartmentId = u.DepartmentId,
                             FullName = u.FullName,
                             UserId = u.UserId,
                             UserName=u.UserName,
                             CreatedOnDate=u.CreateOnDate
                         })
                         .OrderBy(u => u.UserId)
                         .ToList();
                    return new Response<List<BaseUserModel>>(1, "", result, result.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BaseUserModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<BaseUserModel>> GetByDepartment(int DepartmentId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<User>().GetMany(u => u.IsDelete == false && u.DepartmentId == DepartmentId && u.UserRoleId > 0)
                         .Select(u => new BaseUserModel
                         {
                             DepartmentId = u.DepartmentId,
                             FullName = u.FullName,
                             UserId = u.UserId,
                             UserName = u.UserName,
                             CreatedOnDate = u.CreateOnDate
                         })
                         .OrderBy(u => u.UserId)
                         .ToList();
                    return new Response<List<BaseUserModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BaseUserModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<UserDetailModel> GetDetail(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user=unitOfWork.GetRepository<User>().GetById(Id);
                    
                    if (user != null)
                    {
                        UserDetailModel response = new UserDetailModel
                        {
                            FullName = user.FullName,
                            UserId = user.UserId,
                            CreatedOnDate = user.CreateOnDate,
                            PassWord = user.Password,
                            UserName = user.UserName,
                            Department = ""
                        };
                        var department = unitOfWork.GetRepository<Department>().GetById(user.DepartmentId);
                        if (department != null) response.Department = department.Name;
                        return new Response<UserDetailModel>(1, "", response);
                    }
                    else
                    return new Response<UserDetailModel>(0, "Không tìm thấy người dùng",null);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserDetailModel>(-1, ex.ToString(), null);
            }
        }
        public Response<UserModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(Id);
                    if (user != null)
                    {
                        
                        return new Response<UserModel>(1, "", new UserModel
                        {
                            DepartmentId = user.DepartmentId,
                            FullName = user.FullName,
                            IsDelete = user.IsDelete,
                            UserId = user.UserId,
                            UserName=user.UserName,
                            Password=user.Password,
                            CreatedOnDate=user.CreateOnDate 
                        });
                   
                        
                    }
                    return new Response<UserModel>(0, "Không tìm thấy người dùng", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserModel>(-1, ex.ToString(), null);
            }
        }
        public Response<UserModel> Update(int Id,UserUpdateModel userUpdateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    if ((userUpdateModel.Password.Length < 6) || (userUpdateModel.Password.Length > 18))
                        return new Response<UserModel>(0, "Mật khẩu phải có từ 6-18 ký tự", null);
                    if ((userUpdateModel.UserName.Length < 6) || (userUpdateModel.UserName.Length > 18))
                        return new Response<UserModel>(0, "Tên tài khoản phải có từ 6-18 ký tự", null);
                    var user = unitOfWork.GetRepository<User>().GetById(Id);
                    if (user != null)
                    {
                        user.UserName = userUpdateModel.UserName;
                        user.FullName = userUpdateModel.FullName;
                        user.DepartmentId = userUpdateModel.DepartmentId;
                        user.Password = EncryptionLib.EncryptText(userUpdateModel.Password);
                        user.DepartmentId = userUpdateModel.DepartmentId;
                        unitOfWork.GetRepository<User>().Update(user);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(user.UserId);
                        }
                        return new Response<UserModel>(0, "Lưu thông tin không thành công", null);
                    }
                    else
                        return new Response<UserModel>(0, "Không tìm thấy người dùng", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<BaseUserModel>> GetConfirmUsers(int userId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(userId);
                    if (user == null) return new Response<List<BaseUserModel>>(0, "User doesn't exsist", null);
                    var list = unitOfWork.GetRepository<User>().GetMany(u => u.IsDelete == false && u.UserRoleId<user.UserRoleId && u.UserRoleId>0);
                    //if (user.UserRoleId > 0)
                    //{
                    //    list = list.Where(c => c.UserRoleId > 0);
                    //}
                    var result = list
                         .Select(u => new BaseUserModel
                         {
                             DepartmentId = u.DepartmentId,
                             FullName = u.FullName,
                             UserId = u.UserId,
                             UserName = u.UserName,
                             CreatedOnDate = u.CreateOnDate
                         })
                         .OrderBy(u => u.UserId)
                         .ToList();
                    return new Response<List<BaseUserModel>>(1, "", result, result.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BaseUserModel>>(-1, ex.ToString(), null);
            }
        }
    }
}