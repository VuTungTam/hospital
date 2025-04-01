using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "mcs_system_configurations",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabledVerifiedAccount",
                table: "mcs_system_configurations",
                type: "BIT",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingNotificationBccEmails",
                table: "mcs_system_configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingNotificationEmail",
                table: "mcs_system_configurations",
                type: "NVARCHAR(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingNotificationBccEmails",
                table: "mcs_system_configurations");

            migrationBuilder.DropColumn(
                name: "BookingNotificationEmail",
                table: "mcs_system_configurations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "mcs_system_configurations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabledVerifiedAccount",
                table: "mcs_system_configurations",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: true);
        }
    }
}
