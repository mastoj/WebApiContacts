using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiContacts.Models;

namespace WebApiContacts.Controllers
{
    public class ContactsController : ApiController
    {
        private ContactRepository _contactRepository;

        public ContactsController()
        {
            _contactRepository = new ContactRepository();
        }


        // GET /api/Contacts
        public IEnumerable<Contact> Get()
        {
            return _contactRepository.Get();
        }

        // GET /api/Contacts/5
        public HttpResponseMessage<Contact> Get(int id)
        {
            var contact = _contactRepository.Get().Where(y => y.Id == id).SingleOrDefault();
            var statusCode = contact != null ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            var response = new HttpResponseMessage<Contact>(contact, statusCode);
            return response;
        }

        // POST /api/Contacts
        public HttpResponseMessage<Contact> Post()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))    
            {    
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);    
            }
            var folder = HttpContext.Current.Server.MapPath("~/Images/contacts/");
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(folder);   
            
            // Read the MIME multipart content using the stream provider we just created.   
            var task = Request.Content.ReadAsMultipartAsync(streamProvider);
            task.Wait();
            var bodyparts = task.Result;
            // The submitter field is the entity with a Content-Disposition header field with a "name" parameter with value "submitter"   
            string firstName = bodyparts.TryGetFormFieldValue("FirstName", "");
            string lastName = bodyparts.TryGetFormFieldValue("LastName", "");
            string phoneNumber = bodyparts.TryGetFormFieldValue("PhoneNumber", "");            
            // Get a dictionary of local file names from stream provider.   
            // The filename parameters provided in Content-Disposition header fields are the keys.   
            // The local file names where the files are stored are the values.   
            IDictionary<string, string> bodyPartFileNames = streamProvider.BodyPartFileNames;   
            
            var fileName = bodyPartFileNames.Keys.First();
            fileName = fileName.Remove(fileName.Length - 1, 1).Remove(0, 1);
            var fileNameAndPath = "/Images/contacts/" + fileName;
            var contact = new Contact() {FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, Image = fileNameAndPath};
            _contactRepository.Insert(contact);
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            var routeData = new Dictionary<string, string> {{"id", contact.Id.ToString()}};
            var location = Url.Route("DefaultApi", new { httproute = "", controller = "Contacts", id = contact.Id.ToString() });
            response.Headers.Location = new Uri(location, UriKind.Relative);
            return response;

            //var firstName = 
            //var fileName = (value.FirstName + "_" + value.LastName + ".jpg").ToLower(); ;
            //value.Image = "/Images/contacts/" + fileName;
            //SaveFile(fileName);

            //_contactRepository.Insert(value);
            //var response = new HttpResponseMessage<Contact>(value, HttpStatusCode.Created);
            //return response;
        }

        private void SaveFile(string fileName)
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            try
            {
                var filePath = HttpContext.Current.Server.MapPath("~/Images/contacts/" + fileName);
                var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (IOException ex)
            {
                throw new HttpResponseException("A generic error occured. Please try again later.", HttpStatusCode.InternalServerError);
            }
        }

        // PUT /api/Contacts/5
        public HttpResponseMessage<Contact> Put(int id, Contact value)
        {
            var contact = _contactRepository.Get().SingleOrDefault(y => y.Id == id);
            if (value != null)
            {
                contact.FirstName = value.FirstName;
                contact.LastName = value.LastName;
                contact.PhoneNumber = value.PhoneNumber;
                contact.Image = value.Image;
                contact.AddedDate = value.AddedDate;
            }
            var response = new HttpResponseMessage<Contact>(contact, HttpStatusCode.Accepted);
            return response;
        }

        // DELETE /api/Contacts/5
        public HttpResponseMessage Delete(int id)
        {
            _contactRepository.Delete(id);
            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            return response;
        }
    }

    public static class HttpContentExtensions
    {
         public static string TryGetFormFieldValue(this IEnumerable<HttpContent> contents, string dispositionName, string defaultValue)    
         {    
             if (contents == null)    
             {    
                 throw new ArgumentNullException("contents");    
             }    
             
             HttpContent content = contents.FirstDispositionNameOrDefault(dispositionName);    
             if (content != null)   
             {   
                 var formFieldValue = content.ReadAsStringAsync().Result;   
                 return formFieldValue;   
             }   
             return defaultValue;   
         }
    }
}