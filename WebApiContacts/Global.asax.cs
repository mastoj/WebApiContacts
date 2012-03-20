using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApiContacts.Filters;

namespace WebApiContacts
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {   
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var config = GlobalConfiguration.Configuration;
            config.Filters.Add(new ValidationActionFilter());
            config.Formatters.Add(new JpgMediaFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }

    public class JpgMediaFormatter : BufferedMediaTypeFormatter
    {
        public JpgMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            MediaTypeMappings.Add(new QueryStringMapping("format", "jpg", "image/jpg"));
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(string) == type)
            {
                return true;
            }
            return false;
        }

        protected override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            var imageName = value as string;
            if (!string.IsNullOrEmpty(imageName))
            {
                var path = HttpContext.Current.Server.MapPath(WACConfiguration.ImageFolderPath + imageName);
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(stream);
                }
                stream.Flush();
            }
        }
    }
}