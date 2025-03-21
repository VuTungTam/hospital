using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_time_slots_tbl_service_time_rules_ServiceTimeRuleId",
                table: "tbl_time_slots");

            migrationBuilder.DropIndex(
                name: "IX_tbl_time_slots_ServiceTimeRuleId",
                table: "tbl_time_slots");

            migrationBuilder.DropColumn(
                name: "ServiceTimeRuleId",
                table: "tbl_time_slots");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Start",
                table: "tbl_time_slots",
                type: "TIME",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "End",
                table: "tbl_time_slots",
                type: "TIME",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_time_slots",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_time_slots",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_time_slots",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_time_slots_TimeRuleId",
                table: "tbl_time_slots",
                column: "TimeRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_time_slots_tbl_service_time_rules_TimeRuleId",
                table: "tbl_time_slots",
                column: "TimeRuleId",
                principalTable: "tbl_service_time_rules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_time_slots_tbl_service_time_rules_TimeRuleId",
                table: "tbl_time_slots");

            migrationBuilder.DropIndex(
                name: "IX_tbl_time_slots_TimeRuleId",
                table: "tbl_time_slots");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_time_slots");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Start",
                table: "tbl_time_slots",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TIME");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "End",
                table: "tbl_time_slots",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "tbl_time_slots",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "tbl_time_slots",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<long>(
                name: "ServiceTimeRuleId",
                table: "tbl_time_slots",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_time_slots_ServiceTimeRuleId",
                table: "tbl_time_slots",
                column: "ServiceTimeRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_time_slots_tbl_service_time_rules_ServiceTimeRuleId",
                table: "tbl_time_slots",
                column: "ServiceTimeRuleId",
                principalTable: "tbl_service_time_rules",
                principalColumn: "Id");
        }
    }
}
