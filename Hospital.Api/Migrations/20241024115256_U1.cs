using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ServiceEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_HealthProfiles_HealthProfileId",
                        column: x => x.HealthProfileId,
                        principalTable: "HealthProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_HealthServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "HealthServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HealthProfileId",
                table: "Bookings",
                column: "HealthProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings",
                column: "ServiceId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "location_districts");

            migrationBuilder.DropTable(
                name: "location_provinces");

            migrationBuilder.DropTable(
                name: "location_wards");

            migrationBuilder.DropTable(
                name: "mcs_refresh_tokens");

            migrationBuilder.DropTable(
                name: "mcs_sequences");

            migrationBuilder.DropTable(
                name: "mcs_system_configurations");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "perm_roles_actions");

            migrationBuilder.DropTable(
                name: "perm_users_branches");

            migrationBuilder.DropTable(
                name: "perm_users_roles");

            migrationBuilder.DropTable(
                name: "QueueItems");

            migrationBuilder.DropTable(
                name: "ServiceTimeRules");

            migrationBuilder.DropTable(
                name: "SocialNetworks");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "perm_actions");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "perm_roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "HealthProfiles");

            migrationBuilder.DropTable(
                name: "HealthServices");

            migrationBuilder.DropTable(
                name: "FacilitySpecialty");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "HealthFacilities");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "FacilityCategories");
        }
    }
}
