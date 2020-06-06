using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace apiTest1.Migrations
{
    public partial class updates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_UserServices_ServiceName",
                table: "UserData");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserServices_ServiceName",
                table: "UserServices");

            migrationBuilder.DropIndex(
                name: "IX_UserData_ServiceName",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "UserData");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "UserData",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserServices_DeviceId",
                table: "UserServices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_DeviceId",
                table: "UserData",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_UserServices_DeviceId",
                table: "UserData",
                column: "DeviceId",
                principalTable: "UserServices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_UserServices_DeviceId",
                table: "UserData");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserServices_DeviceId",
                table: "UserServices");

            migrationBuilder.DropIndex(
                name: "IX_UserData_DeviceId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "UserData");

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "UserData",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserServices_ServiceName",
                table: "UserServices",
                column: "ServiceName");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_ServiceName",
                table: "UserData",
                column: "ServiceName");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_UserServices_ServiceName",
                table: "UserData",
                column: "ServiceName",
                principalTable: "UserServices",
                principalColumn: "ServiceName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
