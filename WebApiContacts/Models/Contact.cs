using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiContacts.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public DateTime AddedDate { get; set; }

        public Contact()
        {
            
        }

        public Contact(int id, string firstName, string lastName, string phoneNumber, string image, DateTime addedDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Image = image;
            AddedDate = addedDate;
        }
    }
}