using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ECMDemo.API.Filters;
using ECMDemo.Business;
using ECMDemo.Business.Handler;
using ECMDemo.Business.Model;

namespace ECMDemo.API.Controllers
{
    [RoutePrefix("api/v1/ecmdemo")]
   
    public class DirectoryController : ApiController
    {
        private IDbDirectoryHandler handler = BusinessServiceLocator.Instance.GetService<IDbDirectoryHandler>();
        [HttpGet]
        [Route("directory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GellAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAll(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("directory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetAllByModule(int ModuleId)
        {
            var result = handler.GetAllByModule(ModuleId);
            return Ok(result);
        }
        [HttpGet]
        [Route("directory/nodes/{ModuleId}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        //[APIAuthorize]
        public IHttpActionResult GellAll(int ModuleId)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetNodes(ModuleId,Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("directory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetByParentId(int ParentId,int ModuleId)
        {
            var result = handler.GetByParentId(ParentId, ModuleId);
            return Ok(result);
        }
        [HttpGet]
        [Route("directory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("directory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize (Roles ="Admin,Special")]
        public IHttpActionResult Create(DirectoryCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("directory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin,Special")]
        public IHttpActionResult Update(int Id,DirectoryUpdateModel updateModel)
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            updateModel.LastModifiedByUserId = Convert.ToInt32(UserId);
            var result = handler.Update(Id,updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("directory/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin,Special")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
