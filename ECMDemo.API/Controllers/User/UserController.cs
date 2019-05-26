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

namespace ECMDemo.API.Controllers.User
{
    [RoutePrefix("api/v1/ecmdemo")]
    public class UserController : ApiController
    {
        private IDbUserHandler handler = BusinessServiceLocator.Instance.GetService<IDbUserHandler>();
        [HttpGet]
        [Route("user")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GellAll()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAll(Convert.ToInt32(UserId));
            //var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("user")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GellAllByDepartment(int DepartmentId)
        {
            var result = handler.GetByDepartment(DepartmentId);
            return Ok(result);
        }
        [HttpGet]
        [Route("user/forconfirm")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetConfirmUsers()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetConfirmUsers(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("user/base")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GellAllBase()
        {
            string UserId = Request.Headers.GetValues("UserId").FirstOrDefault();
            var result = handler.GetAllBase(Convert.ToInt32(UserId));
            return Ok(result);
        }
        [HttpGet]
        [Route("user/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("user/{Id}/detail")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize]
        public IHttpActionResult GetDetailById(int Id)
        {
            var result = handler.GetDetail(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("user")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Create(UserCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("user/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Update(int Id, UserUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("user/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [APIAuthorize(Roles = "Admin")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
