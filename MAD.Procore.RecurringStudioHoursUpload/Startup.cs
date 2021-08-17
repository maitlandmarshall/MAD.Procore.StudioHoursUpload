using Hangfire;
using MAD.API.Procore;
using MAD.Integration.Common.Settings;
using MAD.Procore.RecurringStudioHoursUpload.Data;
using MAD.Procore.RecurringStudioHoursUpload.Jobs;
using MAD.Procore.RecurringStudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
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

            serviceDescriptors.AddDbContext<StudioHourDbContext>(optionsAction: (svc, opt) => opt.UseSqlServer(svc.GetRequiredService<AppConfig>().ConnectionString));
            serviceDescriptors.AddScoped<RecurringStudioHourUploadJob>();
            serviceDescriptors.AddTransient<NamelyDbConnectionFactory>();
            serviceDescriptors.AddTransient<StudioHourClient>();
            serviceDescriptors.AddTransient<StudioProjectClient>();
        }

        public async Task Configure()
        {
            
        }
    }
}