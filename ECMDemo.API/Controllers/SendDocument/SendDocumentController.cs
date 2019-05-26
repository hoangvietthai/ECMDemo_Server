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
    public class SendDocumentController : ApiController
    {
        private IDbSendDocumentHandler handler = BusinessServiceLocator.Instance.GetService<IDbSendDocumentHandler>();
        [HttpGet]
        [Route("senddocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAll(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("senddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("senddocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(SendDocumentCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("senddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, SendDocumentUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.LastModifiedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("senddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("senddocument/register/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Register(int Id)
        {
            var result = handler.Register(Id);
            return Ok(result);
        }
    }
}
