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
    public class InternalDocumentController : ApiController
    {
        private IDbInternalDocumentHandler handler = BusinessServiceLocator.Instance.GetService<IDbInternalDocumentHandler>();
        [HttpGet]
        [Route("internaldocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAll(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("internaldocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetAllInDirectory(int Directory)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAllInDirectory(Convert.ToInt32(UserId), Directory);
            return Ok(result);
        }
        [HttpGet]
        [Route("internaldocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("internaldocument")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(InternalDocumentCreateModel createModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            createModel.CreatedByUserId = Convert.ToInt32(UserId);
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("internaldocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, InternalDocumentUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.LastModifiedByUserId= Convert.ToInt32(UserId);
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("internaldocument/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("internaldocument/register/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Register(int Id)
        {
            var result = handler.Register(Id);
            return Ok(result);
        }
    }
}
