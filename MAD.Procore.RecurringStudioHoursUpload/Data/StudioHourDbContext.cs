using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MAD.Procore.RecurringStudioHoursUpload.Data
{
    public class StudioHourDbContext : DbContext
    {
        public StudioHourDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<StudioProject> StudioProject { get; set; }
        public DbSet<StudioHourUploadLog> StudioHourUploadLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudioHourUploadLog>(cfg =>
            {
                cfg.HasKey(y => new
                {
                    y.ProjectId,
                    y.Region,
                    y.Country,
                    y.Date
                });
            });
        }
    }
}
