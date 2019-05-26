using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentProcessHandler : IDbDocumentProcessHandler
    {
        public Response<DocumentProcessModel> ChangeStatus(int Id, int OrderIndex,int DocumentId, int Status)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var entity = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == Id && d.OrderIndex == OrderIndex);
                if (entity == null) return new Response<DocumentProcessModel>(0, "", null);
                if (entity.Status != (int)DocumentProcessStatus.FINISHED)
                {
                    entity.Status = Status;
                    entity.RelatedId = DocumentId;
                    unitOfWork.GetRepository<DocumentProcess>().Update(entity);
                    if (unitOfWork.Save() >= 1)
                    {
                        return new Response<DocumentProcessModel>(1, "", null);
                    }
                    return new Response<DocumentProcessModel>(0, "", null);
                }
                return new Response<DocumentProcessModel>(0, "", null);


            }
        }

        public Response<int> Create(int DocumentId, int module, List<DocumentProcessCreateModel> list)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    list.OrderBy(d => d.OrderIndex);
                    //
                    int id = 1;
                    var last = unitOfWork.GetRepository<DocumentProcess>().GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
                    if (last != null) id = last.Id + 1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        var entity = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = item.OrderIndex,
                            RelatedId = item.RelatedId,
                            TaskType = item.TaskType
                        };
                   
                        unitOfWork.GetRepository<DocumentProcess>().Add(entity);
                    }
                    //
                    switch (module)
                    {
                        case (int)Module.SEND:
                            {
                                var tmp = unitOfWork.GetRepository<SendDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                        case (int)Module.RECEIVE:
                            {
                                var tmp = unitOfWork.GetRepository<ReceivedDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                        case (int)Module.INTERNAL:
                            {
                                var tmp = unitOfWork.GetRepository<InternalDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                    }
                    if (unitOfWork.Save() >= 1)
                    {
                        return new Response<int>(1, "", id);
                    }
                    return new Response<int>(0, "", -1);

                }
            }
            catch (Exception ex)
            {
                return new Response<int>(-1, ex.ToString(), -1);
            }
        }

        public Response<int> CreateAuto(int DocumentId, int ModuleType)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {

                    //
                    int id = 1;
                    var last = unitOfWork.GetRepository<DocumentProcess>().GetAll().OrderByDescending(d => d.Id).FirstOrDefault();
                    if (last != null) id = last.Id + 1;
                    if (ModuleType == (int)Module.SEND)
                    {
                        //Unitf
                        var entityUnify = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 1,
                            RelatedId = -1,
                            TaskType = (int)TaskType.UNIFY
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityUnify);
                        //Confirm
                        var entityConfirm = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 2,
                            RelatedId = -1,
                            TaskType = (int)TaskType.CONFIRM
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityConfirm);
                        //Register
                        var entityRegister = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 3,
                            RelatedId = -1,
                            TaskType = (int)TaskType.REGISTER
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityRegister);
                    }
                    else if (ModuleType == (int)Module.RECEIVE)
                    {
                        //Confirm
                        var entityConfirm = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 1,
                            RelatedId = -1,
                            TaskType = (int)TaskType.CONFIRM
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityConfirm);
                        //Register
                        var entityRegister = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 2,
                            RelatedId = -1,
                            TaskType = (int)TaskType.REGISTER
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityRegister);
                    }
                    else if (ModuleType == (int)Module.INTERNAL)
                    {
                        //Confirm
                        var entityConfirm = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 1,
                            RelatedId = -1,
                            TaskType = (int)TaskType.CONFIRM
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityConfirm);
                        //Perform
                        var entityPerform = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 2,
                            RelatedId = -1,
                            TaskType = (int)TaskType.PERFORM
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityPerform);
                        //Register
                        var entityRegister = new DocumentProcess
                        {
                            Id = id,
                            CreatedOnDate = DateTime.Now,
                            Status = (int)DocumentProcessStatus.NOTSET,
                            OrderIndex = 3,
                            RelatedId = -1,
                            TaskType = (int)TaskType.REGISTER
                        };
                        unitOfWork.GetRepository<DocumentProcess>().Add(entityRegister);
                    }
                    //
                    switch (ModuleType)
                    {
                        case (int)Module.SEND:
                            {
                                var tmp = unitOfWork.GetRepository<SendDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                        case (int)Module.RECEIVE:
                            {
                                var tmp = unitOfWork.GetRepository<ReceivedDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                        case (int)Module.INTERNAL:
                            {
                                var tmp = unitOfWork.GetRepository<InternalDocument>().GetById(DocumentId);
                                if (tmp != null) tmp.DocumentProcessId = id;
                                break;
                            }
                    }
                    if (unitOfWork.Save() >= 1)
                    {
                        return new Response<int>(1, "", id);
                    }
                    return new Response<int>(0, "", -1);

                }
            }
            catch (Exception ex)
            {
                return new Response<int>(-1, ex.ToString(), -1);
            }
        }

        public Response<DocumentProcessModel> GetById(int Id, int order)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentProcess>().Get(d => d.Id == Id && d.OrderIndex == order);
                    if (entity != null) return new Response<DocumentProcessModel>(1, "", Ultis.ConvertSameData<DocumentProcessModel>(entity));
                    return new Response<DocumentProcessModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentProcessModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentProcessModel> GetCurrentProcess(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentProcess>().GetMany(d => d.Id == Id && d.Status >= 1).OrderByDescending(d=>d.OrderIndex).OrderBy(d => d.Status).FirstOrDefault();
                    if (entity != null)
                        return new Response<DocumentProcessModel>(1, "", Ultis.ConvertSameData<DocumentProcessModel>(entity));
                    else return new Response<DocumentProcessModel>(0, "", null);

                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentProcessModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DocumentProcessModel>> GetListProcess(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var ds = unitOfWork.GetRepository<DocumentProcess>().GetMany(d => d.Id == Id)
                        .Select(d => new DocumentProcessModel
                        {
                            CreatedOnDate = d.CreatedOnDate,
                            Id = d.Id,
                            OrderIndex = d.OrderIndex,
                            RelatedId
                            = d.RelatedId,
                            Status = d.Status,
                            TaskType = d.TaskType
                        }).OrderBy(d => d.OrderIndex).ToList();
                    return new Response<List<DocumentProcessModel>>(1, "", ds);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentProcessModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentProcessModel> GetNext(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentProcess>().GetMany(d => d.Id == Id && d.Status < 1).OrderBy(d => d.OrderIndex).FirstOrDefault();
                    if (entity != null)
                        return new Response<DocumentProcessModel>(1, "", Ultis.ConvertSameData<DocumentProcessModel>(entity));
                    else return new Response<DocumentProcessModel>(0, "", null);
                }
            }
            catch(Exception ex)
            {
                return new Response<DocumentProcessModel>(-1, ex.ToString(), null);
            }
        }
    }
}