using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_booking_symptom");

            migrationBuilder.DropTable(
                name: "tbl_symptoms");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tbl_bookings",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "tbl_bookings",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "tbl_bookings");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "tbl_bookings");

            migrationBuilder.CreateTable(
                name: "tbl_symptoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_booking_symptom",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    SymptomId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_booking_symptom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_booking_symptom_tbl_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tbl_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_booking_symptom_tbl_symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "tbl_symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_symptom_BookingId",
                table: "tbl_booking_symptom",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_symptom_SymptomId",
                table: "tbl_booking_symptom",
                column: "SymptomId");
        }
    }
}
