using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ECMDemo.Business.Model;
using ECMDemo.Common;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbLoginHandler : IDbLoginHandler
    {
        public Response<LoginResponse> CheckLogin(LoginModel loginModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().Get(p => p.UserName.Equals(loginModel.UserName, StringComparison.Ordinal));
                    if (user != null)
                    {
                        if (EncryptionLib.EncryptText(loginModel.Password).Equals(user.Password))
                        {
                            if (user.IsDelete) return new Response<LoginResponse>(0, "Tài khoản không tồn tại", null);
                            if (user.IsDisable) return new Response<LoginResponse>(0, "Tài khoản của bạn đã bị khóa.", null);
                            var department = unitOfWork.GetRepository<Department>().Get(s => s.DepartmentId == user.DepartmentId);
                            user.LastLoginOnDate = DateTime.Now;
                            user.SessionLoginCode = Guid.NewGuid();
                            unitOfWork.GetRepository<User>().Update(user);
                            if (unitOfWork.Save() >= 1)
                            {
                                var response_ = new LoginResponse
                                {
                                    Token = JWTEncryptionLib.GenerateToken(user.UserName, user.SessionLoginCode.ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"])),
                                    User = new SessionModel
                                    {
                                        UserId = user.UserId,
                                        UserName = user.UserName,
                                        Department = department.Name,
                                        DepartmentId = user.DepartmentId,
                                        FullName = user.FullName,
                                        UserRole = user.UserRoleId
                                    }
                                };

                                
                                 return new Response<LoginResponse>(1, "", response_);
                            }
                            return new Response<LoginResponse>(0, "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại", null);
                        }
                        return new Response<LoginResponse>(0, "Mật không không chính xác", null);
                    }
                    return new Response<LoginResponse>(0, "Tài khoản không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<LoginResponse>(-1, ex.ToString(), null);
            }
        }
    }
}