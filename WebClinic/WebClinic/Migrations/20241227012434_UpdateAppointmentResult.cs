using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebClinic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointmentResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordDate",
                table: "AppointmentResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecordDate",
                table: "AppointmentResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
