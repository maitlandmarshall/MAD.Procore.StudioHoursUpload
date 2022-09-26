using Hangfire;
using MAD.API.Procore;
using MAD.Integration.Common.Jobs;
using MAD.Integration.Common.Settings;
using MAD.Procore.StudioHoursUpload.Data;
using MAD.Procore.StudioHoursUpload.Jobs;
using MAD.Procore.StudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MAD.Procore.StudioHoursUpload
{
    internal class Startup
    {        
        public void ConfigureServices(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddIntegrationSettings<AppConfig>();
            serviceDescriptors.AddSingleton(svc => svc.GetRequiredService<AppConfig>().Procore);
            serviceDescriptors.AddSingleton(svc =>
            {
                var config = svc.GetRequiredService<ProcoreConfig>();
                var apiClientOptions = new ProcoreApiClientOptions
                {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    IsSandbox = config.IsSandbox
                };

                var httpClient = new DefaultProcoreApiClientFactory().CreateHttpClient(apiClientOptions);
                
                if (string.IsNullOrWhiteSpace(config.CompanyId) == false)
                {
                    httpClient.DefaultRequestHeaders.Add("Procore-Company-Id", config.CompanyId);
                }

                return new ProcoreApiClient(httpClient, apiClientOptions);
            });

            serviceDescriptors.AddDbContext<StudioHourDbContext>(optionsAction: (svc, opt) => opt.UseSqlServer(svc.GetRequiredService<AppConfig>().ConnectionString));
            serviceDescriptors.AddScoped<StudioHourUploadLogConsumer>();
            serviceDescriptors.AddScoped<StudioHourUploadLogProducer>();
            serviceDescriptors.AddTransient<NamelyDbConnectionFactory>();
            serviceDescriptors.AddTransient<StudioHourClient>();
        }

        public static void PostConfigure(StudioHourDbContext dbContext, ProcoreConfig procoreConfig, IRecurringJobManager recurringJobManager)
        {
            dbContext.Database.Migrate();

            var queueName = procoreConfig.Name.ToLower().Replace(" ", "_");

            JobFactory.CreateRecurringJob<StudioHourUploadLogProducer>($"{procoreConfig.Name}.{nameof(StudioHourUploadLogProducer)}.ProduceStudioHoursUploadLogs", y => y.ProduceStudioHoursUploadLogs(), queue: queueName);
            JobFactory.CreateRecurringJob<StudioHourUploadLogConsumer>($"{procoreConfig.Name}.{nameof(StudioHourUploadLogConsumer)}.EnqueueUnprocessedStudioHourUploadLogs", y => y.EnqueueUnprocessedStudioHourUploadLogs(), queue: queueName);
        }

        public void Configure(IGlobalConfiguration globalConfiguration, ProcoreConfig procoreConfig, HangfireConfig hangfireConfig)
        {
            var queueName = procoreConfig.Name.ToLower().Replace(" ", "_");

            globalConfiguration.UseFilter<DelegatedQueueAttribute>(new DelegatedQueueAttribute(queueName));
            hangfireConfig.Queues = new[] { queueName, "default" };

#if DEBUG
            globalConfiguration.UseFilter<DisableConcurrentExecutionAttribute>(new DisableConcurrentExecutionAttribute(60 * 5));
#endif
        }
    }
}