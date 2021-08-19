using Hangfire;
using MAD.API.Procore;
using MAD.API.Procore.Endpoints.ManpowerLogs;
using MAD.API.Procore.Endpoints.ManpowerLogs.Models;
using MAD.API.Procore.Requests;
using MAD.Integration.Common.Jobs;
using MAD.Procore.StudioHoursUpload.Data;
using MAD.Procore.StudioHoursUpload.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Procore.StudioHoursUpload.Jobs
{
    public class StudioHourUploadLogConsumer
    {
        private readonly StudioHourDbContext studioHourDbContext;
        private readonly ProcoreApiClient procoreApiClient;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly ProcoreConfig procoreConfig;

        public StudioHourUploadLogConsumer(StudioHourDbContext studioHourDbContext, ProcoreApiClient procoreApiClient, IBackgroundJobClient backgroundJobClient, ProcoreConfig procoreConfig)
        {
            this.studioHourDbContext = studioHourDbContext;
            this.procoreApiClient = procoreApiClient;
            this.backgroundJobClient = backgroundJobClient;
            this.procoreConfig = procoreConfig;
        }

        public async Task EnqueueUnprocessedStudioHourUploadLogs()
        {
            var uploadLogs = await this.studioHourDbContext.StudioHourUploadLog
                .Where(y => y.ProcessedDate.HasValue == false && string.IsNullOrWhiteSpace(y.Error) && y.Region == procoreConfig.Name)
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

            if (uploadLog.ProcessedDate.HasValue)
                return;

            try
            {
                if (uploadLog.HoursPerWorker > 0)
                {
                    var createLogRequest = new CreateManpowerLogRequest
                    {
                        ProjectId = projectId,
                        Body = JsonConvert.SerializeObject(new
                        {
                            manpower_log = new
                            {
                                date = date.ToString("yyyy-MM-dd"),
                                num_workers = uploadLog.NumberOfWorkers,
                                num_hours = uploadLog.HoursPerWorker,
                                notes = $"{region} {country} Office Hours"
                            }
                        })
                    };

                    var result = await createLogRequest.GetResponse(this.procoreApiClient);
                    uploadLog.ProcessedManpowerLogId = result.Result.Id;
                }

                uploadLog.ProcessedDate = DateTime.Now;
            }
            catch(Exception ex)
            {
                uploadLog.Error = ex.ToString();
            }
            finally
            {
                await this.studioHourDbContext.SaveChangesAsync();
            }
        }
    }
}
