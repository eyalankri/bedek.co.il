using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BuildingNumber = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceName = table.Column<string>(nullable: false),
                    WarrantyPeriodInMonths = table.Column<int>(nullable: false),
                    IsEnable = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "char(64)", nullable: false),
                    Phone1 = table.Column<string>(type: "varchar(20)", nullable: false),
                    Phone2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    IdentityCardId = table.Column<int>(nullable: false),
                    IsAcceptEmails = table.Column<bool>(nullable: false),
                    DateRegistered = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Company = table.Column<string>(maxLength: 300, nullable: true),
                    Comment = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingId = table.Column<int>(nullable: false),
                    ApartmentNumber = table.Column<int>(nullable: false),
                    DateOfEntrance = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartments_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apartments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInHandymanInBuilding",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    BuildingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInHandymanInBuilding", x => new { x.BuildingId, x.UserId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_ServiceInHandymanInBuilding_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceInHandymanInBuilding_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceInHandymanInBuilding_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInUser", x => new { x.ServiceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ServiceInUser_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceInUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserInRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentDocs",
                columns: table => new
                {
                    ApartmentDocId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApartmentId = table.Column<int>(nullable: false),
                    DocDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    FileContentType = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentDocs", x => x.ApartmentDocId);
                    table.ForeignKey(
                        name: "FK_ApartmentDocs_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "Handyman" },
                    { 3, "Resident" }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "ServiceId", "IsEnable", "ServiceName", "WarrantyPeriodInMonths" },
                values: new object[] { 1, true, "אינסטלציה", 60 });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Comment", "Company", "DateRegistered", "Email", "FirstName", "IdentityCardId", "IsAcceptEmails", "IsDeleted", "LastName", "Password", "Phone1", "Phone2" },
                values: new object[,]
                {
                    { new Guid("c73633bc-f8df-4660-b3ed-51e9479df820"), null, null, new DateTime(2019, 8, 11, 20, 3, 54, 83, DateTimeKind.Local), "eyal.ank@gmail.com", "Eyal", 33913450, false, false, "Ankri", "744fd6f1e1f3bc2d2a023c27f4bcc1a12523767d55de7508c0b21a160ab1fdbf", "054-6680240", null },
                    { new Guid("c2fef337-4858-4c8b-899e-e6aacb2da339"), null, null, new DateTime(2019, 8, 11, 20, 3, 54, 85, DateTimeKind.Local), "carmelm@maozdaniel.co.il", "Carmel", 0, false, false, "Malca", "d37d6f4d78542a3131bf5977d2ebb36346ff20bf0fc9365b0b7e28b1fbcdb89b", "054-2446997", null }
                });

            migrationBuilder.InsertData(
                table: "UserInRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, new Guid("c73633bc-f8df-4660-b3ed-51e9479df820") });

            migrationBuilder.InsertData(
                table: "UserInRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, new Guid("c2fef337-4858-4c8b-899e-e6aacb2da339") });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentDocs_ApartmentId",
                table: "ApartmentDocs",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInHandymanInBuilding_ServiceId",
                table: "ServiceInHandymanInBuilding",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInHandymanInBuilding_UserId",
                table: "ServiceInHandymanInBuilding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInUser_UserId",
                table: "ServiceInUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_UserId",
                table: "UserInRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentDocs");

            migrationBuilder.DropTable(
                name: "ServiceInHandymanInBuilding");

            migrationBuilder.DropTable(
                name: "ServiceInUser");

            migrationBuilder.DropTable(
                name: "UserInRoles");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
