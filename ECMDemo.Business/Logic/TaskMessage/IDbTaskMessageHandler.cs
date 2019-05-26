using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public interface IDbTaskMessageHandler
    {
        Response<TaskMessageModel> GetById(int Id);
        Response<List<TaskMessageDisplayModel>> GetAll();
        Response<List<TaskMessageDisplayModel>> GetAll(int UserId);
        Response<TaskMessageModel> Create(TaskMessageCreateModel createModel);
        Response<TaskMessageModel> Update(int Id, int Status);
        Response<TaskMessageModel> Delete(int Id);
        Response<List<TaskMessageModel>> SendToListUser(string list, TaskMessageCreateModel createModel);
        void CheckMyTasks(int UserId);
        //
        Response<List<ExpiredTaskModule>> GetExpiredTasks();
        Response<List<PendingTaskModel>> GetPendingTasks();
        Response<List<PendingTaskDetailModel>> GetDetailPendingTask(int type);

    }
}