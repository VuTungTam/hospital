using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "tbl_zones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    HealthFacilityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_zones_tbl_health_facilities_HealthFacilityId",
                        column: x => x.HealthFacilityId,
                        principalTable: "tbl_health_facilities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZoneSpecialty",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ZoneId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneSpecialty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneSpecialty_tbl_specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "tbl_specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneSpecialty_tbl_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "tbl_zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_zones_HealthFacilityId",
                table: "tbl_zones",
                column: "HealthFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneSpecialty_SpecialtyId",
                table: "ZoneSpecialty",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneSpecialty_ZoneId",
                table: "ZoneSpecialty",
                column: "ZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZoneSpecialty");

            migrationBuilder.DropTable(
                name: "tbl_zones");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUnread",
                table: "mcs_notifications",
                type: "BIT",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "BIT(1)",
                oldDefaultValue: false);
        }
    }
}
