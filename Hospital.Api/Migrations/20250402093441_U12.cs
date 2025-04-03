using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZoneSpecialty_tbl_specialties_SpecialtyId",
                table: "ZoneSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_ZoneSpecialty_tbl_zones_ZoneId",
                table: "ZoneSpecialty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZoneSpecialty",
                table: "ZoneSpecialty");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "tbl_health_services");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "tbl_health_services");

            migrationBuilder.RenameTable(
                name: "ZoneSpecialty",
                newName: "tbl_zone_specialty");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneSpecialty_ZoneId",
                table: "tbl_zone_specialty",
                newName: "IX_tbl_zone_specialty_ZoneId");

            migrationBuilder.RenameIndex(
                name: "IX_ZoneSpecialty_SpecialtyId",
                table: "tbl_zone_specialty",
                newName: "IX_tbl_zone_specialty_SpecialtyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_zone_specialty",
                table: "tbl_zone_specialty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_zone_specialty_tbl_specialties_SpecialtyId",
                table: "tbl_zone_specialty",
                column: "SpecialtyId",
                principalTable: "tbl_specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_zone_specialty_tbl_zones_ZoneId",
                table: "tbl_zone_specialty",
                column: "ZoneId",
                principalTable: "tbl_zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_zone_specialty_tbl_specialties_SpecialtyId",
                table: "tbl_zone_specialty");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_zone_specialty_tbl_zones_ZoneId",
                table: "tbl_zone_specialty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_zone_specialty",
                table: "tbl_zone_specialty");

            migrationBuilder.RenameTable(
                name: "tbl_zone_specialty",
                newName: "ZoneSpecialty");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_zone_specialty_ZoneId",
                table: "ZoneSpecialty",
                newName: "IX_ZoneSpecialty_ZoneId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_zone_specialty_SpecialtyId",
                table: "ZoneSpecialty",
                newName: "IX_ZoneSpecialty_SpecialtyId");

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsUnread",
                table: "mcs_notifications",
                type: "BIT",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "BIT(1)",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZoneSpecialty",
                table: "ZoneSpecialty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneSpecialty_tbl_specialties_SpecialtyId",
                table: "ZoneSpecialty",
                column: "SpecialtyId",
                principalTable: "tbl_specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZoneSpecialty_tbl_zones_ZoneId",
                table: "ZoneSpecialty",
                column: "ZoneId",
                principalTable: "tbl_zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
