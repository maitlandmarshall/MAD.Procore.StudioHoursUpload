﻿// <auto-generated />
using System;
using MAD.Procore.RecurringStudioHoursUpload.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAD.Procore.RecurringStudioHoursUpload.Migrations
{
    [DbContext(typeof(StudioHourDbContext))]
    partial class StudioHourDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAD.Procore.RecurringStudioHoursUpload.Data.StudioHourUploadLog", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Region")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HoursPerWorker")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfWorkers")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ProcessedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("ProcessedManpowerLogId")
                        .HasColumnType("bigint");

                    b.HasKey("ProjectId", "Region", "Country", "Date");

                    b.ToTable("StudioHourUploadLog");
                });

            modelBuilder.Entity("MAD.Procore.RecurringStudioHoursUpload.Data.StudioProject", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("ProjectId");

                    b.ToTable("StudioProject");
                });
#pragma warning restore 612, 618
        }
    }
}
