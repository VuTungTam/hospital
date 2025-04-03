using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_zones_tbl_health_facilities_HealthFacilityId",
                table: "tbl_zones");

            migrationBuilder.DropIndex(
                name: "IX_tbl_zones_HealthFacilityId",
                table: "tbl_zones");

            migrationBuilder.DropColumn(
                name: "HealthFacilityId",
                table: "tbl_zones");

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "tbl_zones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_zones_FacilityId",
                table: "tbl_zones",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_zones_tbl_health_facilities_FacilityId",
                table: "tbl_zones",
                column: "FacilityId",
                principalTable: "tbl_health_facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_zones_tbl_health_facilities_FacilityId",
                table: "tbl_zones");

            migrationBuilder.DropIndex(
                name: "IX_tbl_zones_FacilityId",
                table: "tbl_zones");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_zones");

            migrationBuilder.AddColumn<long>(
                name: "HealthFacilityId",
                table: "tbl_zones",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_zones_HealthFacilityId",
                table: "tbl_zones",
                column: "HealthFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_zones_tbl_health_facilities_HealthFacilityId",
                table: "tbl_zones",
                column: "HealthFacilityId",
                principalTable: "tbl_health_facilities",
                principalColumn: "Id");
        }
    }
}
