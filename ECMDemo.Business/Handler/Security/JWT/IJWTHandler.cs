using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMDemo.Business.Model;

namespace ECMDemo.Business
{
    public interface IJWTHandler
    {
        Response<string> GetToken(string Username, string Password);
        //Response<SessionModel> VerifyToken(string token);
    }
}
