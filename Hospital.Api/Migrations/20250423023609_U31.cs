using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "tbl_service_time_rules",
                type: "Time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartBreakTime",
                table: "tbl_service_time_rules",
                type: "Time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "tbl_service_time_rules",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "tbl_service_time_rules",
                type: "Time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndBreakTime",
                table: "tbl_service_time_rules",
                type: "Time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_service_time_rules",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_service_time_rules",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "HealthServiceId",
                table: "tbl_service_time_rules",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_service_time_rules_HealthServiceId",
                table: "tbl_service_time_rules",
                column: "HealthServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_service_time_rules_tbl_health_services_HealthServiceId",
                table: "tbl_service_time_rules",
                column: "HealthServiceId",
                principalTable: "tbl_health_services",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_service_time_rules_tbl_health_services_HealthServiceId",
                table: "tbl_service_time_rules");

            migrationBuilder.DropIndex(
                name: "IX_tbl_service_time_rules_HealthServiceId",
                table: "tbl_service_time_rules");

            migrationBuilder.DropColumn(
                name: "HealthServiceId",
                table: "tbl_service_time_rules");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "tbl_service_time_rules",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "Time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartBreakTime",
                table: "tbl_service_time_rules",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "Time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "tbl_service_time_rules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "tbl_service_time_rules",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "Time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndBreakTime",
                table: "tbl_service_time_rules",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "Time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_service_time_rules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_service_time_rules",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
