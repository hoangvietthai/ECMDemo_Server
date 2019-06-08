using ECMDemo.Business;
using ECMDemo.Business.Handler;
using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ECMDemo.API.Controllers
{
    [RoutePrefix("api/v1/ecmdemo")]
    public class TaskMessageController : ApiController
    {
        private IDbTaskMessageHandler handler = BusinessServiceLocator.Instance.GetService<IDbTaskMessageHandler>();
        [HttpGet]
        [Route("taskmessage")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {

            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("taskmessage")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll(int UserId)
        {
            handler.CheckMyTasks(Convert.ToInt32(UserId));
            return Ok(handler.GetAll(UserId));
            
        }
        [HttpGet]
        [Route("taskmessage/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return  Ok(result);
        }
        [HttpPost]
        [Route("taskmessage")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(TaskMessageCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("taskmessage/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, int Status)
        {
          
            var result = handler.Update(Id, Status);
            return Ok(result);
        }
        [HttpDelete]
        [Route("taskmessage/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        //
        [HttpGet]
        [Route("pendingtasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PendingTask()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetPendingTasks(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("pendingtasks/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetDetailPendingTask(int Id)
        {
            var result = handler.GetDetailPendingTask(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("expiredtasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetExpiredTasks(Convert.ToInt32(UserId));
            return Ok(result);
        }
    }
}
