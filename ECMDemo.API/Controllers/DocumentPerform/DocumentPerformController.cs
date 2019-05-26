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
    public class DocumentPerformController : ApiController
    {

        private IDbDocumentPerformHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentPerformHandler>();
        [HttpGet]
        [Route("documentperform")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("documentperform/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentperform")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(DocumentPerformCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentperform/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, DocumentPerformUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.UpdatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("documentperform/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        /////
        [HttpGet]
        [Route("documentperform/{Id}/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponses(int Id)
        {
            var result = handler.GetAllResponses(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentperform/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetResponse(int UserId, int PerformId)
        {
            var result = handler.GetResponse(UserId, PerformId);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentperform/responses")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CreateResponse(DocumentPerformResponseModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            if (createModel.UserId != createModel.UserId) return BadRequest();
            var result = handler.CreateResponse(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentperform/finish/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Finish(int Id, FinishPerformModel model)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            model.UserId = Convert.ToInt32(UserId);
            var result = handler.Finish(Id, model);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentperform/recreate/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Resend(int Id, RecreateDocumentPerformModel recreateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            recreateModel.UserId = Convert.ToInt32(UserId);
            var result = handler.ReCreate(Id, recreateModel);
            return Ok(result);
        }
    }
}
