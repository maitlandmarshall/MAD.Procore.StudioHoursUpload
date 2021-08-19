using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAD.Procore.RecurringStudioHoursUpload.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudioHourUploadLog",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    NumberOfWorkers = table.Column<int>(type: "int", nullable: false),
                    HoursPerWorker = table.Column<int>(type: "int", nullable: false),
                    ProcessedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProcessedManpowerLogId = table.Column<long>(type: "bigint", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioHourUploadLog", x => new { x.ProjectId, x.Region, x.Country, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "StudioProject",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioProject", x => x.ProjectId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudioHourUploadLog");

            migrationBuilder.DropTable(
                name: "StudioProject");
        }
    }
}
