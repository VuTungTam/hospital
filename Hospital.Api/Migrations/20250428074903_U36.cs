using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_service_time_rules_tbl_health_services_HealthServiceId",
                table: "tbl_service_time_rules");

            migrationBuilder.DropIndex(
                name: "IX_tbl_service_time_rules_HealthServiceId",
                table: "tbl_service_time_rules");

            migrationBuilder.DropColumn(
                name: "HealthServiceId",
                table: "tbl_service_time_rules");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HealthServiceId",
                table: "tbl_service_time_rules",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_service_time_rules_HealthServiceId",
                table: "tbl_service_time_rules",
                column: "HealthServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_service_time_rules_tbl_health_services_HealthServiceId",
                table: "tbl_service_time_rules",
                column: "HealthServiceId",
                principalTable: "tbl_health_services",
                principalColumn: "Id");
        }
    }
}
