﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload.Data
{
    public class StudioProject
    {
        [Key]
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Region { get; set; }

        [MaxLength(200)]
        public string Country { get; set; }
    }
}
