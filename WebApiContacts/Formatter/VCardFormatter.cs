using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using WebApiContacts.Models;

namespace WebApiContacts.Formatter
{
    public class VCardFormatter : BufferedMediaTypeFormatter
    {
        public VCardFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/vcard"));
            MediaTypeMappings.Add(new QueryStringMapping("format", "vcard", "text/vcard"));
        }

        protected override IEnumerable<KeyValuePair<string, string>> OnGetResponseHeaders(Type objectType, string mediaType, System.Net.Http.HttpResponseMessage responseMessage)
        {
            responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {FileName = "contact.vcf"};
            return base.OnGetResponseHeaders(objectType, mediaType, responseMessage);
        }

        protected override void OnWriteToStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            var singleContact = value as Contact;
            if (singleContact != null)
            {
                WriteContact(singleContact, stream);
            }
        }

        private void WriteContact(Contact contact, Stream stream)
        {
            var writer = new StreamWriter(stream);
            writer.WriteLine("BEGIN:VCARD");
            writer.WriteLine(string.Format("FN:{0}", contact.LastName + " " + contact.FirstName));
            writer.WriteLine(string.Format("ADR;TYPE=HOME;{0};{1};{2}", contact.Address, contact.City, contact.Zip));
            writer.WriteLine(string.Format("EMAIL;TYPE=PREF,INTERNET:{0}", contact.Email));
            writer.WriteLine("END:VCARD");
            writer.Flush();
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Contact) == type)
                return true;
            return false;
        }
    }
}