using ECMDemo.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ECMDemo.API.Filters
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            string mess = "";
            if (Authorize(filterContext,out mess))
            {
                return;
            }
            filterContext.ActionArguments.Add("mess", mess);
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            filterContext.Response = new HttpResponseMessage
            {
               
                StatusCode = HttpStatusCode.Unauthorized,
            
                Content = new StringContent(filterContext.ActionArguments["mess"].ToString())
            };
        }

        private bool Authorize(HttpActionContext actionContext,out string message)
        {
            try
            {
                message = "Authorization has been denied for this request.";
                var encodedString = actionContext.Request.Headers.GetValues("Token").First();
                string UserId = actionContext.Request.Headers.GetValues("UserId").FirstOrDefault();
                int Id;
                if (!int.TryParse(UserId, out Id)) return false;
                bool validFlag = BusinessServiceLocator.Instance.GetService<IAuthenticate>().Authorize(Roles, encodedString, Id,out message);

                return validFlag;
            }
            catch (Exception ex)
            {
                message = "Authorization has been denied for this request.";
                return false;
            }
        }
    }
}