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
    public class ReceivedDocumentController : ApiController
    {
        private IDbReceivedDocumenntHandler handler = BusinessServiceLocator.Instance.GetService<IDbReceivedDocumenntHandler>();
        [HttpGet]
        [Route("receiveddocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAll(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("receiveddocument/base")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetAllBase()
        {
            var result = handler.GetAllBase();
            return Ok(result);
        }
        [HttpGet]
        [Route("receiveddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("receiveddocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(ReceivedDocumentCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("receiveddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, ReceivedDocumentUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.LastModifiedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("receiveddocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("receiveddocument/register/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Register(int Id)
        {
            var result = handler.Register(Id);
            return Ok(result);
        }
    }
}
