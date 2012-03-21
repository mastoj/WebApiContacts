
namespace WebApiContacts.Models
{
    public class CreateContactMessage
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
    }

    public class Contact : CreateContactMessage
    {
        public int Id { get; set; }
        public bool HasImage { get; set; }

        public static Contact CreateContact(int id, string firstName = "", string lastName = "", string phoneNumber = "",
                                            bool hasImage = true, string address = "", string city = "",
                                            string email = "", string zip = "")
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