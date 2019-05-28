using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbTaskMessageHandler : IDbTaskMessageHandler
    {
        public Response<TaskMessageModel> Create(TaskMessageCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    TaskMessage entity = new TaskMessage
                    {
                        CreatedOnDate = DateTime.Now,
                        CreatedByUserId = createModel.CreatedByUserId,
                        Status = 0,
                        Deadline = createModel.Deadline,
                        ModuleId = createModel.ModuleId,
                        RelatedId = createModel.RelatedId,
                        TaskType = createModel.TaskType,
                        Title = createModel.Title,
                        UserId = createModel.UserId,
                        IsMyTask = createModel.IsMyTask
                    };
                    unitOfWork.GetRepository<TaskMessage>().Add(entity);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(entity.MessageId);
                    }
                    return new Response<TaskMessageModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<TaskMessageModel>(-1, ex.ToString(), null);
            }
        }

        public Response<TaskMessageModel> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Response<List<TaskMessageDisplayModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Response<List<TaskMessageDisplayModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<TaskMessage>().GetMany(t => t.UserId == UserId)
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                            t => t.CreatedByUserId,
                            u => u.UserId,
                            (t, u) => new TaskMessageDisplayModel
                            {
                                Author = u.UserName,
                                CreatedByUserId = t.CreatedByUserId,
                                Status = t.Status,
                                CreatedOnDate = t.CreatedOnDate,
                                Deadline = t.Deadline,
                                TaskType = t.TaskType,
                                Title = t.Title,
                                MessageId = t.MessageId,
                                RelatedId = t.RelatedId,
                                ModuleId = t.ModuleId,
                                IsMyTask = t.IsMyTask
                            }).OrderBy(t => t.Deadline).OrderBy(t => t.Status).ToList();

                    return new Response<List<TaskMessageDisplayModel>>(1, "", ds);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<TaskMessageDisplayModel>>(-1, ex.ToString(), null);

            }
        }

        public Response<TaskMessageModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<TaskMessage>().GetById(Id);
                    if (entity != null)
                    {
                        return new Response<TaskMessageModel>(1, "", Ultis.ConvertSameData<TaskMessageModel>(entity));
                    }
                    return new Response<TaskMessageModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<TaskMessageModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<TaskMessageModel>> SendToListUser(string list)
        {
            throw new NotImplementedException();
        }

        public Response<List<TaskMessageModel>> SendToListUser(string list, TaskMessageCreateModel createModel)
        {
            throw new NotImplementedException();
        }

        public Response<TaskMessageModel> Update(int Id, int Status)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<TaskMessage>().GetById(Id);
                    if (entity != null)
                    {
                        entity.Status = Status;
                        unitOfWork.GetRepository<TaskMessage>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(entity.MessageId);
                        }
                        return new Response<TaskMessageModel>(0, "", null);
                    }
                    return new Response<TaskMessageModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<TaskMessageModel>(-1, ex.ToString(), null);
            }
        }
        public void CheckMyTasks(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var unifys = unitOfWork.GetRepository<DocumentUnify>().GetMany(d => d.CreatedByUserId == UserId
                    && d.IsFinished == 0 && d.FinishedOnDate < DateTime.Now).ToList();
                    foreach (var item in unifys)
                    {
                        var existed = unitOfWork.GetRepository<TaskMessage>()
                            .Get(m => m.UserId == UserId && m.CreatedByUserId == UserId && m.TaskType == (int)TaskType.UNIFY && m.ModuleId == item.ModuleId && m.IsMyTask == true);
                        if (existed == null)
                        {
                            Create(new TaskMessageCreateModel
                            {
                                CreatedByUserId = UserId,
                                Deadline = DateTime.Now.AddDays(3),
                                RelatedId = item.UnifyId,
                                TaskType = (int)TaskType.UNIFY,
                                Title = "Tham khảo thống nhất: " + item.Name,
                                UserId = UserId,
                                ModuleId = item.ModuleId,
                                IsMyTask = true
                            });
                        }
                    };
                    //
                    var confirms = unitOfWork.GetRepository<DocumentConfirm>().GetMany(d =>
                     d.IsFinished == 0 && d.FinishedOnDate < DateTime.Now).ToList();
                    foreach (var item in confirms)
                    {
                        var existed = unitOfWork.GetRepository<TaskMessage>()
                            .Get(m => m.UserId == UserId && m.TaskType == (int)TaskType.CONFIRM && m.ModuleId == item.ModuleId && m.IsMyTask == true);
                        if (existed == null)
                        {
                            Create(new TaskMessageCreateModel
                            {
                                CreatedByUserId = UserId,
                                Deadline = DateTime.Now.AddDays(3),
                                RelatedId = item.ConfirmId,
                                TaskType = (int)TaskType.CONFIRM,
                                Title = "Tham khảo phê duyệt: " + item.Name,
                                UserId = UserId,
                                ModuleId = item.ModuleId,
                                IsMyTask = true

                            });
                        }
                    };
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Response<List<ExpiredTaskModule>> GetExpiredTasks(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<ExpiredTaskModule>>(0, "", null);
                    if (user.UserRoleId == 2) return GetExpiredTasksByDepartment(user.DepartmentId);
               
                    List<ExpiredTaskModule> results = new List<ExpiredTaskModule>();
                    results.AddRange(unitOfWork.GetRepository<DocumentUnify>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new ExpiredTaskModule
                        {
                            ModuleId = d.ModuleId,
                            TaskType = (int)TaskType.UNIFY,
                            DeadLine = d.FinishedOnDate,
                            Title = d.Name,
                            CreatedByUser = u.UserName,
                            RelatedDocumentId=d.RelatedDocumentId
                        }).ToList());
                    //
                    results.AddRange(unitOfWork.GetRepository<DocumentConfirm>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                     .Join(unitOfWork.GetRepository<User>().GetAll(),
                     d => d.CreatedByUserId,
                     u => u.UserId,
                     (d, u) => new ExpiredTaskModule
                     {
                         ModuleId = d.ModuleId,
                         TaskType = (int)TaskType.CONFIRM,
                         DeadLine = d.FinishedOnDate,
                         Title = d.Name,
                         CreatedByUser = u.UserName,
                         RelatedDocumentId = d.RelatedDocumentId
                     }).ToList());
                    //
                    results.AddRange(unitOfWork.GetRepository<DocumentPerform>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                     .Join(unitOfWork.GetRepository<User>().GetAll(),
                     d => d.CreatedByUserId,
                     u => u.UserId,
                     (d, u) => new ExpiredTaskModule
                     {
                         ModuleId = d.ModuleId,
                         TaskType = (int)TaskType.PERFORM,
                         DeadLine = d.FinishedOnDate,
                         Title = d.Name,
                         CreatedByUser = u.UserName,
                         RelatedDocumentId = d.RelatedDocumentId
                     }).ToList());
                    return new Response<List<ExpiredTaskModule>>(1, "", results);

                }
            }
            catch (Exception ex)
            {
                return new Response<List<ExpiredTaskModule>>(-1, ex.ToString(), null);
            }
        }
        private Response<List<ExpiredTaskModule>> GetExpiredTasksByDepartment(int DepartmentId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<ExpiredTaskModule> results = new List<ExpiredTaskModule>();
                    results.AddRange(unitOfWork.GetRepository<DocumentUnify>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                        .Join(unitOfWork.GetRepository<User>().GetMany(u=>u.DepartmentId==DepartmentId),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new ExpiredTaskModule
                        {
                            ModuleId = d.ModuleId,
                            TaskType = (int)TaskType.UNIFY,
                            DeadLine = d.FinishedOnDate,
                            Title = d.Name,
                            CreatedByUser = u.UserName,
                            RelatedDocumentId = d.RelatedDocumentId
                        }).ToList());
                    //
                    results.AddRange(unitOfWork.GetRepository<DocumentConfirm>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                     .Join(unitOfWork.GetRepository<User>().GetMany(u => u.DepartmentId == DepartmentId),
                     d => d.CreatedByUserId,
                     u => u.UserId,
                     (d, u) => new ExpiredTaskModule
                     {
                         ModuleId = d.ModuleId,
                         TaskType = (int)TaskType.CONFIRM,
                         DeadLine = d.FinishedOnDate,
                         Title = d.Name,
                         CreatedByUser = u.UserName,
                         RelatedDocumentId = d.RelatedDocumentId
                     }).ToList());
                    //
                    results.AddRange(unitOfWork.GetRepository<DocumentPerform>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                     .Join(unitOfWork.GetRepository<User>().GetMany(u => u.DepartmentId == DepartmentId),
                     d => d.CreatedByUserId,
                     u => u.UserId,
                     (d, u) => new ExpiredTaskModule
                     {
                         ModuleId = d.ModuleId,
                         TaskType = (int)TaskType.PERFORM,
                         DeadLine = d.FinishedOnDate,
                         Title = d.Name,
                         CreatedByUser = u.UserName,
                         RelatedDocumentId = d.RelatedDocumentId
                     }).ToList());
                    return new Response<List<ExpiredTaskModule>>(1, "", results);

                }
            }
            catch (Exception ex)
            {
                return new Response<List<ExpiredTaskModule>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<PendingTaskModel>> GetPendingTasks(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<PendingTaskModel>>(0, "User doesn't exist!", null);
                    if (user.UserRoleId == 2) return GetPendingTasksByDepartment(user.DepartmentId);
                    var results = unitOfWork.GetRepository<DocumentProcess>().GetMany(d=>d.Status<2&&d.Status>=0)
                        .Select(d => new PendingTaskModel
                        {
                            CreatedOnDate = d.CreatedOnDate,
                            RelatedId = d.RelatedId,
                            Id = d.Id,
                            TaskType = d.TaskType,
                            Status=d.Status
                        }).ToList();
                    return new Response<List<PendingTaskModel>>(1, "", results);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<PendingTaskModel>>(-1, ex.ToString(), null);
            }
        }
        private Response<List<PendingTaskModel>> GetPendingTasksByDepartment(int DepartmentId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<int> l = new List<int>();
                    l.AddRange(unitOfWork.GetRepository<SendDocument>().GetMany(d => d.DepartmentId == DepartmentId && d.DocumentProcessId != null).Select(d => (int)d.DocumentProcessId).ToList());
                    l.AddRange(unitOfWork.GetRepository<ReceivedDocument>().GetMany(d => d.DepartmentId == DepartmentId && d.DocumentProcessId != null).Select(d => (int)d.DocumentProcessId).ToList());
                    l.AddRange(unitOfWork.GetRepository<InternalDocument>().GetMany(d => d.DepartmentId == DepartmentId && d.DocumentProcessId != null).Select(d => (int)d.DocumentProcessId).ToList());

                    var results = unitOfWork.GetRepository<DocumentProcess>().GetMany(d => d.Status < 2 && d.Status >= 0 && l.Contains(d.Id))
                        .Select(d => new PendingTaskModel
                        {
                            CreatedOnDate = d.CreatedOnDate,
                            RelatedId = d.RelatedId,
                            Id = d.Id,
                            TaskType = d.TaskType,
                            Status = d.Status
                        }).ToList();
                    return new Response<List<PendingTaskModel>>(1, "", results);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<PendingTaskModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<PendingTaskDetailModel>> GetDetailPendingTask(int type)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    List<PendingTaskDetailModel> results = new List<PendingTaskDetailModel>();
                    switch (type)
                    {
                        case (int)TaskType.UNIFY:
                            {
                                results.AddRange(unitOfWork.GetRepository<DocumentUnify>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished==0)
                               .Join(unitOfWork.GetRepository<User>().GetAll(),
                               d => d.CreatedByUserId,
                               u => u.UserId,
                               (d, u) => new PendingTaskDetailModel
                               {
                                   ModuleId = d.ModuleId,
                                   TaskType = (int)TaskType.UNIFY,
                                   DeadLine = d.FinishedOnDate,
                                   Title = d.Name,
                                   CreatedByUser = u.UserName
                               }).ToList());
                                break;
                            }
                        case (int)TaskType.CONFIRM:
                            {
                                //
                                results.AddRange(unitOfWork.GetRepository<DocumentConfirm>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                                 .Join(unitOfWork.GetRepository<User>().GetAll(),
                                 d => d.CreatedByUserId,
                                 u => u.UserId,
                                 (d, u) => new PendingTaskDetailModel
                                 {
                                     ModuleId = d.ModuleId,
                                     TaskType = (int)TaskType.CONFIRM,
                                     DeadLine = d.FinishedOnDate,
                                     Title = d.Name,
                                     CreatedByUser = u.UserName
                                 }).ToList());
                                break;
                            }
                        case (int)TaskType.PERFORM:
                            {
                                //
                                results.AddRange(unitOfWork.GetRepository<DocumentPerform>().GetMany(d => d.FinishedOnDate < DateTime.Today && d.IsFinished == 0)
                                 .Join(unitOfWork.GetRepository<User>().GetAll(),
                                 d => d.CreatedByUserId,
                                 u => u.UserId,
                                 (d, u) => new PendingTaskDetailModel
                                 {
                                     ModuleId = d.ModuleId,
                                     TaskType = (int)TaskType.PERFORM,
                                     DeadLine = d.FinishedOnDate,
                                     Title = d.Name,
                                     CreatedByUser = u.UserName
                                 }).ToList());
                                break;
                            }

                    }




                    return new Response<List<PendingTaskDetailModel>>(1, "", results);

                }
            }
            catch (Exception ex)
            {
                return new Response<List<PendingTaskDetailModel>>(-1, ex.ToString(), null);
            }
        }

    }
}