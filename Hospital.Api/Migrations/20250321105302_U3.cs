using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_service_doctor");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_service_doctor");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_service_doctor");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_service_doctor");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_facility_type_mappings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_facility_type_mappings");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_facility_type_mappings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_facility_type_mappings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_doctor_specialty");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_doctor_specialty");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_doctor_specialty");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_doctor_specialty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_service_doctor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_service_doctor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_service_doctor",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_service_doctor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_facility_type_mappings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_facility_type_mappings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_facility_type_mappings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_facility_type_mappings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_doctor_specialty",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_doctor_specialty",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_doctor_specialty",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_doctor_specialty",
                type: "bigint",
                nullable: true);
        }
    }
}
