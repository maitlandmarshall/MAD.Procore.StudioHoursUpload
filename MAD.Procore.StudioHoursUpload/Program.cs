using MAD.Integration.Common;
using Microsoft.Extensions.Hosting;

namespace MAD.Procore.StudioHoursUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            IntegrationHost.CreateDefaultBuilder(args)
                .UseAspNetCore()
                .UseAppInsights()
                .UseStartup<Startup>()
                .Build()            
                .Run();
        }
    }
}
