using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApiContacts.Controllers
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class SessionController : ApiController
    {

        public HttpResponseMessage<string> Post(Credentials credentials)
        {
            if (credentials.UserName == credentials.Password && !string.IsNullOrEmpty(credentials.UserName))
            {
                var authCookie = FormsAuthentication.GetAuthCookie(credentials.UserName, true);
                HttpContext.Current.Response.Cookies.Add(authCookie);
                return new HttpResponseMessage<string>("Logged in", HttpStatusCode.Created);
            }
            return new HttpResponseMessage<string>("Invalid credentials", HttpStatusCode.BadRequest);
        }
    }
}
