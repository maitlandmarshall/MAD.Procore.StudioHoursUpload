using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public ProcoreConfig Procore { get; set; }
    }
}
