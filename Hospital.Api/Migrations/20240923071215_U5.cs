using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleAction_Action_ActionId",
                table: "RoleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAction_Role_RoleId",
                table: "RoleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFacility_Branches_BranchId",
                table: "UserFacility");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFacility_Users_UserId",
                table: "UserFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFacility",
                table: "UserFacility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleAction",
                table: "RoleAction");

            migrationBuilder.RenameTable(
                name: "UserFacility",
                newName: "UserBranch");

            migrationBuilder.RenameTable(
                name: "RoleAction",
                newName: "RoleActions");

            migrationBuilder.RenameIndex(
                name: "IX_UserFacility_UserId",
                table: "UserBranch",
                newName: "IX_UserBranch_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFacility_BranchId",
                table: "UserBranch",
                newName: "IX_UserBranch_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAction_RoleId",
                table: "RoleActions",
                newName: "IX_RoleActions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAction_ActionId",
                table: "RoleActions",
                newName: "IX_RoleActions_ActionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBranch",
                table: "UserBranch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleActions",
                table: "RoleActions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "mcs_sequences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_sequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcs_system_configurations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequiresPasswordLevel = table.Column<int>(type: "int", nullable: false),
                    IsEnabledVerifiedAccount = table.Column<bool>(type: "bit", nullable: true),
                    Session = table.Column<int>(type: "int", nullable: true),
                    PasswordMinLength = table.Column<int>(type: "int", nullable: true),
                    MaxNumberOfSmsPerDay = table.Column<int>(type: "int", nullable: false),
                    PreventCopying = table.Column<bool>(type: "bit", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_system_configurations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleActions_Action_ActionId",
                table: "RoleActions",
                column: "ActionId",
                principalTable: "Action",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleActions_Role_RoleId",
                table: "RoleActions",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranch_Branches_BranchId",
                table: "UserBranch",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranch_Users_UserId",
                table: "UserBranch",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleActions_Action_ActionId",
                table: "RoleActions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleActions_Role_RoleId",
                table: "RoleActions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranch_Branches_BranchId",
                table: "UserBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranch_Users_UserId",
                table: "UserBranch");

            migrationBuilder.DropTable(
                name: "mcs_sequences");

            migrationBuilder.DropTable(
                name: "mcs_system_configurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBranch",
                table: "UserBranch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleActions",
                table: "RoleActions");

            migrationBuilder.RenameTable(
                name: "UserBranch",
                newName: "UserFacility");

            migrationBuilder.RenameTable(
                name: "RoleActions",
                newName: "RoleAction");

            migrationBuilder.RenameIndex(
                name: "IX_UserBranch_UserId",
                table: "UserFacility",
                newName: "IX_UserFacility_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBranch_BranchId",
                table: "UserFacility",
                newName: "IX_UserFacility_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleActions_RoleId",
                table: "RoleAction",
                newName: "IX_RoleAction_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleActions_ActionId",
                table: "RoleAction",
                newName: "IX_RoleAction_ActionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFacility",
                table: "UserFacility",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleAction",
                table: "RoleAction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAction_Action_ActionId",
                table: "RoleAction",
                column: "ActionId",
                principalTable: "Action",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAction_Role_RoleId",
                table: "RoleAction",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFacility_Branches_BranchId",
                table: "UserFacility",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFacility_Users_UserId",
                table: "UserFacility",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
