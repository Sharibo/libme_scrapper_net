using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace libme_scrapper.code
{
    class Test2
    {
        static async Task Main(string[] args)
        {
            var ips = await Dns.GetHostAddressesAsync("www.google.com");

            foreach (var ipAddress in ips)
            {
                Console.WriteLine(ipAddress.MapToIPv4().ToString());
            }

            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(2),
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(2),
                MaxConnectionsPerServer = 2
            };

            var client = new HttpClient(socketsHandler);

            var sw = Stopwatch.StartNew();

            var tasks = Enumerable.Range(0, 200).Select(i => client.GetAsync("https://www.google.com"));

            await Task.WhenAll(tasks);

            sw.Stop();

            Console.WriteLine($"{sw.ElapsedMilliseconds}ms taken for 200 requests");

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
    }
}