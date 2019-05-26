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
    public class DepartmentController : ApiController
    {
        private IDbDepartmentHandler handler = BusinessServiceLocator.Instance.GetService<IDbDepartmentHandler>();
        [HttpGet]
        [Route("department")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("department/{Id}/subs")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetByParentId(int Id)
        {
            var result = handler.GetByParentId(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("department/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("department")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(DepartmentCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("department/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, DepartmentupdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("department/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
