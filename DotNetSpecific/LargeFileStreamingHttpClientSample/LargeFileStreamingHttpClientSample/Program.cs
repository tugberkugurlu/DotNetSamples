using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileStreamingHttpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // DoItSoWrongAsync().Wait();
            // DoItWrongAsync().Wait();
            // DoItRightAsync().Wait();
            Console.ReadLine();
        }

        static async Task DoItSoWrongAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://github.com/tugberkugurlu/AspNet.Identity.RavenDB/archive/v2.0.0-pre-06.zip"))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    using (FileStream fileStream = File.Open(Path.GetTempFileName(), FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        await writer.WriteAsync(content);
                    }
                }
            }
        }

        static async Task DoItWrongAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://github.com/tugberkugurlu/AspNet.Identity.RavenDB/archive/v2.0.0-pre-06.zip"))
                {
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = File.Open(Path.GetTempFileName(), FileMode.Create))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        static async Task DoItRightAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://github.com/tugberkugurlu/AspNet.Identity.RavenDB/archive/v2.0.0-pre-06.zip", HttpCompletionOption.ResponseHeadersRead))
                {
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = File.Open(Path.GetTempFileName(), FileMode.Create))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }
        }
    }
}
