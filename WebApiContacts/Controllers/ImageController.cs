using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiContacts.Models;

namespace WebApiContacts.Controllers
{
    public class ImageController : ApiController
    {
        private ContactRepository _contactRepository;

        public ImageController(ContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public HttpResponseMessage<string> Post([FromUri]int id)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var folder = HttpContext.Current.Server.MapPath(WACConfiguration.ImageFolderPath);
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(folder);
            var task = Request.Content.ReadAsMultipartAsync(streamProvider);
            task.Wait();
            IDictionary<string, string> bodyPartFileNames = streamProvider.BodyPartFileNames;
            var fileName = bodyPartFileNames.Keys.First();
            fileName = fileName.Remove(fileName.Length - 1, 1).Remove(0, 1);
            _contactRepository.InsertImage(id, fileName);
            var response = new HttpResponseMessage<string>(fileName, HttpStatusCode.Created);
            var location = Url.Route("DefaultApi", new { httproute = "", controller = "Image", id });
            response.Headers.Location = new Uri(location, UriKind.Relative);
            return response;
        }
    }
}
