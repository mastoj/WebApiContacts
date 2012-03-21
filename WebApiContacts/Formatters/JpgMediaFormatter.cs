using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace WebApiContacts.Formatter
{
    public class JpgMediaFormatter : BufferedMediaTypeFormatter
    {
        public JpgMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            MediaTypeMappings.Add(new QueryStringMapping("format", "jpg", "image/jpg"));
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof (string) == type)
            {
                return true;
            }
            return false;
        }

        protected override void OnWriteToStream(Type type, object value, Stream stream,
                                                HttpContentHeaders contentHeaders, FormatterContext formatterContext,
                                                TransportContext transportContext)
        {
            var imageName = value as string;
            if (!string.IsNullOrEmpty(imageName))
            {
                var path = HttpContext.Current.Server.MapPath(WACConfiguration.ImageFolderPath + imageName);
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(stream);
                }
                stream.Flush();
            }
        }
    }
}