using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "tbl_bookings",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndBooking",
                table: "tbl_bookings",
                type: "TIME",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartBooking",
                table: "tbl_bookings",
                type: "TIME",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndBooking",
                table: "tbl_bookings");

            migrationBuilder.DropColumn(
                name: "StartBooking",
                table: "tbl_bookings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "tbl_bookings",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");
        }
    }
}
