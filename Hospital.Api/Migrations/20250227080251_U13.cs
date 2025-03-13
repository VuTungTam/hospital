using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "perm_users_branches");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "HealthFacilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "HealthFacilities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    Address = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    FoundingDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perm_users_branches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_users_branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perm_users_branches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_perm_users_branches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_perm_users_branches_BranchId",
                table: "perm_users_branches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_users_branches_UserId",
                table: "perm_users_branches",
                column: "UserId");
        }
    }
}
