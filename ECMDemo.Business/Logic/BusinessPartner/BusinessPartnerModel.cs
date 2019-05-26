using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Model
{
    public class BusinessPartnerDisplayModel
    {
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public int ResponsibleUserId { get; set; }
        public string ResponsibleUserFullName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class BaseBusinessPartnerModel
    {
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public int ResponsibleUserId { get; set; }
    }
    public class BusinessPartnerModel: BaseBusinessPartnerModel
    {
        public int BusinessTypeId { get; set; }
        public string TaxCode { get; set; }
        public string AgencyCode { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessRegisteredCode { get; set; }
        public string RegisteredAddress { get; set; }
        public string ActualAddress { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
    }
    public class BusinessPartnerCreateModel
    {
        public string Name { get; set; }
        public int BusinessTypeId { get; set; }
        public string TaxCode { get; set; }
        public string AgencyCode { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessRegisteredCode { get; set; }
        public int ResponsibleUserId { get; set; }
        public string RegisteredAddress { get; set; }
        public string ActualAddress { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
    }
    public class BusinessPartnerUpdateModel
    {
        public string Name { get; set; }
        public int BusinessTypeId { get; set; }
        public string TaxCode { get; set; }
        public string AgencyCode { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessRegisteredCode { get; set; }
        public int ResponsibleUserId { get; set; }
        public string RegisteredAddress { get; set; }
        public string ActualAddress { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
    }
}