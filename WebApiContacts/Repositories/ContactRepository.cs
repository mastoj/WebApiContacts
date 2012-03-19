using System;
using System.Collections.Generic;
using System.Linq;
using WebApiContacts.Models;

namespace WebApiContacts.Controllers
{
    public class ContactRepository
    {
        private static Dictionary<int, string> _imageStore = new Dictionary<int, string>(); 
        private static List<Contact> _contacts = new List<Contact>();
        private static int nextId = 0;

        static ContactRepository()
        {
            _contacts.Add(Contact.CreateContact(0, "Alexander", "Rybak", "34343434", true)); _imageStore.Add(0, "alexander_rybak.jpg");
            _contacts.Add(Contact.CreateContact(1, "Bruce", "Lee", "2342", true)); _imageStore.Add(1, "bruce_lee.jpg");
            _contacts.Add(Contact.CreateContact(2, "Darth", "Vader", "24235", true)); _imageStore.Add(2, "darth_vader.jpg");
            _contacts.Add(Contact.CreateContact(3, "Chuck", "Norris", "235235", true)); _imageStore.Add(3, "chuck_norris.jpg");
            _contacts.Add(Contact.CreateContact(4, "George", "Bush", "25235", true)); _imageStore.Add(4, "george_bush.jpg");
            nextId = 5;
        }

        public IEnumerable<Contact> Get()
        {
            return _contacts;
        }

        public void Insert(Contact contact)
        {
            contact.Id = nextId++;
            _contacts.Add(contact);
        }

        public void Delete(int id)
        {
            var contact = Get(id);
            if (contact != null)
            {
                _contacts.Remove(contact);
            }
        }

        private Contact Get(int id)
        {
            return _contacts.SingleOrDefault(y => y.Id == id);
        }

        public void InsertImage(int id, string fileName)
        {
            var contact = Get(id);
            _imageStore[id] = fileName;
            contact.HasImage = true;
        }

        public string GetImage(int id)
        {
            return _imageStore.SingleOrDefault(y => y.Key == id).Value;
        }
    }
}