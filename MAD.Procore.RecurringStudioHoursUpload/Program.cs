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
            dbContext.Database.Migrate();

            // Force Hangfire to initialize by injecting the background job client
            var jobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
            JobFactory.CreateRecurringJob<StudioHourUploadLogProducer>(nameof(StudioHourUploadLogProducer), y => y.ProduceStudioHoursUploadLogs());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            IntegrationHost.CreateDefaultBuilder(args)
                .UseAspNetCore()
                .UseAppInsights()
                .UseStartup<Startup>();
    }
}
