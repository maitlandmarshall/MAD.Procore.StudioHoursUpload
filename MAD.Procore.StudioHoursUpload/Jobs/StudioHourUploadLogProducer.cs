using Hangfire;
using MAD.Integration.Common.Jobs;
using MAD.Procore.StudioHoursUpload.Data;
using MAD.Procore.StudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAD.Procore.StudioHoursUpload.Jobs
{
    public class StudioHourUploadLogProducer
    {
        private readonly StudioHourClient studioHourClient;
        private readonly StudioHourDbContext studioHourDbContext;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly ProcoreConfig procoreConfig;

        public StudioHourUploadLogProducer(StudioHourClient studioHourClient, StudioHourDbContext studioHourDbContext, IBackgroundJobClient backgroundJobClient, ProcoreConfig procoreConfig)
        {
            this.studioHourClient = studioHourClient;
            this.studioHourDbContext = studioHourDbContext;
            this.backgroundJobClient = backgroundJobClient;
            this.procoreConfig = procoreConfig;
        }

        public async Task ProduceStudioHoursUploadLogs()
        {
            var studioProjects = await this.studioHourDbContext.StudioProject
                .Where(y => y.Region == this.procoreConfig.Name)
                .ToListAsync();

            //Retrieve last processed studio log and pass processed date time to GetStudioHours method
            var lastStudioLog = await this.studioHourDbContext.StudioHourUploadLog
                .OrderByDescending(x => x.ProcessedDate)
                .FirstOrDefaultAsync(x => x.Region == this.procoreConfig.Name && x.ProcessedDate.HasValue);

            var studioHours = await this.studioHourClient.GetStudioHours(lastStudioLog?.ProcessedDate);
            var today = this.GetTodayUtc();

            foreach (var sh in studioHours)
            {
                // For each studio hour row, a staging table record should be generated & eventually processed
                // we have to know which project to associate with the studio hour record
                var project = this.GetStudioProject(sh.Region, sh.Country, studioProjects);
                this.backgroundJobClient.Enqueue<StudioHourUploadLogProducer>(y => y.ValidateAndCreateLog(project.ProjectId, sh.Region, sh.Country, today, sh.EmailCount));
            }
        }

        [DisableIdenticalQueuedItems(IncludeFailedJobs = true)]
        public async Task ValidateAndCreateLog(int projectId, string region, string country, DateTime date, int numberOfWorkers)
        {
            var lastLog = await this.studioHourDbContext.StudioHourUploadLog.FindAsync(projectId, region, country, date);

            if (lastLog != null)
                return;

            var isWeekend =
                date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;

            var stagingTable = new StudioHourUploadLog
            {
                ProjectId = projectId,
                Country = country,
                Region = region,
                Date = date,
                HoursPerWorker = isWeekend ? 0 : 8,
                NumberOfWorkers = numberOfWorkers
            };

            this.studioHourDbContext.StudioHourUploadLog.Add(stagingTable);
            await this.studioHourDbContext.SaveChangesAsync();

            this.backgroundJobClient.Enqueue<StudioHourUploadLogConsumer>(y => y.ProcessStudioHourUpload(projectId, region, country, date));
        }

        private DateTime GetTodayUtc()
        {
            var today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Utc);

            return today;
        }

        private StudioProject GetStudioProject(string region, string country, IEnumerable<StudioProject> studioProjects)
        {
            var result = studioProjects.SingleOrDefault(y => y.Region == region && y.Country == country)
                ?? studioProjects.SingleOrDefault(y => y.Region == region && string.IsNullOrEmpty(y.Country));

            if (result is null)
            {
                throw new StudioProjectMappingNotFoundException($"Could not find a {nameof(StudioProject)} mapping for {region} {country}.");
            }

            return result;
        }
    }
}
