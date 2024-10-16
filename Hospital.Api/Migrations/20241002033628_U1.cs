using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Api.Migrations
{
    public partial class U1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent = table.Column<int>(type: "int", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Author = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    ImageUrl = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    FoundingDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Active = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Declarations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CICode = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(12)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Dob = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Pname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Did = table.Column<int>(type: "int", nullable: false),
                    Dname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Wid = table.Column<int>(type: "int", nullable: false),
                    Wname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Declarations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "location_districts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "location_provinces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "location_wards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_wards", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshTokenValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialNetworks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Link = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Logo = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Salt = table.Column<string>(type: "NVARCHAR(8)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    PhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Dob = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Pname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Did = table.Column<int>(type: "int", nullable: false),
                    Dname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Wid = table.Column<int>(type: "int", nullable: false),
                    Wname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZaloId = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    Provider = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Json = table.Column<string>(type: "TEXT", nullable: true),
                    Shard = table.Column<int>(type: "int", nullable: false),
                    IsCustomer = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    LastPurchase = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    TotalSpending = table.Column<decimal>(type: "DECIMAL(19,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Modified = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthFacility",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionVn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Pname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<int>(type: "int", nullable: false),
                    Dname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wid = table.Column<int>(type: "int", nullable: false),
                    Wname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longtitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthFacility_FacilityCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "FacilityCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleActions_Action_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleActions_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchSpecialty",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchSpecialty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchSpecialty_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchSpecialty_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBranch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBranch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBranch_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    BranchSpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthServices_BranchSpecialty_BranchSpecialtyId",
                        column: x => x.BranchSpecialtyId,
                        principalTable: "BranchSpecialty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthServices_ServiceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeclarationId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Declarations_DeclarationId",
                        column: x => x.DeclarationId,
                        principalTable: "Declarations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visits_HealthServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "HealthServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueueItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<long>(type: "bigint", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    Creator = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueItems_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitSymptom",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<long>(type: "bigint", nullable: false),
                    SymptomId = table.Column<long>(type: "bigint", nullable: false),
                    Creator = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitSymptom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitSymptom_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitSymptom_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchSpecialty_BranchId",
                table: "BranchSpecialty",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchSpecialty_SpecialtyId",
                table: "BranchSpecialty",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthFacility_CategoryId",
                table: "HealthFacility",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthServices_BranchSpecialtyId",
                table: "HealthServices",
                column: "BranchSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthServices_TypeId",
                table: "HealthServices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueItems_VisitId",
                table: "QueueItems",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActions_ActionId",
                table: "RoleActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActions_RoleId",
                table: "RoleActions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranch_BranchId",
                table: "UserBranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranch_UserId",
                table: "UserBranch",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Code",
                table: "Users",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordHash",
                table: "Users",
                column: "PasswordHash");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_DeclarationId",
                table: "Visits",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_ServiceId",
                table: "Visits",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitSymptom_SymptomId",
                table: "VisitSymptom",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitSymptom_VisitId",
                table: "VisitSymptom",
                column: "VisitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "HealthFacility");

            migrationBuilder.DropTable(
                name: "location_districts");

            migrationBuilder.DropTable(
                name: "location_provinces");

            migrationBuilder.DropTable(
                name: "location_wards");

            migrationBuilder.DropTable(
                name: "mcs_sequences");

            migrationBuilder.DropTable(
                name: "mcs_system_configurations");

            migrationBuilder.DropTable(
                name: "QueueItems");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleActions");

            migrationBuilder.DropTable(
                name: "SocialNetworks");

            migrationBuilder.DropTable(
                name: "UserBranch");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "VisitSymptom");

            migrationBuilder.DropTable(
                name: "FacilityCategories");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Declarations");

            migrationBuilder.DropTable(
                name: "HealthServices");

            migrationBuilder.DropTable(
                name: "BranchSpecialty");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
