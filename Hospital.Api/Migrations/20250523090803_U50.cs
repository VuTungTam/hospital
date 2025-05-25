using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentUrl",
                table: "tbl_payment");

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "tbl_payment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "tbl_payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentUrl",
                table: "tbl_payment",
                type: "NVARCHAR(512)",
                nullable: false,
                defaultValue: "");
        }
    }
}
