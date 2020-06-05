using Microsoft.EntityFrameworkCore.Migrations;

namespace apiTest1.Migrations
{
    public partial class latestchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKeyId",
                table: "UserData",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKeyId",
                table: "UserData");
        }
    }
}
