using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentUnifyHandler : IDbDocumentUnifyHandler
    {
        public Response<DocumentUnifyModel> Create(DocumentUnifyCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
 
                    DocumentUnify entity = new DocumentUnify();
                    Ultis.TransferValues(entity, createModel);

                    entity.FinishedOnDate = createModel.FinishedOnDate;
                    entity.CreatedOnDate = DateTime.Now;
                    entity.UpdatedOnDate = DateTime.Now;
                    var last = unitOfWork.GetRepository<DocumentUnify>().GetAll().OrderByDescending(u => u.UnifyId).FirstOrDefault();
                    if (last != null) entity.UnifyId = last.UnifyId + 1;
                    else entity.UnifyId = 1;
                    entity.RelatedDocumentId = entity.UnifyId;
                    unitOfWork.GetRepository<DocumentUnify>().Add(entity);
                    foreach (var item in createModel.UserList.Split(','))
                    {
                        int user_id = Convert.ToInt32(item.Trim());
                        TaskMessage message = new TaskMessage
                        {
                            CreatedByUserId = createModel.CreatedByUserId,
                            Deadline = createModel.FinishedOnDate,
                            TaskType = (int)TaskType.UNIFY,
                            Title = createModel.Name,
                            UserId = user_id,
                            CreatedOnDate = DateTime.Now,
                            RelatedId = entity.UnifyId,
                            ModuleId = createModel.ModuleId
                        };
                        unitOfWork.GetRepository<TaskMessage>().Add(message);
                    }
                    //

                    var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == createModel.ProcessId && d.TaskType == createModel.TaskType);
                    if (tmp == null)
                    {
                        return new Response<DocumentUnifyModel>(0, "Bạn chưa tạo tiến trình xử lý cho tài liệu này", null);
                    }
                    tmp.Status = (int)DocumentProcessStatus.INPROCESS;
                    tmp.RelatedId = entity.UnifyId;
                    unitOfWork.GetRepository<DocumentProcess>().Update(tmp);

                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.UnifyId);

                    }
                    return new Response<DocumentUnifyModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentUnifyResponseModel> GetResponse(int UserId, int DocumentUnifyId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<QH_DocumentUnify_User>().Get(q => q.UserId == UserId && q.DocumentUnifyId == DocumentUnifyId);
                    if (entity != null)
                    {
                        return new Response<DocumentUnifyResponseModel>(1, "", new DocumentUnifyResponseModel
                        {
                            UserId = entity.UserId,
                            CreatedOnDate = entity.CreatedOnDate,
                            DocumentUnifyId = entity.DocumentUnifyId,
                            Note = entity.Note,
                            ResponseStatus = entity.ResponseStatus
                        });
                    }
                    return new Response<DocumentUnifyResponseModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyResponseModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentUnifyResponseModel> CreateResponse(DocumentUnifyResponseModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var existed = unitOfWork.GetRepository<QH_DocumentUnify_User>().Get(d => d.DocumentUnifyId == createModel.DocumentUnifyId && d.UserId == createModel.UserId);
                    if (existed != null) return new Response<DocumentUnifyResponseModel>(0, "Bạn đã thống nhất rồi!", null);
                    QH_DocumentUnify_User entity = new QH_DocumentUnify_User();
                    Ultis.TransferValues(entity, createModel);
                    unitOfWork.GetRepository<QH_DocumentUnify_User>().Add(entity);
                    //update status
                    //turn off notif
                    var notif = unitOfWork.GetRepository<TaskMessage>().Get(c => c.UserId == createModel.UserId && c.RelatedId == createModel.DocumentUnifyId);
                    if (notif != null)
                    {
                        notif.Status = 1;
                        unitOfWork.GetRepository<TaskMessage>().Update(notif);
                    }

                    if (unitOfWork.Save() >= 1)
                    {
                        return GetResponse(createModel.UserId, createModel.DocumentUnifyId);
                    }
                    return new Response<DocumentUnifyResponseModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyResponseModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentUnifyModel> Delete(int Id)
        {
            throw new NotImplementedException();
        }
        public Response<List<DocumentUnifyModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<DocumentUnify>().GetAll()
                        .Select(u => new DocumentUnifyModel
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
                            UnifyId = u.UnifyId,
                            UpdatedByUserId = u.UpdatedByUserId,
                            UpdatedOnDate = u.UpdatedOnDate,
                            UserList = u.UserList,
                            ModuleId = u.ModuleId
                        }).ToList();
                    return new Response<List<DocumentUnifyModel>>(1, "", ds);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentUnifyModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentUnifyResponseDisplayModel>> GetAllResponses(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<QH_DocumentUnify_User>().GetMany(t => t.DocumentUnifyId == Id)
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        qh => qh.UserId,
                        u => u.UserId,
                        (qh, u) => new DocumentUnifyResponseDisplayModel
                        {
                            CreatedOnDate = qh.CreatedOnDate,
                            DocumentUnifyId = qh.DocumentUnifyId,
                            Note = qh.Note,
                            ResponseStatus = qh.ResponseStatus,
                            UserId = qh.UserId,
                            UserFullName = u.FullName,
                            UserName = u.UserName
                        })
                        .OrderBy(c => c.UserId)
                        .ToList();

                    return new Response<List<DocumentUnifyResponseDisplayModel>>(1, "", ds, ds.Count);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentUnifyResponseDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentUnifyModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentUnify>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<DocumentUnifyModel>(1, "", Ultis.ConvertSameData<DocumentUnifyModel>(entity));
                    }

                    return new Response<DocumentUnifyModel>(0, "Id is not valid!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentUnifyModel> Update(int Id, DocumentUnifyUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentUnify>().GetById(Id);
                    if (entity != null)
                    {
                        Ultis.TransferValues(entity, updateModel);
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentUnify>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.UnifyId);

                        }
                        return new Response<DocumentUnifyModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentUnifyModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentUnifyModel> ReCreate(int UnifyId, RecreateDocumentUnifyModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    DocumentUnify entity = unitOfWork.GetRepository<DocumentUnify>().GetById(UnifyId);
                    if (entity == null) return new Response<DocumentUnifyModel>(0, "Id is not valid", null);
                    if (entity.IsFinished == 1) return new Response<DocumentUnifyModel>(0, "thống nhất đã kết thúc rồi!", null);
                    if (entity.CreatedByUserId != createModel.UserId) return new Response<DocumentUnifyModel>(0, "Bạn không phải là tác giả", null);
                    //change message
                    entity.Status = 2;
                    entity.Description = createModel.Message;
                    entity.FinishedOnDate = DateTime.Now.AddDays(createModel.ExtraDays);
                    entity.UpdatedOnDate = DateTime.Now;
                    unitOfWork.GetRepository<DocumentUnify>().Update(entity);
                    //delete responses
                    foreach (var item in unitOfWork.GetRepository<QH_DocumentUnify_User>().GetMany(d => d.DocumentUnifyId == UnifyId))
                    {
                        unitOfWork.GetRepository<QH_DocumentUnify_User>().Delete(item);
                    }
                    //update message
                    var exsisted_mess = unitOfWork.GetRepository<TaskMessage>().GetMany(t => t.ModuleId == createModel.ModuleId && t.TaskType == (int)TaskType.UNIFY && t.RelatedId == UnifyId);
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
                            TaskType = (int)TaskType.UNIFY,
                            Title = "Thống nhất lại: " + entity.Name,
                            UserId = user_id,
                            CreatedOnDate = DateTime.Now,
                            RelatedId = entity.UnifyId,
                            ModuleId = entity.ModuleId
                        };
                        unitOfWork.GetRepository<TaskMessage>().Add(message);
                    }
                    //
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.UnifyId);

                    }
                    return new Response<DocumentUnifyModel>(0, "Saving data not successful!", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentUnifyModel> Finish(int Id, FinishUnifyModel model)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentUnify>().GetById(Id);

                    if (entity != null)
                    {
                        if (entity.CreatedByUserId != model.UserId) return new Response<DocumentUnifyModel>(0, "Bạn không phải là tác giả", null);
                        entity.Status = model.Status;
                        entity.IsFinished = 1;
                        entity.UpdatedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<DocumentUnify>().Update(entity);
                        //
                        var tmp = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == model.ProcessId && d.TaskType == (int)TaskType.UNIFY);
                        if (tmp == null) return new Response<DocumentUnifyModel>(0, "Bạn chưa khởi tạo tiến trình xử lý cho tài liệu này", null);
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
                            return GetById(entity.UnifyId);

                        }
                        return new Response<DocumentUnifyModel>(0, "Saving data not successful!", null);
                    }
                    return new Response<DocumentUnifyModel>(0, "Id is not valid", null);


                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentUnifyModel>(-1, ex.ToString(), null);
            }
        }
    }
}