using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentConfirmHandler : IDbDocumentConfirmHandler
    {
        public Response<DocumentConfirmModel> Create(DocumentConfirmCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                   
                    DocumentConfirm entity = new DocumentConfirm();
                    Ultis.TransferValues(entity, createModel);
                    entity.CreatedOnDate = DateTime.Now;
                    entity.UpdatedOnDate = DateTime.Now;
                    var last = unitOfWork.GetRepository<DocumentConfirm>().GetAll().OrderByDescending(u => u.ConfirmId).FirstOrDefault();
                    if (last != null) entity.ConfirmId = last.ConfirmId + 1;
                    else entity.ConfirmId = 1;
                    unitOfWork.GetRepository<DocumentConfirm>().Add(entity);
                    TaskMessage message = new TaskMessage
                    {
                        CreatedByUserId = createModel.CreatedByUserId,
                        Deadline = createModel.FinishedOnDate,
                        TaskType = (int)TaskType.CONFIRM,
                        Title = createModel.Name,
                        UserId = createModel.UserId,
                        CreatedOnDate = DateTime.Now,
                        RelatedId = entity.ConfirmId,
                        ModuleId = createModel.ModuleId
                    };
                    unitOfWork.GetRepository<TaskMessage>().Add(message);
                    //
                    var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == createModel.ProcessId && d.TaskType == (int)TaskType.CONFIRM);
                    if (tmp == null)
                    {
                        return new Response<DocumentConfirmModel>(0, "Bạn chưa tạo tiến trình xử lý cho tài liệu này", null);
                    }
                    tmp.Status = (int)DocumentProcessStatus.INPROCESS;
                    tmp.RelatedId = entity.ConfirmId;
                    unitOfWork.GetRepository<DocumentProcess>().Update(tmp);
                    //
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.ConfirmId);

                    }
                    return new Response<DocumentConfirmModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmResponseDisplayModel> GetResponse(int UserId, int DocumentConfirmId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<DocumentConfirmResponseDisplayModel>(0, "User does not exsist!", null);
                    var entity = unitOfWork.GetRepository<QH_DocumentConfirm_User>().Get(q => q.UserId == UserId && q.DocumentConfirmId == DocumentConfirmId);
                    if (entity != null)
                    {
                        return new Response<DocumentConfirmResponseDisplayModel>(1, "", new DocumentConfirmResponseDisplayModel
                        {
                            UserId = entity.UserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DocumentConfirmId = entity.DocumentConfirmId,
                            Note = entity.Note,
                            ResponseStatus = entity.ResponseStatus,
                            UserFullName = user.FullName,
                            UserName = user.UserName

                        });
                    }
                    return new Response<DocumentConfirmResponseDisplayModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmResponseDisplayModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmResponseDisplayModel> GetResponse(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {

                    var entity = unitOfWork.GetRepository<QH_DocumentConfirm_User>().Get(q => q.DocumentConfirmId == Id);
                    if (entity != null)
                    {
                        var user = unitOfWork.GetRepository<User>().GetById(entity.UserId);
                        if (user == null) return new Response<DocumentConfirmResponseDisplayModel>(0, "User does not exsist!", null);
                        return new Response<DocumentConfirmResponseDisplayModel>(1, "", new DocumentConfirmResponseDisplayModel
                        {
                            UserId = entity.UserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DocumentConfirmId = entity.DocumentConfirmId,
                            Note = entity.Note,
                            ResponseStatus = entity.ResponseStatus,
                            UserFullName = user.FullName,
                            UserName = user.UserName

                        });
                    }
                    return new Response<DocumentConfirmResponseDisplayModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmResponseDisplayModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmResponseModel> CreateResponse(DocumentConfirmResponseModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var existed = unitOfWork.GetRepository<QH_DocumentConfirm_User>().Get(d => d.DocumentConfirmId == createModel.DocumentConfirmId && d.UserId == createModel.UserId);
                    if (existed != null) return new Response<DocumentConfirmResponseModel>(0, "Bạn đã xác nhận rồi!", null);
                    QH_DocumentConfirm_User entity = new QH_DocumentConfirm_User();
                    Ultis.TransferValues(entity, createModel);
                    unitOfWork.GetRepository<QH_DocumentConfirm_User>().Add(entity);
                    var confirm = unitOfWork.GetRepository<DocumentConfirm>().GetById(createModel.DocumentConfirmId);
                    if (confirm != null)
                    {
                        TaskMessage taskMessage = new TaskMessage
                        {
                            Deadline = DateTime.Now.AddDays(3),
                            CreatedOnDate = DateTime.Now,
                            CreatedByUserId = createModel.UserId,
                            UserId = confirm.CreatedByUserId,
                            IsMyTask = true,
                            ModuleId = confirm.ModuleId,
                            RelatedId = confirm.ConfirmId,
                            Status = 0,
                            TaskType = (int)TaskType.CONFIRM,
                            Title = "Tham khảo phê duyệt " + confirm.Name
                        };
                        unitOfWork.GetRepository<TaskMessage>().Add(taskMessage);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<DocumentConfirmResponseModel>(1, "", Ultis.ConvertSameData<DocumentConfirmResponseModel>(entity));
                        }
                        return new Response<DocumentConfirmResponseModel>(0, "", null);
                    }
                    return new Response<DocumentConfirmResponseModel>(0, "", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmResponseModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmModel> Delete(int Id)
        {
            throw new NotImplementedException();
        }
        public Response<List<DocumentConfirmModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<DocumentConfirm>().GetAll()
                        .Select(u => new DocumentConfirmModel
                        {
                            CreatedByUserId = u.CreatedByUserId,
                            CreatedOnDate = u.CreatedOnDate,
                            Description = u.Description,
                            FinishedOnDate = u.FinishedOnDate,
                            Name = u.Name,
                            PriorityLevel = u.PriorityLevel,
                            RelatedDocumentId = u.RelatedDocumentId,
                            ConfirmId = u.ConfirmId,
                            UpdatedByUserId = u.UpdatedByUserId,
                            UpdatedOnDate = u.UpdatedOnDate,
                            UserId = u.UserId,
                            ModuleId = u.ModuleId
                        }).ToList();
                    return new Response<List<DocumentConfirmModel>>(1, "", ds);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentConfirmModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentConfirmModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentConfirm>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<DocumentConfirmModel>(1, "", Ultis.ConvertSameData<DocumentConfirmModel>(entity));
                    }

                    return new Response<DocumentConfirmModel>(0, "Id is not valid!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmModel> Update(int Id, DocumentConfirmUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentConfirm>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, updateModel);
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentConfirm>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.ConfirmId);

                        }
                        return new Response<DocumentConfirmModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentConfirmModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentConfirmModel> ReCreate(int ConfirmId, RecreateDocumentConfirmModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    DocumentConfirm entity = unitOfWork.GetRepository<DocumentConfirm>().GetById(ConfirmId);
                    if (entity == null) return new Response<DocumentConfirmModel>(0, "Id is not valid", null);
                    if (entity.CreatedByUserId != createModel.UserId) return new Response<DocumentConfirmModel>(0, "Bạn không phải là tác giả", null);
                    //change message
                    entity.Description = createModel.Message;
                    entity.FinishedOnDate = DateTime.Now.AddDays(createModel.ExtraDays);
                    entity.UpdatedOnDate = DateTime.Now;
                    unitOfWork.GetRepository<DocumentConfirm>().Update(entity);
                    //delete responses
                    foreach (var item in unitOfWork.GetRepository<QH_DocumentConfirm_User>().GetMany(d => d.DocumentConfirmId == ConfirmId))
                    {
                        unitOfWork.GetRepository<QH_DocumentConfirm_User>().Delete(item);
                    }
                    //update message
                    var exsisted_mess = unitOfWork.GetRepository<TaskMessage>().GetMany(t => t.ModuleId == createModel.ModuleId && t.TaskType == (int)TaskType.CONFIRM && t.RelatedId == ConfirmId);
                    foreach (var item in exsisted_mess)
                    {
                        unitOfWork.GetRepository<TaskMessage>().Delete(item);
                    }
                    //
                    TaskMessage message = new TaskMessage
                    {
                        CreatedByUserId = entity.CreatedByUserId,
                        Deadline = entity.FinishedOnDate,
                        TaskType = (int)TaskType.CONFIRM,
                        Title = "Phê duyệt lại: " + entity.Name,
                        UserId = entity.UserId,
                        CreatedOnDate = DateTime.Now,
                        RelatedId = entity.ConfirmId,
                        ModuleId = entity.ModuleId,
                        Status = 0,
                        IsMyTask = false

                    };
                    unitOfWork.GetRepository<TaskMessage>().Add(message);
                    //
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.ConfirmId);

                    }
                    return new Response<DocumentConfirmModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentConfirmModel> Finish(int Id, FinishConfirmModel model)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentConfirm>().GetById(Id);

                    if (entity != null)
                    {
                        if (entity.CreatedByUserId != model.UserId) return new Response<DocumentConfirmModel>(0, "Bạn không phải là tác giả", null);
                        entity.IsFinished = 1;
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentConfirm>().Update(entity);
                        //
                        var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == model.ProcessId && d.TaskType == (int)TaskType.CONFIRM);
                        if (tmp == null) return new Response<DocumentConfirmModel>(0, "Bạn chưa khởi tạo tiến trình xử lý cho tài liệu này", null);
                        tmp.Status = (int)DocumentProcessStatus.FINISHED;
                        unitOfWork.GetRepository<DocumentProcess>().Update(tmp);
                        
                        //
                        //var next = unitOfWork.GetRepository<DocumentProcess>().GetMany(d => d.Id == model.ProcessId && d.Status < 1).OrderBy(d => d.OrderIndex).FirstOrDefault();
                        //if (next != null)
                        //{
                        //    next.Status = (int)DocumentProcessStatus.INPROCESS;
                        //    unitOfWork.GetRepository<DocumentProcess>().Update(next);
                        //}
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.ConfirmId);

                        }
                        return new Response<DocumentConfirmModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentConfirmModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentConfirmModel>(-1, ex.ToString(), null);
            }
        }
    }
}