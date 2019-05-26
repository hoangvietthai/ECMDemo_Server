using ECMDemo.Business;
using ECMDemo.Business.Handler;
using ECMDemo.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ECMDemo.API.Controllers
{
    [RoutePrefix("api/v1/ecmdemo")]
    public class DocumentUnifyController : ApiController
    {

        private IDbDocumentUnifyHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentUnifyHandler>();
        [HttpGet]
        [Route("documentunify")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("documentunify/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentunify")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(DocumentUnifyCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentunify/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id,DocumentUnifyUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.UpdatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("documentunify/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        /////
        [HttpGet]
        [Route("documentunify/{Id}/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponses(int Id)
        {
            var result = handler.GetAllResponses(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentunify/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponse(int UserId,int UnifyId)
        {
            var result = handler.GetResponse(UserId, UnifyId);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentunify/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CreateResponse(DocumentUnifyResponseModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            if (createModel.UserId != createModel.UserId) return BadRequest();
            var result = handler.CreateResponse(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentunify/finish/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Finish(int Id,FinishUnifyModel model)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            model.UserId = Convert.ToInt32(UserId);
            var result = handler.Finish(Id, model);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentunify/recreate/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Resend(int Id, RecreateDocumentUnifyModel recreateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            recreateModel.UserId = Convert.ToInt32(UserId);
            var result = handler.ReCreate(Id, recreateModel);
            return Ok(result);
        }
    }
}
