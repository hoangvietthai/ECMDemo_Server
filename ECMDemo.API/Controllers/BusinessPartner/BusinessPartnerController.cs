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
    public class BusinessPartnerController : ApiController
    {
        private IDbBusinessPartnerHandler handler = BusinessServiceLocator.Instance.GetService<IDbBusinessPartnerHandler>();
        
        //
        [HttpGet]
        [Route("businesspartner")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GellAll()
        {
            var result = handler.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("businesspartner/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetById(int Id)
        {
            var result = handler.GetById(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route("businesspartner")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(BusinessPartnerCreateModel createModel)
        {
            var result = handler.Create(createModel);
            return Ok(result);
        }
        [HttpPut]
        [Route("businesspartner/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update(int Id, BusinessPartnerUpdateModel updateModel)
        {
            var result = handler.Update(Id, updateModel);
            return Ok(result);
        }
        [HttpDelete]
        [Route("businesspartner/{Id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(int Id)
        {
            var result = handler.Delete(Id);
            return Ok(result);
        }
    }
}
