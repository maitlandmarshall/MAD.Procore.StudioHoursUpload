using Hangfire;
using MAD.Integration.Common;
using MAD.Integration.Common.Jobs;
using MAD.Procore.RecurringStudioHoursUpload.Data;
using MAD.Procore.RecurringStudioHoursUpload.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MAD.Procore.RecurringStudioHoursUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            MigrateAndStartRecurringJobs(host.Services);
            host.Run();
        }

        static void MigrateAndStartRecurringJobs(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<StudioHourDbContext>();
            var jobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
            var procoreConfig = serviceProvider.GetRequiredService<ProcoreConfig>();

            dbContext.Database.Migrate();

            JobFactory.CreateRecurringJob<StudioHourUploadLogProducer>($"{procoreConfig.Name}.{nameof(StudioHourUploadLogProducer)}.ProduceStudioHoursUploadLogs", y => y.ProduceStudioHoursUploadLogs());
            JobFactory.CreateRecurringJob<StudioHourUploadLogConsumer>($"{procoreConfig.Name}.{nameof(StudioHourUploadLogConsumer)}.EnqueueUnprocessedStudioHourUploadLogs", y => y.EnqueueUnprocessedStudioHourUploadLogs());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            IntegrationHost.CreateDefaultBuilder(args)
                .UseAspNetCore()
                .UseAppInsights()
                .UseStartup<Startup>();
    }
}
