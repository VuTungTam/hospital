using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankBin",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "ExternalTransactionId",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "tbl_payment");

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "tbl_payment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "tbl_payment");

            migrationBuilder.AddColumn<string>(
                name: "BankBin",
                table: "tbl_payment",
                type: "NVARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_payment",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "tbl_payment",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalTransactionId",
                table: "tbl_payment",
                type: "NVARCHAR(512)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_payment",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tbl_payment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "tbl_payment",
                type: "NVARCHAR(512)",
                nullable: true);
        }
    }
}
