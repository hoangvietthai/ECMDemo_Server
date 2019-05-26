using ECMDemo.Business.Model;
using System;

namespace ECMDemo.Business
{
    public interface IAuthenticate
    {
        bool Authorize(string Roles, string encodedString, int UserId,out string message);
    }
    
}
