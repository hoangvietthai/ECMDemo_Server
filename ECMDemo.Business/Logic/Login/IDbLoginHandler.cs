using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbLoginHandler
    {
        Response<LoginResponse> CheckLogin(LoginModel loginModel);
        //Response<ChangePassModel> ChangePassword(int UserId, ChangePassModel loginModel);
    }
}