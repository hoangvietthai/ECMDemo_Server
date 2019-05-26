using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentStatusHandler : IDbDocumentStatusHandler
    {
     
        public Response<DocumentStatusModel> Create(DocumentStatusCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    DocumentStatus entity = new DocumentStatus
                    {
                        RegisterStatus = createModel.RegisterStatus,
                        ConfirmStatus = createModel.ConfirmStatus,
                        PerformStatus = createModel.PerformStatus,
                        DisplayName = createModel.DisplayName,
                        Id = 1,
                        UnifyStatus = createModel.UnifyStatus
                    };
                    var last = unitOfWork.GetRepository<DocumentStatus>().GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
                    if (last != null) entity.Id = last.Id + 1;
                    unitOfWork.GetRepository<DocumentStatus>().Add(entity);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.Id);
                    }
                    return new Response<DocumentStatusModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentStatusModel>(-1, ex.ToString(), null);
            }
        }

        public Response<BaseDocumentStatusModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentStatus>().GetById(Id);
                    if (entity != null)
                    {

                        unitOfWork.GetRepository<DocumentStatus>().Delete(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<BaseDocumentStatusModel>(1, "", null);
                        }
                        return new Response<BaseDocumentStatusModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<BaseDocumentStatusModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BaseDocumentStatusModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<BaseDocumentStatusModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Response<DocumentStatusModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<DocumentStatus>().GetById(Id);
                    if (doc != null) return new Response<DocumentStatusModel>(1, "", Ultis.ConvertSameData<DocumentStatusModel>(doc));
                    else
                        return new Response<DocumentStatusModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentStatusModel>(-1, ex.ToString(), null);
            }
        }

        public Response<BaseDocumentStatusModel> GetNameById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<DocumentStatus>().GetById(Id);
                    if (doc != null)
                    {
                        return new Response<BaseDocumentStatusModel>(1, "", new BaseDocumentStatusModel
                        {
                            Id = doc.Id,
                            DisplayName = doc.DisplayName
                        });
                    }
                    else
                        return new Response<BaseDocumentStatusModel>(0, "Id không tồn tại", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<BaseDocumentStatusModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentStatusModel> Update(int Id, DocumentStatusUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentStatus>().GetById(Id);
                    if (entity != null)
                    {
                        entity.RegisterStatus = updateModel.RegisterStatus;
                        entity.UnifyStatus = updateModel.UnifyStatus;
                        entity.ConfirmStatus = updateModel.ConfirmStatus;
                        entity.PerformStatus = updateModel.PerformStatus;
                        entity.DisplayName = updateModel.DisplayName;
                        entity.ConfirmRelatedId = updateModel.ConfirmRelatedId != null ? updateModel.ConfirmRelatedId : entity.ConfirmRelatedId;
                        entity.PerformRelatedId = updateModel.PerformRelatedId != null ? updateModel.PerformRelatedId : entity.PerformRelatedId;
                        entity.UnifyRelatedId = updateModel.UnifyRelatedId != null ? updateModel.UnifyRelatedId : entity.UnifyRelatedId;
                        entity.RegisterRelatedId = updateModel.RegisterRelatedId != null ? updateModel.RegisterRelatedId : entity.RegisterRelatedId;
                        unitOfWork.GetRepository<DocumentStatus>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.Id);
                        }
                        return new Response<DocumentStatusModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DocumentStatusModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentStatusModel>(-1, ex.ToString(), null);
            }
        }

       
    }
}