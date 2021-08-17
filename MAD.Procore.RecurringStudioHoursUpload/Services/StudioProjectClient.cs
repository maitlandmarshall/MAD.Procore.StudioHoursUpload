using MAD.Procore.RecurringStudioHoursUpload.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Procore.RecurringStudioHoursUpload.Services
{
    public class StudioProjectClient
    {
        private readonly StudioHourDbContext studioHourDbContext;

        public StudioProjectClient(StudioHourDbContext studioHourDbContext)
        {
            this.studioHourDbContext = studioHourDbContext;
        }

        public async Task<IEnumerable<StudioProject>> GetStudioProjects()
        {
            return await this.studioHourDbContext.StudioProject.ToListAsync();
        }
    }
}
