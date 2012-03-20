using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HttpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Push a button...");
            Console.ReadLine();
            var baseUrl = "http://localhost:51049/api/";
            var contactsUrl = baseUrl + "/Contacts";
            var httpClient = new HttpClient();
            httpClient.GetAsync(contactsUrl).ContinueWith(
                (responseTask) =>
                    {
                        var result = responseTask.Result;
                        result.EnsureSuccessStatusCode();
                        result.Content.ReadAsStringAsync().ContinueWith(
                            (readTask) =>
                                {
                                    Console.WriteLine(readTask.Result);
                                }
                            );
                    }
                );
            Console.WriteLine("Waiting...");
            Console.ReadLine();
        }
    }
}
