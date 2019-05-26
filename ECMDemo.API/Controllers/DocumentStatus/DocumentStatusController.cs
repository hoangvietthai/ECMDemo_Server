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
    public class DocumentStatusController : ApiController
    {
        private IDbDocumentStatusHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentStatusHandler>();
        [HttpGet]
        [Route("documentstatus")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("documentstatus/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentstatus")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(DocumentStatusCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentstatus/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, DocumentStatusUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("documentstatus/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
