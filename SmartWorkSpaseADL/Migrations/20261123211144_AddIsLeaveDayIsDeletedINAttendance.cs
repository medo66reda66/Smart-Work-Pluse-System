using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWorkSpaseADL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLeaveDayIsDeletedINAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AttendanceRecords",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeaveDay",
                table: "AttendanceRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "IsLeaveDay",
                table: "AttendanceRecords");
        }
    }
}
