using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_bookings_FacilityId",
                table: "tbl_bookings",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_bookings_tbl_health_facilities_FacilityId",
                table: "tbl_bookings",
                column: "FacilityId",
                principalTable: "tbl_health_facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_bookings_tbl_health_facilities_FacilityId",
                table: "tbl_bookings");

            migrationBuilder.DropIndex(
                name: "IX_tbl_bookings_FacilityId",
                table: "tbl_bookings");
        }
    }
}
