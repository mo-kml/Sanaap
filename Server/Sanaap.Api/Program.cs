using Bit.OwinCore;
using Microsoft.AspNetCore.Hosting;

namespace Sanaap.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            BitWebHost.CreateDefaultBuilder(args)
                .UseStartup<AppStartup>()
#if DEBUG
                .UseUrls("http://*:53148/")
#endif
                .Build();
    }
}
