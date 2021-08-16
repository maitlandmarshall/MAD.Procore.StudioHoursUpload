using Hangfire;
using MAD.API.Procore;
using MAD.Integration.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MAD.Procore.RecurringStudioHoursUpload
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddIntegrationSettings<AppConfig>();
            serviceDescriptors.AddSingleton<ProcoreApiClient>(svc =>
            {
                var appConfig = svc.GetRequiredService<AppConfig>();
                var procoreConfig = appConfig.Procore;

                return new DefaultProcoreApiClientFactory().Create(new ProcoreApiClientOptions
                {
                    ClientId = procoreConfig.ClientId,
                    ClientSecret = procoreConfig.ClientSecret,
                    IsSandbox = procoreConfig.IsSandbox
                });
            });
        }

        public async Task Configure(IGlobalConfiguration hangfireConfig)
        {

        }
    }
}