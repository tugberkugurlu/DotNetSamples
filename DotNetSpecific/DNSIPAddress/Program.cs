using System;
using System.Linq;
using System.Net;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string potentialIPAddress = "tugberkugurlu.com";

            var ipAddress = Dns.GetHostEntryAsync(potentialIPAddress).Result.AddressList.Last();
            Console.WriteLine(string.Join(",", hostEntry.AddressList.Select(x => x.ToString())));

            IPAddress ipAddress;
            if(IPAddress.TryParse(potentialIPAddress, out ipAddress)) 
            {
                Console.WriteLine($"ip address: {ipAddress}");
            }
            else 
            {
                System.Console.WriteLine("not an ip address, resolving ip address...");
            }
        }
    }
}
