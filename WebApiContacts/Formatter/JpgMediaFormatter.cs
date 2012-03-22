using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebApiContacts.Controllers;
using WebApiContacts.Models;

namespace WebApiContacts.Formatter
{
    public class JpgMediaFormatter : BufferedMediaTypeFormatter
    {
        private ContactRepository _contactsRepository;

        public JpgMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            MediaTypeMappings.Add(new QueryStringMapping("format", "jpg", "image/jpg"));
            _contactsRepository = new ContactRepository();
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Contact) == type)
            {
                return true;
            }
            return false;
        }

        protected override System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> OnGetResponseHeaders(Type objectType, string mediaType, System.Net.Http.HttpResponseMessage responseMessage)
        {
            var contact = responseMessage.Content.ReadAsAsync(objectType).Result as Contact;
            if(contact != null)
            {
                string fileName = string.Format("{0}_{1}.jpg", contact.FirstName, contact.LastName);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                                                         {FileName = fileName};
            }
            return base.OnGetResponseHeaders(objectType, mediaType, responseMessage);
        }

        protected override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            var contact = value as Contact;
            if (contact != null)
            {
                if (contact.HasImage)
                {
                    var imageName = _contactsRepository.GetImage(contact.Id);
                    var path = HttpContext.Current.Server.MapPath(WACConfiguration.ImageFolderPath + imageName);
                    using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(stream);
                    }
                    stream.Flush();
                }
                else
                {
                    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var writer = new StreamWriter(stream);
                    writer.WriteLine("Contact has no image");
                    writer.Flush();
                }
            }
        }
    }
}