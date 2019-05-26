using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbInternalDocumentHandler : IDbInternalDocumentHandler
    {
        public Response<InternalDocumentModel> Create(InternalDocumentCreateModel createModel)
        {
            try
            {
                using (var unitOfwork = new UnitOfWork())
                {
                    var user = unitOfwork.GetRepository<User>().GetById(createModel.CreatedByUserId);
                    if (user == null) return new Response<InternalDocumentModel>(0, "", null);

                    InternalDocument entity = new InternalDocument
                    {
                        SecretLevel = createModel.SecretLevel,
                        ResponsibleUserId = createModel.ResponsibleUserId,
                        Summary = createModel.Summary,
                        WrittenByUserId = createModel.WrittenByUserId,
                        Name = createModel.Name,
                        CategoryId = createModel.CategoryId,
                        CreatedByUserId = createModel.CreatedByUserId,
                        CreatedOnDate = DateTime.Now,
                        LastModifiedOnDate = DateTime.Now,
                        DepartmentId = user.DepartmentId,
                        IsDelete = false,
                        LastModifiedByUserId = createModel.CreatedByUserId,
                        DocumentStatusId=createModel.DocumentStatusId,
                        DirectoryId=createModel.DirectoryId,
                        InternalDocumentId=1,
                        ProjectId=createModel.ProjectId,
                        AttachedFileUrl=createModel.AttachedFileUrl
                    };
                    var last = unitOfwork.GetRepository<InternalDocument>().GetAll().OrderByDescending(d => d.InternalDocumentId).FirstOrDefault();
                    if (last != null) entity.InternalDocumentId = last.InternalDocumentId + 1;
                    unitOfwork.GetRepository<InternalDocument>().Add(entity);
                    if (unitOfwork.Save() >= 1)
                    {
                        return GetById(entity.InternalDocumentId);
                    }
                    return new Response<InternalDocumentModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<InternalDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<InternalDocumentModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<InternalDocument>().GetById(Id);
                    if (entity != null)
                    {
                        entity.IsDelete = true;
                        unitOfWork.GetRepository<InternalDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<InternalDocumentModel>(1, "", new InternalDocumentModel());
                        }
                    }
                    return new Response<InternalDocumentModel>(0, "Tài liệu không tồn tại", null);

                }
            }
            catch (Exception ex)
            {
                return new Response<InternalDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<InternalDocumentDisplayModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<InternalDocumentDisplayModel>>(0, "", null);
                    var _list = unitOfWork.GetRepository<InternalDocument>().GetMany(d => d.IsDelete == false && d.DepartmentId == user.DepartmentId);
             
                    if (user.UserRoleId > 1)
                    {
                        _list=_list.Where(d =>  d.CreatedByUserId == UserId || d.ResignedNumber != null);
                    }
              
                    var list = _list
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.WrittenByUserId,
                        u => u.UserId,
                        (d, u) => new InternalDocumentDisplayModel
                        {
                            InternalDocumentId = d.InternalDocumentId,
                            WrittenByUserFullName = u.FullName,
                            ResignedNumber = d.ResignedNumber,
                            ResignedOnDate = d.ResignedOnDate,
                            CreatedOnDate = d.CreatedOnDate,
                            Name = d.Name,
                            DocumentProcessId=d.DocumentProcessId,
                            DocumentStatusId=d.DocumentStatusId
                        }).OrderBy(d => d.InternalDocumentId).ToList();


                    return new Response<List<InternalDocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<InternalDocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<List<InternalDocumentDisplayModel>> GetAllInDirectory(int UserId, int DirectoryId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<InternalDocumentDisplayModel>>(0, "", null);
                    var _list = unitOfWork.GetRepository<InternalDocument>().GetMany(d => d.IsDelete == false && d.DirectoryId == DirectoryId);
                    if (user.UserRoleId > 1)
                    {
                        _list = _list.Where(d => d.DepartmentId == user.DepartmentId && (d.CreatedByUserId == UserId || d.ResignedNumber != null));
                    }

                    var list = _list
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.WrittenByUserId,
                        u => u.UserId,
                        (d, u) => new InternalDocumentDisplayModel
                        {
                            InternalDocumentId = d.InternalDocumentId,
                            WrittenByUserFullName = u.FullName,
                            ResignedNumber = d.ResignedNumber,
                            ResignedOnDate = d.ResignedOnDate,
                            CreatedOnDate = d.CreatedOnDate,
                            Name = d.Name,
                            DocumentProcessId = d.DocumentProcessId,
                            DocumentStatusId = d.DocumentStatusId
                        }).OrderBy(d => d.InternalDocumentId).ToList();


                    return new Response<List<InternalDocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<InternalDocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<InternalDocumentModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<InternalDocument>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<InternalDocumentModel>(1, "", new InternalDocumentModel
                        {
                            SecretLevel = entity.SecretLevel,
                            ResponsibleUserId = entity.ResponsibleUserId,
                            Summary = entity.Summary,
                            WrittenByUserId = entity.WrittenByUserId,
                            Name = entity.Name,
                            CategoryId = entity.CategoryId,
                            CreatedByUserId = entity.CreatedByUserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            LastModifiedOnDate = entity.LastModifiedOnDate,
                            DepartmentId = entity.DepartmentId,
                            IsDelete = entity.IsDelete,
                            LastModifiedByUserId = entity.LastModifiedByUserId,
                            DocumentStatusId = entity.DocumentStatusId,
                            DirectoryId = entity.DirectoryId,
                            InternalDocumentId = entity.InternalDocumentId,
                            ProjectId = entity.ProjectId,
                            AttachedFileUrl=entity.AttachedFileUrl,
                            DocumentProcessId=entity.DocumentProcessId,
                            ResignedNumber=entity.ResignedNumber,
                            ResignedOnDate=entity.ResignedOnDate
                            
                        });
                    }
                    return new Response<InternalDocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<InternalDocumentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<InternalDocumentModel> Update(int Id, InternalDocumentUpdateModel userModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<InternalDocument>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, userModel);
                        unitOfWork.GetRepository<InternalDocument>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.InternalDocumentId);
                        }
                        return new Response<InternalDocumentModel>(0, "Lưu thông tin không thành công", null);
                    }
                    return new Response<InternalDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<InternalDocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<InternalDocumentModel> Register(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<InternalDocument>().GetById(Id);
                    if (entity != null)
                    {
                        if (entity.DocumentProcessId != null)
                        {
                            DocumentProcessModel current = new DbDocumentProcessHandler().GetCurrentProcess((int)entity.DocumentProcessId).Data;
                            if (current.TaskType == (int)TaskType.REGISTER)
                            {
                                if (entity.ResignedNumber != null) return new Response<InternalDocumentModel>(0, "Tài liệu đã được đăng ký rồi", null);
                                entity.ResignedNumber = 1;
                                var last = unitOfWork.GetRepository<InternalDocument>().GetMany(d => d.ResignedNumber != null).OrderByDescending(d => d.ResignedNumber).FirstOrDefault();
                                if (last != null) entity.ResignedNumber = last.ResignedNumber + 1;
                                entity.ResignedOnDate = DateTime.Now;
                                unitOfWork.GetRepository<InternalDocument>().Update(entity);
                                //
                                var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == entity.DocumentProcessId && d.TaskType == (int)TaskType.REGISTER);
                                tmp.Status = (int)DocumentProcessStatus.FINISHED;
                                tmp.RelatedId = entity.InternalDocumentId;
                                unitOfWork.GetRepository<DocumentProcess>().Update(tmp);
                                if (unitOfWork.Save() >= 1)
                                {
                                    return GetById(entity.InternalDocumentId);
                                }
                                return new Response<InternalDocumentModel>(0, "Lưu thông tin không thành công", null);
                            }
                            return new Response<InternalDocumentModel>(0, "Quy trình hiện tại không phải đăng ký", null);
                        }
                        return new Response<InternalDocumentModel>(0, "Bạn chưa khởi tạo quy trình xử lý tài liệu này", null);

                    }
                    return new Response<InternalDocumentModel>(0, "Tài liệu không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<InternalDocumentModel>(-1, ex.ToString(), null);
            }
        }
    }
}