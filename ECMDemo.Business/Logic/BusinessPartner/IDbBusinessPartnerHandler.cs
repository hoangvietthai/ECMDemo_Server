using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbBusinessPartnerHandler
    {
        Response<BusinessPartnerModel> GetById(int Id);
        Response<List<BusinessPartnerDisplayModel>> GetAll();
        Response<BusinessPartnerModel> Create(BusinessPartnerCreateModel createModel);
        Response<BusinessPartnerModel> Update(int Id, BusinessPartnerUpdateModel userUpdateModel);
        Response<BaseBusinessPartnerModel> Delete(int Id);
    }
}