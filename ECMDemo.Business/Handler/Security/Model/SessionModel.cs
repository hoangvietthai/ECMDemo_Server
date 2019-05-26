using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public SessionModel User { get; set; }
    }
    public class SessionModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public int UserRole { get; set; }
    }
}