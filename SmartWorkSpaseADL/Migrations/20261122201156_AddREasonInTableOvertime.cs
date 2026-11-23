using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWorkSpaseADL.Migrations
{
    /// <inheritdoc />
    public partial class AddREasonInTableOvertime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RatePerMinute",
                table: "OvertimeSessions",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "OvertimeSessions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "OvertimeSessions");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatePerMinute",
                table: "OvertimeSessions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);
        }
    }
}
