using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbContactPersonHandler
    {
        Response<ContactPersonModel> GetById(int Id);
        Response<List<ContactPersonDisplayModel>> GetAll();
        Response<List<BaseContactPersonModel>> GetByPartnerId(int PartnerId);
        Response<ContactPersonModel> Create(ContactPersonCreateModel createModel);
        Response<ContactPersonModel> Update(int Id, ContactPersonUpdateModel userModel);
        Response<ContactPersonModel> Delete(int Id);
    }
}