using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "tbl_images");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_images");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_images");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tbl_images");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_images");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "tbl_health_facilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_images",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "tbl_images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_images",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeletedBy",
                table: "tbl_images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "tbl_health_facilities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
