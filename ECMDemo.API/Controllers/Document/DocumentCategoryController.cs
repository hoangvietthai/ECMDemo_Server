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
    public class DocumentCategoryController : ApiController
    {
        private IDbDocumentCategoryHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentCategoryHandler>();
        [HttpGet]
        [Route("doccategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("doccategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll(int groupId)
        {
            var result = handler.GetAll(groupId);
            return Ok(result);
        }
        [HttpGet]
        [Route("doccategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("doccategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Create(DocumentCategoryCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("doccategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Update(int Id, DocumentCategoryUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("doccategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
