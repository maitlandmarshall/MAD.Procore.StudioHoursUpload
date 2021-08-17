using Hangfire;
using MAD.API.Procore;
using MAD.Integration.Common.Jobs;
using MAD.Procore.RecurringStudioHoursUpload.Data;
using MAD.Procore.RecurringStudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Procore.RecurringStudioHoursUpload.Jobs
{
    public class StudioHourUploadLogConsumer
    {
        private readonly StudioHourDbContext studioHourDbContext;
        private readonly ProcoreApiClient procoreApiClient;
        private readonly IBackgroundJobClient backgroundJobClient;

        public StudioHourUploadLogConsumer(StudioHourDbContext studioHourDbContext, ProcoreApiClient procoreApiClient, IBackgroundJobClient backgroundJobClient)
        {
            this.studioHourDbContext = studioHourDbContext;
            this.procoreApiClient = procoreApiClient;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task EnqueueUnprocessedStudioHourUploadLogs()
        {
            var uploadLogs = await this.studioHourDbContext.StudioHourUploadLog
                .Where(y => y.ProcessedDate.HasValue == false)
                .ToListAsync();

            foreach (var ul in uploadLogs)
            {
                this.backgroundJobClient.Enqueue<StudioHourUploadLogConsumer>(y => y.ProcessStudioHourUpload(ul.ProjectId, ul.Region, ul.Country, ul.Date));
            }
        }

        [DisableIdenticalQueuedItems(IncludeFailedJobs = true)]
        public async Task ProcessStudioHourUpload(int projectId, string region, string country, DateTime date)
        {
            var uploadLog = await this.studioHourDbContext.StudioHourUploadLog.FindAsync(projectId, region, country, date);

            try
            {
                if (uploadLog.ProcessedDate.HasValue)
                    return;

            }
            catch(Exception ex)
            {
                uploadLog.Error = ex.ToString();
            }
            finally
            {

            }
        }

        
    }
}
