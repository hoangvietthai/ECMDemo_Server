using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbSendDocumentHandler : IDbSendDocumentHandler
    {
        public Response<SendDocumentModel> Create(SendDocumentCreateModel createModel)
        {

            try
            {
                using (var unitOfwork = new UnitOfWork())
                {
                    var user = unitOfwork.GetRepository<User>().GetById(createModel.CreatedByUserId);
                    if (user == null) return new Response<SendDocumentModel>(0, "", null);
                    SendDocument entity = new SendDocument
                    {
                        SecretLevel = createModel.SecretLevel,
                        ResponsibleUserId = createModel.ResponsibleUserId,
                        ResponseForRDocId = createModel.ResponseForRDocId,
                        SenderId = createModel.SenderId,
                        SignedByUserId = createModel.SignedByUserId,
                        Summary = createModel.Summary,
                        WrittenByUserId = createModel.WrittenByUserId,
                        ResponseDeadline = createModel.ResponseDeadline,
                        ReceiverContactPersonId = createModel.ReceiverContactPersonId,
                        ReceiverId = createModel.ReceiverId,
                        Name = createModel.Name,
                        DocumentStatusId = createModel.DocumentStatusId,
                        CategoryId = createModel.CategoryId,
                        CreatedByUserId = createModel.CreatedByUserId,
                        CreatedOnDate = DateTime.Now,
                        DeliveryMethodId = createModel.DeliveryMethodId,
                        LastModifiedOnDate = DateTime.Now,
                        DepartmentId = user.DepartmentId,
                        IsDelete = false,
                        LastModifiedByUserId = createModel.CreatedByUserId,
                        SendDocumentId = 1,
                        ResignedNumber = createModel.ResignedNumber,
                        ResignedOnDate = createModel.ResignedOnDate,
                        AttachedFileUrl = createModel.AttachedFileUrl
                    };
                    var last = unitOfwork.GetRepository<SendDocument>().GetAll().OrderByDescending(d => d.SendDocumentId).FirstOrDefault();
                    if (last != null) entity.SendDocumentId = last.SendDocumentId + 1;
                    unitOfwork.GetRepository<SendDocument>().Add(entity);
                    if (unitOfwork.Save() >= 1)
                    {
                        return GetById(entity.SendDocumentId);
                    }
                    return new Response<SendDocumentModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<SendDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<SendDocumentModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<SendDocument>().GetById(Id);
                    if (entity != null)
                    {
                        entity.IsDelete = true;
                        unitOfWork.GetRepository<SendDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<SendDocumentModel>(1, "", new SendDocumentModel());
                        }
                    }
                    return new Response<SendDocumentModel>(0, "Tài liệu không tồn tại", null);

                }
            }
            catch (Exception ex)
            {
                return new Response<SendDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<SendDocumentDisplayModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<SendDocumentDisplayModel>>(0, "", null);
                    var _list = unitOfWork.GetRepository<SendDocument>().GetMany(d => d.IsDelete == false);
                    if (user.UserRoleId > 1)
                    {
                        _list=_list.Where(d => d.DepartmentId == user.DepartmentId);
                    }
                    
                    var list = _list
                         .Join(unitOfWork.GetRepository<Department>().GetAll(),
                            d => d.DepartmentId,
                            u => u.DepartmentId,
                            (d, u) => new
                            {
                                ReceiverId = d.ReceiverId,
                                SendDocumentId = d.SendDocumentId,
                                DepartmentName = u.Name,
                                Summary = d.Summary,
                                ResignedNumber = d.ResignedNumber,
                                ResignedOnDate = d.ResignedOnDate,
                                SignedByUserId = d.SignedByUserId,
                                CreatedOnDate = d.CreatedOnDate,
                                Name = d.Name,
                                DocumentStatusId = d.DocumentStatusId,
                                DocumentProcessId = d.DocumentProcessId
                            }
                        )
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.SignedByUserId,
                        u => u.UserId,
                        (d, u) => new
                        {
                            ReceiverId = d.ReceiverId,
                            SendDocumentId = d.SendDocumentId,
                            DepartmentName = d.DepartmentName,
                            Summary = d.Summary,
                            ResignedNumber = d.ResignedNumber,
                            ResignedOnDate = d.ResignedOnDate,
                            SignedByUserFullName = u.FullName,
                            CreatedOnDate = d.CreatedOnDate,
                            Name = d.Name,
                            DocumentStatusId = d.DocumentStatusId,
                            DocumentProcessId = d.DocumentProcessId
                        }
                        )
                        .Join(unitOfWork.GetRepository<BusinessPartner>().GetAll(),
                            d => d.ReceiverId,
                            b => b.PartnerId,
                            (d, b) => new SendDocumentDisplayModel
                            {
                                SendDocumentId = d.SendDocumentId,
                                Receiver = b.Name,
                                DepartmentName = d.DepartmentName,
                                ResignedNumber = d.ResignedNumber,
                                ResignedOnDate = d.ResignedOnDate,
                                SignedByUserFullName = d.SignedByUserFullName,
                                CreatedOnDate = d.CreatedOnDate,
                                Name = d.Name,
                                DocumentStatusId = d.DocumentStatusId,
                                DocumentProcessId = d.DocumentProcessId
                            }).OrderBy(d => d.SendDocumentId).ToList();

                    return new Response<List<SendDocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<SendDocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<SendDocumentModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<SendDocument>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<SendDocumentModel>(1, "", new SendDocumentModel
                        {
                            SecretLevel = entity.SecretLevel,
                            ResponsibleUserId = entity.ResponsibleUserId,
                            ResponseForRDocId = entity.ResponseForRDocId,
                            SenderId = entity.SenderId,
                            SignedByUserId = entity.SignedByUserId,
                            Summary = entity.Summary,
                            WrittenByUserId = entity.WrittenByUserId,
                            ResponseDeadline = entity.ResponseDeadline,
                            ReceiverContactPersonId = entity.ReceiverContactPersonId,
                            ReceiverId = entity.ReceiverId,
                            Name = entity.Name,
                            DocumentStatusId = entity.DocumentStatusId,
                            CategoryId = entity.CategoryId,
                            CreatedByUserId = entity.LastModifiedByUserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DeliveryMethodId = entity.DeliveryMethodId,
                            LastModifiedOnDate = entity.LastModifiedOnDate,
                            DepartmentId = entity.DepartmentId,
                            IsDelete = entity.IsDelete,
                            LastModifiedByUserId = entity.CreatedByUserId,
                            SendDocumentId = entity.SendDocumentId,
                            ResignedNumber = entity.ResignedNumber,
                            ResignedOnDate = entity.ResignedOnDate,
                            AttachedFileUrl = entity.AttachedFileUrl,
                            DocumentProcessId = entity.DocumentProcessId
                        });
                    }
                    return new Response<SendDocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<SendDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<SendDocumentModel> Register(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<SendDocument>().GetById(Id);
                    if (entity != null)
                    {
                        if (entity.DocumentProcessId != null)
                        {
                            DocumentProcessModel current = new DbDocumentProcessHandler().GetCurrentProcess((int)entity.DocumentProcessId).Data;
                            if (current.TaskType == (int)TaskType.REGISTER)
                            {
                                if (entity.ResignedNumber != null) return new Response<SendDocumentModel>(0, "Tài liệu đã được đăng ký rồi", null);
                                entity.ResignedNumber = 1;
                                var last = unitOfWork.GetRepository<SendDocument>().GetMany(d => d.ResignedNumber != null).OrderByDescending(d => d.ResignedNumber).FirstOrDefault();
                                if (last != null) entity.ResignedNumber = last.ResignedNumber + 1;
                                entity.ResignedOnDate = DateTime.Now;
                                unitOfWork.GetRepository<SendDocument>().Update(entity);
                                //
                                var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == entity.DocumentProcessId && d.TaskType == (int)TaskType.REGISTER);
                                tmp.Status = (int)DocumentProcessStatus.FINISHED;
                                tmp.RelatedId = entity.SendDocumentId;
                                unitOfWork.GetRepository<DocumentProcess>().Update(tmp);
                                if (unitOfWork.Save() >= 1)
                                {
                                    return GetById(entity.SendDocumentId);
                                }
                                return new Response<SendDocumentModel>(0, "Lưu thông tin không thành công", null);
                            }
                            return new Response<SendDocumentModel>(0, "Quy trình hiện tại không phải đăng ký", null);
                        }
                        return new Response<SendDocumentModel>(0, "Bạn chưa khởi tạo quy trình xử lý tài liệu này", null);

                    }
                    return new Response<SendDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<SendDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<SendDocumentModel> Update(int Id, SendDocumentUpdateModel userUpdateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<SendDocument>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, userUpdateModel);
                        unitOfWork.GetRepository<SendDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.SendDocumentId);
                        }
                        return new Response<SendDocumentModel>(0, "Lưu thông tin không thành công", null);
                    }
                    return new Response<SendDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<SendDocumentModel>(-1, ex.ToString(), null);
            }
        }
    }
}