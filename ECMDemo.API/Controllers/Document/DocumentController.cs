using ECMDemo.API.Filters;
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
    public class DocumentController : ApiController
    {
        private IDbDocumentHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentHandler>();
        [HttpGet]
        [Route("document")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();

            var result = handler.GetAll(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpPost]
        [Route("document/search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll(FilterDocument filter)
        {
            var result = handler.GetAll(filter);
            return Ok(result);
        }
        [HttpGet]
        [Route("document")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll(int UserId)
        {
            var result = handler.GetByUser(UserId);
            return Ok(result);
        }
        [HttpGet]
        [Route("document")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetByTypeId(int TypeId)
        {
            var result = handler.GetByType(TypeId);
            return Ok(result);
        }
        [HttpGet]
        [Route("document")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetByDirectory(int DirectoryId)
        {
            var result = handler.GetDocsInDirectory(DirectoryId);
            return Ok(result);
        }
        [HttpGet]
        [Route("document/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("document")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin,Special")]
        public IHttpActionResult Create(DocumentCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("document/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin,Special")]
        public IHttpActionResult Update(int Id, DocumentUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("document/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin,Special")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
