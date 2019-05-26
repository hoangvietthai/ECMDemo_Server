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
    public class DocumentConfirmController : ApiController
    {

        private IDbDocumentConfirmHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentConfirmHandler>();
        [HttpGet]
        [Route("documentconfirm")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("documentconfirm/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentconfirm")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(DocumentConfirmCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentconfirm/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, DocumentConfirmUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.UpdatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("documentconfirm/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentconfirm/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponse(int UserId, int ConfirmId)
        {
            var result = handler.GetResponse(UserId, ConfirmId);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentconfirm/responses/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponseById(int Id)
        {
            var result = handler.GetResponse(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentconfirm/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CreateResponse(DocumentConfirmResponseModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            if (createModel.UserId != createModel.UserId) return BadRequest();
            var result = handler.CreateResponse(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentconfirm/finish/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Finish(int Id, FinishConfirmModel model)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            model.UserId = Convert.ToInt32(UserId);
            var result = handler.Finish(Id, model);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentconfirm/recreate/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Resend(int Id, RecreateDocumentConfirmModel recreateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            recreateModel.UserId = Convert.ToInt32(UserId);
            var result = handler.ReCreate(Id, recreateModel);
            return Ok(result);
        }
    }
}

