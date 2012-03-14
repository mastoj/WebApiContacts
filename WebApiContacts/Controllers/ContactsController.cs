using System;
using System.Collections.Generic;
using System.Linq;
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
        public Contact Get(int id)
        {
            var contact = _contactRepository.Get().Where(y => y.Id == id).SingleOrDefault();
            return contact;
        }

        // POST /api/Contacts
        public Contact Post(Contact value)
        {
            _contactRepository.Insert(value);
            return value;
        }

        // PUT /api/Contacts/5
        public Contact Put(int id, Contact value)
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
            return value;
        }

        // DELETE /api/Contacts/5
        public void Delete(int id)
        {
            _contactRepository.Delete(id);
        }
    }
}