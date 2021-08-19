using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.Procore.StudioHoursUpload
{
    public class ProcoreConfig
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool IsSandbox { get; set; } = false;
    }
}
