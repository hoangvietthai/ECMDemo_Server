using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbContactPersonHandler : IDbContactPersonHandler
    {
        public Response<ContactPersonModel> Create(ContactPersonCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<ContactPerson>().GetAll().OrderByDescending(c => c.ContactPersonId).FirstOrDefault();

                    ContactPerson entity = new ContactPerson
                    {
                        ContactPersonId = 1,
                        Email = createModel.Email,
                        Note = createModel.Note,
                        OfficePhoneNumber = createModel.OfficePhoneNumber,
                        PartnerId = createModel.PartnerId,
                        PersonalPhoneNumber = createModel.PersonalPhoneNumber,
                        Position = createModel.Position,
                        Name = createModel.Name
                    };


                    if (last != null) entity.ContactPersonId = last.ContactPersonId + 1;
                    unitOfWork.GetRepository<ContactPerson>().Add(entity);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.ContactPersonId);
                    }
                    return new Response<ContactPersonModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ContactPersonModel>(-1, ex.ToString(), null);
            }
        }

        public Response<ContactPersonModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<ContactPerson>().GetById(Id);
                    if (entity != null)
                    {

                        unitOfWork.GetRepository<ContactPerson>().Delete(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<ContactPersonModel>(1, "", null);
                        }
                        return new Response<ContactPersonModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<ContactPersonModel>(0, "Không tìm thấy thông tin", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ContactPersonModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<ContactPersonDisplayModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<ContactPerson>().GetAll()
                        .Join(unitOfWork.GetRepository<BusinessPartner>().GetAll(),
                        d => d.PartnerId,
                        u => u.PartnerId,
                        (d, u) => new ContactPersonDisplayModel
                        {

                            Name = d.Name,
                            ContactPersonId = d.ContactPersonId,
                            OfficePhoneNumber = d.OfficePhoneNumber,
                            Partner = u.Name,
                            PersonalPhoneNumber = d.PersonalPhoneNumber,
                            Position = d.Position
                        })
                         .OrderByDescending(u => u.Partner)
                         .ToList();
                    return new Response<List<ContactPersonDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<ContactPersonDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<ContactPersonModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<ContactPerson>().GetById(Id);
                    if (doc != null) return new Response<ContactPersonModel>(1, "", Ultis.ConvertSameData<ContactPersonModel>(doc));
                    else
                        return new Response<ContactPersonModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ContactPersonModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<BaseContactPersonModel>> GetByPartnerId(int PartnerId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<ContactPerson>().GetMany(c => c.PartnerId == PartnerId)
                        .Select(c => new BaseContactPersonModel
                        {
                            ContactPersonId = c.ContactPersonId,
                            Name = c.Name,
                            Position = c.Position
                        })
                         .OrderByDescending(u => u.ContactPersonId)
                         .ToList();
                    return new Response<List<BaseContactPersonModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BaseContactPersonModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<ContactPersonModel> Update(int Id, ContactPersonUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var dir = unitOfWork.GetRepository<ContactPerson>().GetById(Id);
                    if (dir != null)
                    {
                        Ultis.TransferValues(dir, updateModel);
                        unitOfWork.GetRepository<ContactPerson>().Update(dir);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(dir.ContactPersonId);
                        }
                        return new Response<ContactPersonModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<ContactPersonModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ContactPersonModel>(-1, ex.ToString(), null);
            }
        }
    }
}