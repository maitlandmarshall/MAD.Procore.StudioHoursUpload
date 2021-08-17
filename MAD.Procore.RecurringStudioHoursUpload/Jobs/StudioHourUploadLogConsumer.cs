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

        public StudioHourUploadLogConsumer(StudioHourDbContext studioHourDbContext, ProcoreApiClient procoreApiClient)
        {
            this.studioHourDbContext = studioHourDbContext;
            this.procoreApiClient = procoreApiClient;
        }

        

        [DisableIdenticalQueuedItems(IncludeFailedJobs = true)]
        public async Task ProcessStudioHourUpload(int uploadLogId)
        {
            var uploadLog = await this.studioHourDbContext.StudioHourUploadLog.FindAsync(uploadLogId);

            throw new NotImplementedException();
        }

        
    }
}
