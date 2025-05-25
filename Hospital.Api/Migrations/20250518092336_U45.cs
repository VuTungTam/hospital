using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "perm_employee_action_map");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "perm_employee_action_map",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsExclude = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_employee_action_map", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perm_employee_action_map_mcs_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "mcs_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_perm_employee_action_map_perm_actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "perm_actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_action_map_ActionId",
                table: "perm_employee_action_map",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_action_map_EmployeeId",
                table: "perm_employee_action_map",
                column: "EmployeeId");
        }
    }
}
