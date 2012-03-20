using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Common;
using System.Web.Http.Filters;
using WebApiContacts.Logging;

namespace WebApiContacts.Filters
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new Error
                    {
                        Name = e.Key,
                        Message = e.Value.Errors.First().ErrorMessage
                    }).ToArray();
                foreach (var error in errors)
                {
                    Log.Notification(error.Message);
                }
                actionContext.Response = new HttpResponseMessage<Error[]>(errors, HttpStatusCode.BadRequest);
            }
        }

        public class Error
        {
            public string Name { get; set; }
            public string Message { get; set; }
        }
    }
}