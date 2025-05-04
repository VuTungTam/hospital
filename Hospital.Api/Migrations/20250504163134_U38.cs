using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    TransactionContent = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    PaymentUrl = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    BankBin = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    Note = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    ExpiredAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tbl_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_BookingId",
                table: "tbl_payment",
                column: "BookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_payment");
        }
    }
}
