using ECMDemo.Data;
using ECMDemo.Business.Model;
using ECMDemo.Common;
using System;
using System.Linq;
using static ECMDemo.Common.EncryptionLib;
using ECMDemo.Business.Common;

namespace ECMDemo.Business
{
    public class Authenticate : IAuthenticate
    {
        public bool Authorize(string Roles, string encodedString, int UserId,out string message)
        {
            message = "Authorization has been denied for this request.";
            bool validFlag = true;
            using (var unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.GetRepository<User>().GetById(UserId);
                if (user == null) return false;
                if (Roles.Length > 0)
                {
                    validFlag = false;
                    
                    string role = unitOfWork.GetRepository<UserRole>().GetById(user.UserRoleId).Name;
                    if (Roles.Split(',').Any(p => p.Equals(role))) validFlag = true;
                }
                if (validFlag)
                {
                    validFlag = JWTEncryptionLib.ValidateToken(encodedString, user, out message);
                }
                    
                return validFlag;
            }
        }
    }
}