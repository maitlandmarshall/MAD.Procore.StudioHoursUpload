using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload.Data
{
    [Keyless]
    public class vwStudioHoursByRegionAndCountry
    {
        public int EmailCount { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public int DailyStudioHours { get; set; }
        public int WeeklyStudioHours { get; set; }
    }
}
