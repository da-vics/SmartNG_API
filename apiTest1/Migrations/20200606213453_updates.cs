using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartNG.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldMasterKey",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessKey = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldMasterKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterUser",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    PassWordHash = table.Column<string>(maxLength: 100, nullable: false),
                    ApiKeyId = table.Column<string>(maxLength: 100, nullable: false),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    HomeAddress = table.Column<string>(maxLength: 300, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterUser", x => x.Email);
                    table.UniqueConstraint("AK_RegisterUser_ApiKeyId", x => x.ApiKeyId);
                });

            migrationBuilder.CreateTable(
                name: "SetupModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(maxLength: 60, nullable: false),
                    ApiKeyId = table.Column<string>(maxLength: 100, nullable: false),
                    DeviceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServices", x => x.Id);
                    table.UniqueConstraint("AK_UserServices_DeviceId", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_UserServices_RegisterUser_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "RegisterUser",
                        principalColumn: "ApiKeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<Guid>(nullable: false),
                    ServiceData = table.Column<decimal>(nullable: false),
                    DataInsertDat = table.Column<DateTime>(nullable: false),
                    ApiKeyId = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_UserServices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "UserServices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterUser_ApiKeyId",
                table: "RegisterUser",
                column: "ApiKeyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisterUser_Email",
                table: "RegisterUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_DeviceId",
                table: "UserData",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_ApiKeyId",
                table: "UserServices",
                column: "ApiKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_ServiceName",
                table: "UserServices",
                column: "ServiceName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldMasterKey");

            migrationBuilder.DropTable(
                name: "SetupModels");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "UserServices");

            migrationBuilder.DropTable(
                name: "RegisterUser");
        }
    }
}
