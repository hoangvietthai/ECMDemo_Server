using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbReceivedDocumentHandler : IDbReceivedDocumenntHandler
    {
        public Response<ReceivedDocumentModel> Create(ReceivedDocumentCreateModel createModel)
        {
            try
            {
                using (var unitOfwork = new UnitOfWork())
                {
                    var user=unitOfwork.GetRepository<User>().GetById(createModel.CreatedByUserId);
                    if (user == null) return new Response<ReceivedDocumentModel>(0, "", null);

                    ReceivedDocument entity = new ReceivedDocument
                    {
                       
                        SecretLevel = createModel.SecretLevel,
                        ResponsibleUserId = createModel.ResponsibleUserId,
                        ReceivedDocumentId = 1,
                        SenderId = createModel.SenderId,
                        SignedByUserId = createModel.SignedByUserId,
                        Summary = createModel.Summary,
                        DocumentDate = createModel.DocumentDate,
                        DocumentIndex = createModel.DocumentIndex,
                        ReceiverUserId = createModel.ReceiverUserId,
                        DocumentStatusId=createModel.DocumentStatusId,
                        ReceiverId = createModel.ReceiverId,
                        Name = createModel.Name,
                        CategoryId = createModel.CategoryId,
                        CreatedByUserId = createModel.CreatedByUserId,
                        CreatedOnDate = DateTime.Now,
                        DeliveryMethodId = createModel.DeliveryMethodId,
                        LastModifiedOnDate = DateTime.Now,
                        DepartmentId = user.DepartmentId,
                        IsDelete = false,
                        LastModifiedByUserId = createModel.CreatedByUserId,
                        ResignedNumber = createModel.ResignedNumber,
                        ResignedOnDate = createModel.ResignedOnDate,
                        AttachedFileUrl=createModel.AttachedFileUrl
                    };
                    var last = unitOfwork.GetRepository<ReceivedDocument>().GetAll().OrderByDescending(d => d.ReceivedDocumentId).FirstOrDefault();
                    if (last != null) entity.ReceivedDocumentId = last.ReceivedDocumentId + 1;
                    unitOfwork.GetRepository<ReceivedDocument>().Add(entity);
                    if (unitOfwork.Save() >= 1)
                    {
                        return GetById(entity.ReceivedDocumentId);
                    }
                    return new Response<ReceivedDocumentModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch(Exception ex)
            {
                return new Response<ReceivedDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<BaseReceivedDocumentModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<ReceivedDocument>().GetById(Id);
                    if (entity != null)
                    {
                        entity.IsDelete = true;
                        unitOfWork.GetRepository<ReceivedDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<BaseReceivedDocumentModel>(1, "", new BaseReceivedDocumentModel());
                        }
                    }
                    return new Response<BaseReceivedDocumentModel>(0, "Tài liệu không tồn tại", null);

                }
            }
            catch (Exception ex)
            {
                return new Response<BaseReceivedDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<ReceivedDocumentDisplayModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<ReceivedDocumentDisplayModel>>(0, "", null);
                    var _list = unitOfWork.GetRepository<ReceivedDocument>().GetMany(d => d.IsDelete == false);
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
                                ReceivedDocumentId = d.ReceivedDocumentId,
                                ResignedNumber = d.ResignedNumber,
                                DepartmentName = u.Name,
                                ResignedOnDate = d.ResignedOnDate,
                                SenderId = d.SenderId,
                                DocumentDate = d.DocumentDate,
                                DocumentIndex = d.DocumentIndex,
                                CreatedOnDate = d.CreatedOnDate,
                                SignedByUserId = d.SignedByUserId,
                                Name = d.Name,
                                DocumentProcessId = d.DocumentProcessId,
                                DocumentStatusId = d.DocumentStatusId
                            }
                        )
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.SignedByUserId,
                        u => u.UserId,
                        (d, u) => new
                        {
                            ReceivedDocumentId = d.ReceivedDocumentId,
                            ReceiverUserFullName = u.FullName,
                            DepartmentName = d.DepartmentName,
                            ResignedNumber = d.ResignedNumber,
                            ResignedOnDate = d.ResignedOnDate,
                            SenderId = d.SenderId,
                            DocumentDate = d.DocumentDate,
                            DocumentIndex = d.DocumentIndex,
                            CreatedOnDate = d.CreatedOnDate,
                            Name=d.Name,
                            DocumentProcessId = d.DocumentProcessId,
                            DocumentStatusId = d.DocumentStatusId
                        }
                        )
                        .Join(unitOfWork.GetRepository<BusinessPartner>().GetAll(),
                            d => d.SenderId,
                            b => b.PartnerId,
                            (d, b) => new ReceivedDocumentDisplayModel
                            {
                                ReceivedDocumentId = d.ReceivedDocumentId,
                                DepartmentName = d.DepartmentName,
                                ReceiverUserFullName = d.ReceiverUserFullName,
                                ResignedNumber = d.ResignedNumber,
                                ResignedOnDate = d.ResignedOnDate,
                                Sender = b.Name,
                                DocumentDate=d.DocumentDate,
                                DocumentIndex=d.DocumentIndex,
                                CreatedOnDate = d.CreatedOnDate,
                                Name = d.Name,
                                DocumentProcessId = d.DocumentProcessId,
                                DocumentStatusId = d.DocumentStatusId
                            }).OrderBy(d => d.ReceivedDocumentId).ToList();

                    return new Response<List<ReceivedDocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<ReceivedDocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<List<BaseReceivedDocumentModel>> GetAllBase()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list=unitOfWork.GetRepository<ReceivedDocument>().GetMany(d => d.IsDelete==false)
                        .Select(d => new BaseReceivedDocumentModel
                        {
                            CreatedOnDate = d.CreatedOnDate,
                            Summary = d.Summary,
                            Name = d.Name,
                            ReceivedDocumentId = d.ReceivedDocumentId
                        }).OrderBy(d => d.ReceivedDocumentId).ToList();
                    return new Response<List<BaseReceivedDocumentModel>>(1, "", list, list.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<BaseReceivedDocumentModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<ReceivedDocumentModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<ReceivedDocument>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<ReceivedDocumentModel>(1, "", new ReceivedDocumentModel {
                            SecretLevel = entity.SecretLevel,
                            ResponsibleUserId = entity.ResponsibleUserId,
                            ReceivedDocumentId = entity.ReceivedDocumentId,
                            SenderId = entity.SenderId,
                            SignedByUserId = entity.SignedByUserId,
                            Summary = entity.Summary,
                            DocumentDate = entity.DocumentDate,
                            DocumentIndex = entity.DocumentIndex,
                            ReceiverUserId = entity.ReceiverUserId,
                            DocumentStatusId = entity.DocumentStatusId,
                            ReceiverId = entity.ReceiverId,
                            Name = entity.Name,
                            CategoryId = entity.CategoryId,
                            CreatedByUserId = entity.CreatedByUserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DeliveryMethodId = entity.DeliveryMethodId,
                            LastModifiedOnDate = entity.LastModifiedOnDate,
                            DepartmentId = entity.DepartmentId,
                            IsDelete = entity.IsDelete,
                            LastModifiedByUserId = entity.LastModifiedByUserId,
                            ResignedNumber = entity.ResignedNumber,
                            ResignedOnDate = entity.ResignedOnDate,
                            AttachedFileUrl = entity.AttachedFileUrl,
                            DocumentProcessId=entity.DocumentProcessId
                        });
                    }
                    return new Response<ReceivedDocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ReceivedDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<ReceivedDocumentModel> Update(int Id, ReceivedDocumentUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<ReceivedDocument>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, updateModel);
                        entity.LastModifiedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<ReceivedDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.ReceivedDocumentId);
                        }
                        return new Response<ReceivedDocumentModel>(0, "Lưu thông tin không thành công", null);
                    }
                    return new Response<ReceivedDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ReceivedDocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<ReceivedDocumentModel> Register(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<ReceivedDocument>().GetById(Id);
                    if (entity != null)
                    {
                        if (entity.DocumentProcessId != null)
                        {
                            DocumentProcessModel current = new DbDocumentProcessHandler().GetCurrentProcess((int)entity.DocumentProcessId).Data;
                            if (current.TaskType == (int)TaskType.REGISTER)
                            {
                                if (entity.ResignedNumber != null) return new Response<ReceivedDocumentModel>(0, "Tài liệu đã được đăng ký rồi", null);
                                entity.ResignedNumber = 1;
                                var last = unitOfWork.GetRepository<ReceivedDocument>().GetMany(d => d.ResignedNumber != null).OrderByDescending(d => d.ResignedNumber).FirstOrDefault();
                                if (last != null) entity.ResignedNumber = last.ResignedNumber + 1;
                                entity.ResignedOnDate = DateTime.Now;
                                unitOfWork.GetRepository<ReceivedDocument>().Update(entity);
                                //
                                var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == entity.DocumentProcessId && d.TaskType == (int)TaskType.REGISTER);
                                tmp.Status = (int)DocumentProcessStatus.FINISHED;
                                tmp.RelatedId = entity.ReceivedDocumentId;
                                unitOfWork.GetRepository<DocumentProcess>().Update(tmp);
                                if (unitOfWork.Save() >= 1)
                                {
                                    return GetById(entity.ReceivedDocumentId);
                                }
                                return new Response<ReceivedDocumentModel>(0, "Lưu thông tin không thành công", null);
                            }
                            return new Response<ReceivedDocumentModel>(0, "Quy trình hiện tại không phải đăng ký", null);
                        }
                        return new Response<ReceivedDocumentModel>(0, "Bạn chưa khởi tạo quy trình xử lý tài liệu này", null);

                    }
                    return new Response<ReceivedDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<ReceivedDocumentModel>(-1, ex.ToString(), null);
            }
        }
    }
}