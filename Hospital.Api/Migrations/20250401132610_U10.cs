using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mcs_notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<byte>(type: "TINYINT", nullable: false),
                    IsUnread = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_notifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mcs_notifications");
        }
    }
}
