using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentPerformHandler : IDbDocumentPerformHandler
    {
        public Response<DocumentPerformModel> Create(DocumentPerformCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
 
                    DocumentPerform entity = new DocumentPerform();
                    Ultis.TransferValues(entity, createModel);

                    entity.FinishedOnDate = createModel.FinishedOnDate;
                    entity.CreatedOnDate = DateTime.Now;
                    entity.UpdatedOnDate = DateTime.Now;
                    var last = unitOfWork.GetRepository<DocumentPerform>().GetAll().OrderByDescending(u => u.PerformId).FirstOrDefault();
                    if (last != null) entity.PerformId = last.PerformId + 1;
                    else entity.PerformId = 1;
                    unitOfWork.GetRepository<DocumentPerform>().Add(entity);
                    foreach (var item in createModel.UserList.Split(','))
                    {
                        int user_id = Convert.ToInt32(item.Trim());
                        TaskMessage message = new TaskMessage
                        {
                            CreatedByUserId = createModel.CreatedByUserId,
                            Deadline = createModel.FinishedOnDate,
                            TaskType = (int)TaskType.PERFORM,
                            Title = createModel.Name,
                            UserId = user_id,
                            CreatedOnDate = DateTime.Now,
                            RelatedId = entity.PerformId,
                            ModuleId = createModel.ModuleId
                        };
                        unitOfWork.GetRepository<TaskMessage>().Add(message);
                    }
                    //

                    var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == createModel.ProcessId && d.TaskType == createModel.TaskType);
                    if (tmp == null)
                    {
                        return new Response<DocumentPerformModel>(0, "Bạn chưa tạo tiến trình xử lý cho tài liệu này", null);
                    }
                    tmp.Status = (int)DocumentProcessStatus.INPROCESS;
                    tmp.RelatedId = entity.PerformId;
                    unitOfWork.GetRepository<DocumentProcess>().Update(tmp);

                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.PerformId);

                    }
                    return new Response<DocumentPerformModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentPerformResponseModel> GetResponse(int UserId, int DocumentPerformId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<QH_DocumentPerform_User>().Get(q => q.UserId == UserId && q.DocumentPerformId == DocumentPerformId);
                    if (entity != null)
                    {
                        return new Response<DocumentPerformResponseModel>(1, "", new DocumentPerformResponseModel
                        {
                            UserId = entity.UserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DocumentPerformId = entity.DocumentPerformId,
                            Note = entity.Note,
                            ResponseStatus = entity.ResponseStatus
                        });
                    }
                    return new Response<DocumentPerformResponseModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformResponseModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentPerformResponseModel> CreateResponse(DocumentPerformResponseModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var existed = unitOfWork.GetRepository<QH_DocumentPerform_User>().Get(d => d.DocumentPerformId == createModel.DocumentPerformId && d.UserId == createModel.UserId);
                    if (existed != null) return new Response<DocumentPerformResponseModel>(0, "Bạn đã thống nhất rồi!", null);
                    QH_DocumentPerform_User entity = new QH_DocumentPerform_User();
                    Ultis.TransferValues(entity, createModel);
                    unitOfWork.GetRepository<QH_DocumentPerform_User>().Add(entity);
                    //update status
                    //turn off notif
                    var notif = unitOfWork.GetRepository<TaskMessage>().Get(c => c.UserId == createModel.UserId && c.RelatedId == createModel.DocumentPerformId);
                    if (notif != null)
                    {
                        notif.Status = 1;
                        unitOfWork.GetRepository<TaskMessage>().Update(notif);
                    }

                    if (unitOfWork.Save() >= 1)
                    {
                        return GetResponse(createModel.UserId, createModel.DocumentPerformId);
                    }
                    return new Response<DocumentPerformResponseModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformResponseModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentPerformModel> Delete(int Id)
        {
            throw new NotImplementedException();
        }
        public Response<List<DocumentPerformModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<DocumentPerform>().GetAll()
                        .Select(u => new DocumentPerformModel
                        {
                            CreatedByUserId = u.CreatedByUserId,
                            Status = u.Status,
                            CreatedOnDate = u.CreatedOnDate,
                            Description = u.Description,
                            FinishedOnDate = u.FinishedOnDate,
                            Name = u.Name,
                            PriorityLevel = u.PriorityLevel,
                            RelatedDocumentId = u.RelatedDocumentId,
                            TaskType = u.TaskType,
                            PerformId = u.PerformId,
                            UpdatedByUserId = u.UpdatedByUserId,
                            UpdatedOnDate = u.UpdatedOnDate,
                            UserList = u.UserList,
                            ModuleId = u.ModuleId
                        }).ToList();
                    return new Response<List<DocumentPerformModel>>(1, "", ds);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentPerformModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentPerformResponseDisplayModel>> GetAllResponses(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<QH_DocumentPerform_User>().GetMany(t => t.DocumentPerformId == Id)
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        qh => qh.UserId,
                        u => u.UserId,
                        (qh, u) => new DocumentPerformResponseDisplayModel
                        {
                            CreatedOnDate = qh.CreatedOnDate,
                            DocumentPerformId = qh.DocumentPerformId,
                            Note = qh.Note,
                            ResponseStatus = qh.ResponseStatus,
                            UserId = qh.UserId,
                            UserFullName = u.FullName,
                            UserName = u.UserName
                        })
                        .OrderBy(c => c.UserId)
                        .ToList();

                    return new Response<List<DocumentPerformResponseDisplayModel>>(1, "", ds, ds.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentPerformResponseDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentPerformModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentPerform>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<DocumentPerformModel>(1, "", Ultis.ConvertSameData<DocumentPerformModel>(entity));
                    }

                    return new Response<DocumentPerformModel>(0, "Id is not valid!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentPerformModel> Update(int Id, DocumentPerformUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentPerform>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, updateModel);
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentPerform>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.PerformId);

                        }
                        return new Response<DocumentPerformModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentPerformModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentPerformModel> ReCreate(int PerformId, RecreateDocumentPerformModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    DocumentPerform entity = unitOfWork.GetRepository<DocumentPerform>().GetById(PerformId);
                    if (entity == null) return new Response<DocumentPerformModel>(0, "Id is not valid", null);
                    if (entity.IsFinished == 1) return new Response<DocumentPerformModel>(0, "thống nhất đã kết thúc rồi!", null);
                    if (entity.CreatedByUserId != createModel.UserId) return new Response<DocumentPerformModel>(0, "Bạn không phải là tác giả", null);
                    //change message
                    entity.Status = 2;
                    entity.Description = createModel.Message;
                    entity.FinishedOnDate = DateTime.Now.AddDays(createModel.ExtraDays);
                    entity.UpdatedOnDate = DateTime.Now;
                    unitOfWork.GetRepository<DocumentPerform>().Update(entity);
                    //delete responses
                    foreach (var item in unitOfWork.GetRepository<QH_DocumentPerform_User>().GetMany(d => d.DocumentPerformId == PerformId))
                    {
                        unitOfWork.GetRepository<QH_DocumentPerform_User>().Delete(item);
                    }
                    //update message
                    var exsisted_mess = unitOfWork.GetRepository<TaskMessage>().GetMany(t => t.ModuleId == createModel.ModuleId && t.TaskType == (int)TaskType.PERFORM && t.RelatedId == PerformId);
                    foreach (var item in exsisted_mess)
                    {
                        unitOfWork.GetRepository<TaskMessage>().Delete(item);
                    }
                    //
                    foreach (var item in entity.UserList.Split(','))
                    {
                        int user_id = Convert.ToInt32(item.Trim());
                        TaskMessage message = new TaskMessage
                        {
                            CreatedByUserId = entity.CreatedByUserId,
                            Deadline = entity.FinishedOnDate,
                            TaskType = (int)TaskType.PERFORM,
                            Title = "Thống nhất lại: " + entity.Name,
                            UserId = user_id,
                            CreatedOnDate = DateTime.Now,
                            RelatedId = entity.PerformId,
                            ModuleId = entity.ModuleId
                        };
                        unitOfWork.GetRepository<TaskMessage>().Add(message);
                    }
                    //
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.PerformId);

                    }
                    return new Response<DocumentPerformModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentPerformModel> Finish(int Id, FinishPerformModel model)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentPerform>().GetById(Id);

                    if (entity != null)
                    {
                        if (entity.CreatedByUserId != model.UserId) return new Response<DocumentPerformModel>(0, "Bạn không phải là tác giả", null);
                        entity.Status = model.Status;
                        entity.IsFinished = 1;
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentPerform>().Update(entity);
                        //
                        var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == model.ProcessId && d.TaskType == (int)TaskType.PERFORM);
                        if (tmp == null) return new Response<DocumentPerformModel>(0, "Bạn chưa khởi tạo tiến trình xử lý cho tài liệu này", null);
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
                            return GetById(entity.PerformId);

                        }
                        return new Response<DocumentPerformModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentPerformModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentPerformModel>(-1, ex.ToString(), null);
            }
        }
    }
}