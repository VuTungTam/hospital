using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_social_networks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "tbl_health_facilities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "tbl_health_facilities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "tbl_health_facilities");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "tbl_health_facilities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "TrainingProcess",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "WorkExperience",
                table: "mcs_doctors");

            migrationBuilder.RenameColumn(
                name: "FacilityName",
                table: "tbl_bookings",
                newName: "FacilityNameVn");

            migrationBuilder.AddColumn<string>(
                name: "Symptoms",
                table: "tbl_specialties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacilityNameEn",
                table: "tbl_bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symptoms",
                table: "tbl_specialties");

            migrationBuilder.DropColumn(
                name: "FacilityNameEn",
                table: "tbl_bookings");

            migrationBuilder.RenameColumn(
                name: "FacilityNameVn",
                table: "tbl_bookings",
                newName: "FacilityName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tbl_health_facilities",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "tbl_health_facilities",
                type: "DECIMAL(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "tbl_health_facilities",
                type: "DECIMAL(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "tbl_health_facilities",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "mcs_doctors",
                type: "NVARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingProcess",
                table: "mcs_doctors",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkExperience",
                table: "mcs_doctors",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_social_networks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Logo = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Qr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_social_networks", x => x.Id);
                });
        }
    }
}
