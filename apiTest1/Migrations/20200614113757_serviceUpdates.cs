using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartNG.Migrations
{
    public partial class serviceUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "DeviceStatus",
                table: "UserServices",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DeviceType",
                table: "UserServices",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceStatus",
                table: "UserServices");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "UserServices");
        }
    }
}
