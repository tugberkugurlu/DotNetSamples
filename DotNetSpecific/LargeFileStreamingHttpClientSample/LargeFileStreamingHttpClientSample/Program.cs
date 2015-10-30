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
        const string Url = "https://download.elasticsearch.org/elasticsearch/release/org/elasticsearch/distribution/zip/elasticsearch/2.0.0/elasticsearch-2.0.0.zip";

        static void Main(string[] args)
        {
            // DoItSoWrongAsync().Wait();
            DoItWrongAsync().Wait();
            // DoItRightAsync().Wait();
        }

        static async Task DoItSoWrongAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(Url))
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
                using (HttpResponseMessage response = await client.GetAsync(Url))
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
                using (HttpResponseMessage response = await client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead))
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
