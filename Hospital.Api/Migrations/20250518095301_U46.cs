using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_distances");

            migrationBuilder.DropColumn(
                name: "ScheduleColor",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "Shard",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "ZaloId",
                table: "mcs_employees");

            migrationBuilder.DropColumn(
                name: "Shard",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "ZaloId",
                table: "mcs_doctors");

            migrationBuilder.DropColumn(
                name: "LastPurchase",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "Shard",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "TotalSpending",
                table: "mcs_customers");

            migrationBuilder.DropColumn(
                name: "ZaloId",
                table: "mcs_customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduleColor",
                table: "mcs_employees",
                type: "VARCHAR(7)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shard",
                table: "mcs_employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZaloId",
                table: "mcs_employees",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shard",
                table: "mcs_doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZaloId",
                table: "mcs_doctors",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPurchase",
                table: "mcs_customers",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shard",
                table: "mcs_customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpending",
                table: "mcs_customers",
                type: "DECIMAL(19,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ZaloId",
                table: "mcs_customers",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_distances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    DistanceMeter = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    SourceLatitude = table.Column<double>(type: "float", nullable: false),
                    SourceLongitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_distances", x => x.Id);
                });
        }
    }
}
