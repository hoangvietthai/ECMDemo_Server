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
    public class DocumentProcessController : ApiController
    {
        private IDbDocumentProcessHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentProcessHandler>();
        [HttpGet]
        [Route("documentprocess/{Id}/active")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetCurrentProcess(int Id)
        {
            var result = handler.GetCurrentProcess(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentprocess/{Id}/next")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetNext(int Id)
        {
            var result = handler.GetNext(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentprocess/{Id}/all")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetListProcess(int Id)
        {
            var result = handler.GetListProcess(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("documentprocess/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetListProcess(int Id,int Order)
        {
            var result = handler.GetById(Id, Order);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentprocess")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(int DocumentId,int ModuleType,List<DocumentProcessCreateModel> createModel)
        {
            var result = handler.Create(DocumentId, ModuleType,createModel);
            return Ok(result);
        }
        [HttpPost]
        [Route("documentprocess/auto")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CreateAuto(int DocumentId, int ModuleType)
        {
            var result = handler.CreateAuto(DocumentId, ModuleType);
            return Ok(result);
        }
        [HttpPut]
        [Route("documentprocess/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult ChangeStatus(int Id,int OrderIndex,int DocumentId,int Status)
        {
            var result = handler.ChangeStatus(Id, OrderIndex, DocumentId, Status);
            return Ok(result);
        }
    }
}
