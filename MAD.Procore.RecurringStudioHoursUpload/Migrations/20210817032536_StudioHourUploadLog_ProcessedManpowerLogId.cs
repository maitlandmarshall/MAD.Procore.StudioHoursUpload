using Microsoft.EntityFrameworkCore.Migrations;

namespace MAD.Procore.RecurringStudioHoursUpload.Migrations
{
    public partial class StudioHourUploadLog_ProcessedManpowerLogId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProcessedManpowerLogId",
                table: "StudioHourUploadLog",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedManpowerLogId",
                table: "StudioHourUploadLog");
        }
    }
}
