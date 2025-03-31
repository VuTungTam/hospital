using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropTable(
                name: "tbl_service_doctor");

            migrationBuilder.AlterColumn<long>(
                name: "FacilitySpecialtyId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "DoctorId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddColumn<long>(
                name: "ZoneId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "tbl_feedbacks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "tbl_bookings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ZoneId",
                table: "tbl_bookings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "mcs_employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ZoneId",
                table: "mcs_employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FacilityId",
                table: "mcs_doctors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services",
                column: "FacilitySpecialtyId",
                principalTable: "tbl_facility_specialty",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_feedbacks");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_bookings");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "tbl_bookings");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "mcs_doctors");

            migrationBuilder.AlterColumn<long>(
                name: "FacilitySpecialtyId",
                table: "tbl_health_services",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_service_doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_service_doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_service_doctor_mcs_doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "mcs_doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_service_doctor_tbl_health_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "tbl_health_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_service_doctor_DoctorId",
                table: "tbl_service_doctor",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_service_doctor_ServiceId",
                table: "tbl_service_doctor",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                table: "tbl_health_services",
                column: "FacilitySpecialtyId",
                principalTable: "tbl_facility_specialty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
