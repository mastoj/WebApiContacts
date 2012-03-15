using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage<Contact> Post(Contact value)
        {
            _contactRepository.Insert(value);
            var response = new HttpResponseMessage<Contact>(value, HttpStatusCode.OK);
            return response;
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
}