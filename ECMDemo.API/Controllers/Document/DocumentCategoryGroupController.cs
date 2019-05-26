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
    public class DocumentCategoryGroupController : ApiController
    {
        private IDbDocumentGroupCateHandler handler = BusinessServiceLocator.Instance.GetService<IDbDocumentGroupCateHandler>();
        [HttpGet]
        [Route("docgroupcategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        public IHttpActionResult GetAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("docgroupcategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("docgroupcategory/active")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetActiveGroup()
        {
            var result = handler.GetActiveGroup();
            return Ok(result);
        }
        [HttpPost]
        [Route("docgroupcategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Create(DocumentGroupCateCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("docgroupcategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Create(int Id, DocumentGroupCateUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("docgroupcategory/active/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult ChangeActiveGroup(int Id)
        {
            var result = handler.ChangeActiveGroup(Id);
            return Ok(result);
        }
        [HttpDelete]
        [Route("docgroupcategory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
