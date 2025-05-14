using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mcs_cancel_reasons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    CancelType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_cancel_reasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mcs_cancel_reasons_tbl_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tbl_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mcs_cancel_reasons_BookingId",
                table: "mcs_cancel_reasons",
                column: "BookingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mcs_cancel_reasons");
        }
    }
}
