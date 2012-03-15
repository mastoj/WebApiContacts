using System;
using System.Collections.Generic;
using System.Linq;
using WebApiContacts.Models;

namespace WebApiContacts.Controllers
{
    public class ContactRepository
    {
        private static List<Contact> _contacts = new List<Contact>();
        private static int nextId = 0;

        static ContactRepository()
        {
            _contacts.Add(new Contact(0, "Alexander", "Rybak", "34343434", "/Images/contacts/alexander_rybak.jpg", DateTime.Now.AddDays(-1)));
            _contacts.Add(new Contact(1, "Bruce", "Lee", "343423434", "/Images/contacts/bruce_lee.jpg", DateTime.Now.AddDays(-3)));
            _contacts.Add(new Contact(2, "Darth", "Vader", "1234323", "/Images/contacts/darth_vader.jpg", DateTime.Now.AddDays(-2)));
            _contacts.Add(new Contact(3, "Chuck", "Norris", "234234234", "/Images/contacts/chuck_norris.jpg", DateTime.Now.AddDays(-0)));
            _contacts.Add(new Contact(4, "George", "Bush", "11111111", "/Images/contacts/george_bush.jpg", DateTime.Now.AddDays(-0)));
            nextId = 5;
        }

        public IEnumerable<Contact> Get()
        {
            return _contacts;
        }

        public void Insert(Contact contact)
        {
            contact.Id = nextId++;
            contact.AddedDate = DateTime.Now;
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
    }
}