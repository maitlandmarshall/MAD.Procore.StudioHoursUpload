using Hangfire;
using MAD.API.Procore;
using MAD.Integration.Common.Jobs;
using MAD.Integration.Common.Settings;
using MAD.Procore.StudioHoursUpload.Data;
using MAD.Procore.StudioHoursUpload.Jobs;
using MAD.Procore.StudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MAD.Procore.StudioHoursUpload
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddIntegrationSettings<AppConfig>();
            serviceDescriptors.AddSingleton(svc => svc.GetRequiredService<AppConfig>().Procore);
            serviceDescriptors.AddSingleton<ProcoreApiClient>(svc =>
            {
                var procoreConfig = svc.GetRequiredService<ProcoreConfig>();

                return new DefaultProcoreApiClientFactory().Create(new ProcoreApiClientOptions
                {
                    ClientId = procoreConfig.ClientId,
                    ClientSecret = procoreConfig.ClientSecret,
                    IsSandbox = procoreConfig.IsSandbox
                });
            });

            serviceDescriptors.AddDbContext<StudioHourDbContext>(optionsAction: (svc, opt) => opt.UseSqlServer(svc.GetRequiredService<AppConfig>().ConnectionString));
            serviceDescriptors.AddScoped<StudioHourUploadLogConsumer>();
            serviceDescriptors.AddScoped<StudioHourUploadLogProducer>();
            serviceDescriptors.AddTransient<NamelyDbConnectionFactory>();
            serviceDescriptors.AddTransient<StudioHourClient>();
        }

        public void Configure(IGlobalConfiguration globalConfiguration, ProcoreConfig procoreConfig, HangfireConfig hangfireConfig)
        {
            var queueName = procoreConfig.Name.ToLower().Replace(" ", "_");

            globalConfiguration.UseFilter<DelegatedQueueAttribute>(new DelegatedQueueAttribute(queueName));
            hangfireConfig.Queues = new[] { queueName };

#if DEBUG
            globalConfiguration.UseFilter<DisableConcurrentExecutionAttribute>(new DisableConcurrentExecutionAttribute(60 * 5));
#endif
        }
    }
}