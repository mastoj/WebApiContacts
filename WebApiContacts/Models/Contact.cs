
using System.ComponentModel.DataAnnotations;

namespace WebApiContacts.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool HasImage { get; set; }

        public Contact()
        {
            
        }

        public static Contact CreateContact(int id, string firstName, string lastName, string phoneNumber, bool hasImage)
        {
            return new Contact()
                       {
                           Id = id,
                           FirstName = firstName,
                           LastName = lastName,
                           PhoneNumber = phoneNumber,
                           HasImage = hasImage
                       };
        }
    }
}