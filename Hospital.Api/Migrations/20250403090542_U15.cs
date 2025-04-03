using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "tbl_health_services");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_DoctorId",
                table: "tbl_health_services",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_mcs_doctors_DoctorId",
                table: "tbl_health_services",
                column: "DoctorId",
                principalTable: "mcs_doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_mcs_doctors_DoctorId",
                table: "tbl_health_services");

            migrationBuilder.DropIndex(
                name: "IX_tbl_health_services_DoctorId",
                table: "tbl_health_services");

            migrationBuilder.AddColumn<long>(
                name: "ZoneId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
