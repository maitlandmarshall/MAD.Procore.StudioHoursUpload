using MAD.API.Procore;
using MAD.Procore.RecurringStudioHoursUpload.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Procore.RecurringStudioHoursUpload.Jobs
{
    public class RecurringStudioHourUploadJob
    {
        private readonly StudioHourClient studioHourClient;
        private readonly StudioProjectClient studioProjectClient;
        private readonly ProcoreApiClient procoreApiClient;

        public RecurringStudioHourUploadJob(StudioHourClient studioHourClient, StudioProjectClient studioProjectClient, ProcoreApiClient procoreApiClient)
        {
            this.studioHourClient = studioHourClient;
            this.studioProjectClient = studioProjectClient;
            this.procoreApiClient = procoreApiClient;
        }

        public async Task EnqueueStudioProjectsForDailyUpload()
        {

        }
    }
}
