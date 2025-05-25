using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mcs_cancel_reasons_tbl_bookings_BookingId",
                table: "mcs_cancel_reasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mcs_cancel_reasons",
                table: "mcs_cancel_reasons");

            migrationBuilder.RenameTable(
                name: "mcs_cancel_reasons",
                newName: "tbl_cancel_reasons");

            migrationBuilder.RenameIndex(
                name: "IX_mcs_cancel_reasons_BookingId",
                table: "tbl_cancel_reasons",
                newName: "IX_tbl_cancel_reasons_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_cancel_reasons",
                table: "tbl_cancel_reasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cancel_reasons_tbl_bookings_BookingId",
                table: "tbl_cancel_reasons",
                column: "BookingId",
                principalTable: "tbl_bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cancel_reasons_tbl_bookings_BookingId",
                table: "tbl_cancel_reasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_cancel_reasons",
                table: "tbl_cancel_reasons");

            migrationBuilder.RenameTable(
                name: "tbl_cancel_reasons",
                newName: "mcs_cancel_reasons");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_cancel_reasons_BookingId",
                table: "mcs_cancel_reasons",
                newName: "IX_mcs_cancel_reasons_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mcs_cancel_reasons",
                table: "mcs_cancel_reasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_mcs_cancel_reasons_tbl_bookings_BookingId",
                table: "mcs_cancel_reasons",
                column: "BookingId",
                principalTable: "tbl_bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
