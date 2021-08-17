using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAD.Procore.RecurringStudioHoursUpload.Migrations
{
    public partial class StudioHourUploadLog_CompositePrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioHourUploadLog",
                table: "StudioHourUploadLog");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudioHourUploadLog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "StudioHourUploadLog",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "StudioHourUploadLog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioHourUploadLog",
                table: "StudioHourUploadLog",
                columns: new[] { "ProjectId", "Region", "Country", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudioHourUploadLog",
                table: "StudioHourUploadLog");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Date",
                table: "StudioHourUploadLog",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "StudioHourUploadLog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudioHourUploadLog",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudioHourUploadLog",
                table: "StudioHourUploadLog",
                column: "Id");
        }
    }
}
