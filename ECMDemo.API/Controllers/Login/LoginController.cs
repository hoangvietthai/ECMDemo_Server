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

namespace ECMDemo.API.Controllers.Login
{
    [RoutePrefix("api/v1/ecmdemo")]
    public class LoginController : ApiController
    {
        private IDbLoginHandler handler = BusinessServiceLocator.Instance.GetService<IDbLoginHandler>();
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CheckLogin([FromBody]LoginModel loginModel)
        {
            return Ok(handler.CheckLogin(loginModel));
        }
    }
}
