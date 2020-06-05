using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace apiTest1.Migrations
{
    public partial class davicsadd3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "UserServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(maxLength: 60, nullable: false),
                    ApiKeyId = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServices", x => x.Id);
                    table.UniqueConstraint("AK_UserServices_ServiceName", x => x.ServiceName);
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
                    ServiceName = table.Column<string>(maxLength: 60, nullable: false),
                    ServiceData = table.Column<decimal>(nullable: false),
                    DataInsertDat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_UserServices_ServiceName",
                        column: x => x.ServiceName,
                        principalTable: "UserServices",
                        principalColumn: "ServiceName",
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
                name: "IX_UserData_ServiceName",
                table: "UserData",
                column: "ServiceName");

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
                name: "UserData");

            migrationBuilder.DropTable(
                name: "UserServices");

            migrationBuilder.DropTable(
                name: "RegisterUser");
        }
    }
}
