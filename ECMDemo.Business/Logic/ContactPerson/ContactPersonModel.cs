using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class ContactPersonDisplayModel
    {
        public int ContactPersonId { get; set; }
        public string Name { get; set; }
        public string Partner { get; set; }
        public string Position { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
    }
    public class BaseContactPersonModel
    {
        public int ContactPersonId { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
    }
    public class ContactPersonModel: BaseContactPersonModel
    {
        public int PartnerId { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }

    public class ContactPersonCreateModel
    {
        public int PartnerId { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        
        public string OfficePhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
    public class ContactPersonUpdateModel
    {
        public int PartnerId { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}