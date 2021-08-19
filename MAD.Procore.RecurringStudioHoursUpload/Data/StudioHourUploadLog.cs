using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload.Data
{
    public class StudioHourUploadLog
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Region { get; set; }

        [Required]
        [MaxLength(200)]
        public string Country { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public int NumberOfWorkers { get; set; }

        [Required]
        public int HoursPerWorker { get; set; }

        public DateTimeOffset? ProcessedDate { get; set; }
        public long? ProcessedManpowerLogId { get; set; }

        public string Error { get; set; }
    }
}
