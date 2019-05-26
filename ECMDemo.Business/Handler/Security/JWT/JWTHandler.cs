using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMDemo.Business.Model;
using ECMDemo.Common;
//using YODOSE.Common;
using ECMDemo.Data;

namespace ECMDemo.Business
{
    public class JWTHandler : IJWTHandler
    {
        public Response<string> GetToken(string Username, string Password)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().Get(p => p.UserName.Equals(Username) && p.Password.Equals(Password));
                    if (user != null)
                    {
                        string token = JWTEncryptionLib.GenerateToken(user.UserName,user.SessionLoginCode.ToString(), 24);
                        return new Response<string>(1, "", token);
                    }
                    return new Response<string>(0, "User doesn't exsist!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<string>(-1, ex.ToString(), null);
            }
        }
    }
}
