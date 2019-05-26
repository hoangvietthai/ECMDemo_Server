using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class BaseUserModel
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOnDate { get; set; }
      
    }
    public class UserDisplayModel
    {
        public int UserId { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int UserRoleId { get; set; }
    }
    public class UserDetailModel
    {
        public int UserId { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public string PassWord { get; set; }
    }
    public class UserModel: BaseUserModel
    {
        public string Password { get; set; }
        public bool IsDelete { get; set; }
    }
    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public string FullName { get; set; }
    }
    public class UserUpdateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public string FullName { get; set; }
    }
}