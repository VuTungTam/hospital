using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropIndex(
                name: "IX_tbl_health_services_FacilitySpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "FacilitySpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SpecialtyId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_FacilityId",
                table: "tbl_health_services",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_SpecialtyId",
                table: "tbl_health_services",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_tbl_health_facilities_FacilityId",
                table: "tbl_health_services",
                column: "FacilityId",
                principalTable: "tbl_health_facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_tbl_specialties_SpecialtyId",
                table: "tbl_health_services",
                column: "SpecialtyId",
                principalTable: "tbl_specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_tbl_health_facilities_FacilityId",
                table: "tbl_health_services");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_tbl_specialties_SpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropIndex(
                name: "IX_tbl_health_services_FacilityId",
                table: "tbl_health_services");

            migrationBuilder.DropIndex(
                name: "IX_tbl_health_services_SpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.AddColumn<long>(
                name: "FacilitySpecialtyId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_FacilitySpecialtyId",
                table: "tbl_health_services",
                column: "FacilitySpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services",
                column: "FacilitySpecialtyId",
                principalTable: "tbl_facility_specialty",
                principalColumn: "Id");
        }
    }
}
