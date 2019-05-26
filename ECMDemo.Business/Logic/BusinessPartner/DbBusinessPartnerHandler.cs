using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbBusinessPartnerHandler : IDbBusinessPartnerHandler
    {
        public Response<BusinessPartnerModel> Create(BusinessPartnerCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<BusinessPartner>().GetAll().OrderByDescending(c => c.PartnerId).FirstOrDefault();

                    BusinessPartner partner = new BusinessPartner
                    {
                        ActualAddress=createModel.ActualAddress,
                        AgencyCode=createModel.AgencyCode,
                        BusinessCode=createModel.BusinessCode,
                        BusinessRegisteredCode=createModel.BusinessRegisteredCode,
                        BusinessTypeId=createModel.BusinessTypeId,
                        Email=createModel.Email,
                        Fax=createModel.Fax,
                        Name=createModel.Name,
                        Note=createModel.Note,
                        PartnerId=1,
                        PhoneNumber=createModel.PhoneNumber,
                        RegisteredAddress=createModel.RegisteredAddress,
                        ResponsibleUserId=createModel.ResponsibleUserId,
                        TaxCode=createModel.TaxCode,
                        Website=createModel.Website
                    };


                    if (last != null) partner.PartnerId = last.PartnerId + 1;
                    unitOfWork.GetRepository<BusinessPartner>().Add(partner);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(partner.PartnerId);
                    }
                    return new Response<BusinessPartnerModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BusinessPartnerModel>(-1, ex.ToString(), null);
            }
        }
      
        public Response<BaseBusinessPartnerModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var partner = unitOfWork.GetRepository<BusinessPartner>().GetById(Id);
                    if (partner != null)
                    {

                        unitOfWork.GetRepository<BusinessPartner>().Delete(partner);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<BaseBusinessPartnerModel>(1, "", null);
                        }
                        return new Response<BaseBusinessPartnerModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<BaseBusinessPartnerModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BaseBusinessPartnerModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<BusinessPartnerDisplayModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<BusinessPartner>().GetAll()
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        b => b.ResponsibleUserId,
                        u => u.UserId,
                        (d, u) => new BusinessPartnerDisplayModel
                        {
                          PartnerId=d.PartnerId,
                          PhoneNumber=d.PhoneNumber,
                          ResponsibleUserFullName=u.FullName,
                          ResponsibleUserId=d.ResponsibleUserId,
                          Name = d.Name
                        })
                         .OrderByDescending(u => u.PartnerId)
                         .ToList();
                    return new Response<List<BusinessPartnerDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BusinessPartnerDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<BusinessPartnerModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<BusinessPartner>().GetById(Id);
                    if (doc != null) return new Response<BusinessPartnerModel>(1, "", Ultis.ConvertSameData<BusinessPartnerModel>(doc));
                    else
                        return new Response<BusinessPartnerModel>(0, "Đối tác không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BusinessPartnerModel>(-1, ex.ToString(), null);
            }
        }

        public Response<BusinessPartnerModel> Update(int Id, BusinessPartnerUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var dir = unitOfWork.GetRepository<BusinessPartner>().GetById(Id);
                    if (dir != null)
                    {
                        Ultis.TransferValues(dir, updateModel);
                        unitOfWork.GetRepository<BusinessPartner>().Update(dir);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(dir.PartnerId);
                        }
                        return new Response<BusinessPartnerModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<BusinessPartnerModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BusinessPartnerModel>(-1, ex.ToString(), null);
            }
        }
    }
}