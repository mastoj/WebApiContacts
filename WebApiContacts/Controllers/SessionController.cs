using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApiContacts.Controllers
{
    public class SessionController : Controller
    {
        public JsonResult Create(string userName, string password)
        {
            if (userName == password && !string.IsNullOrEmpty(userName))
            {
                FormsAuthentication.SetAuthCookie(userName, true);
                return new JsonResult() {Data = "LoggedIn"};
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new JsonResult() {Data = "Invalid credentials"};
        }
    }
}
