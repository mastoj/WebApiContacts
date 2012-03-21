
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace WebApiContacts.Models
{
    public class CreateContactMessage
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"\d{6}", ErrorMessage = "Enter six digit phone number with no space.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [RegularExpression(@"\d{4}", ErrorMessage = "Enter your four digit zip.")]
        public string Zip { get; set; }
        [Email]
        [Required]
        public string Email { get; set; }
    }

    public class Contact : CreateContactMessage
    {
        public int Id { get; set; }
        public bool HasImage { get; set; }

        public static Contact CreateContact(int id, string firstName = "", string lastName = "", string phoneNumber = "", bool hasImage = true, string address = "", string city = "", string email = "", string zip = "") 
        {
            return new Contact()
                       {
                           Id = id,
                           FirstName = firstName,
                           LastName = lastName,
                           PhoneNumber = phoneNumber,
                           Address = address,
                           City = city,
                           Email = email,
                           Zip = zip,
                           HasImage = hasImage
                       };
        }
    }
}