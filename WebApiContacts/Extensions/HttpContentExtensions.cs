using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebApiContacts.Controllers
{
    public static class HttpContentExtensions
    {
        public static string TryGetFormFieldValue(this IEnumerable<HttpContent> contents, string dispositionName, string defaultValue = "")    
        {    
            if (contents == null)    
            {    
                throw new ArgumentNullException("contents");    
            }    
             
            HttpContent content = contents.FirstDispositionNameOrDefault(dispositionName);    
            if (content != null)   
            {   
                var formFieldValue = content.ReadAsStringAsync().Result;   
                return formFieldValue;   
            }   
            return defaultValue;   
        }
    }
}