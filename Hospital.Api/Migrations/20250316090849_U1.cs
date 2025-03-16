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
                name: "mcs_customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    LastPurchase = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    TotalSpending = table.Column<decimal>(type: "DECIMAL(19,2)", nullable: false),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    AliasLogin = table.Column<string>(type: "NVARCHAR(128)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
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
                    IsDefaultPassword = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPasswordChangeRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastSeen = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcs_doctors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expertise = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    TrainingProcess = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    WorkExperience = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    MinFee = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    MaxFee = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    StarPoint = table.Column<decimal>(type: "DECIMAL(1,1)", nullable: false),
                    DoctorStatus = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    AliasLogin = table.Column<string>(type: "NVARCHAR(128)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
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
                    IsDefaultPassword = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPasswordChangeRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastSeen = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcs_employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleColor = table.Column<string>(type: "VARCHAR(7)", nullable: true),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    AliasLogin = table.Column<string>(type: "NVARCHAR(128)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
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
                    IsDefaultPassword = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPasswordChangeRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastSeen = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcs_login_histories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_login_histories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mcs_refresh_tokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshTokenValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_refresh_tokens", x => x.Id);
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mcs_system_configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perm_actions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(1024)", nullable: false),
                    Exponent = table.Column<int>(type: "int", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perm_roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Slug = table.Column<string>(type: "NVARCHAR(1048)", nullable: false),
                    TitleSeo = table.Column<string>(type: "NVARCHAR(1024)", nullable: true),
                    Title = table.Column<string>(type: "NVARCHAR(1024)", nullable: false),
                    TitleEn = table.Column<string>(type: "NVARCHAR(1024)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    SummaryEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Toc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TocEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Status = table.Column<short>(type: "SMALLINT", nullable: false),
                    IsHighlight = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_distances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceLatitude = table.Column<double>(type: "float", nullable: false),
                    SourceLongitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    DistanceMeter = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_distances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_facility_types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_facility_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_feedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferId = table.Column<long>(type: "bigint", nullable: false),
                    Stars = table.Column<int>(type: "INT", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_health_facilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    ImageUrl = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Website = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Pname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Did = table.Column<int>(type: "int", nullable: false),
                    Dname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Wid = table.Column<int>(type: "int", nullable: false),
                    Wname = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false),
                    Longtitude = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_health_facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_health_profiles",
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
                    Eid = table.Column<int>(type: "int", nullable: false),
                    Ethinic = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_health_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_service_types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_service_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_social_networks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    Link = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Logo = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Qr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_social_networks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_specialties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_symptoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perm_employee_action_map",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: false),
                    IsExclude = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_employee_action_map", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perm_employee_action_map_mcs_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "mcs_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_perm_employee_action_map_perm_actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "perm_actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "perm_employee_role_map",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_employee_role_map", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perm_employee_role_map_mcs_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "mcs_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_perm_employee_role_map_perm_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "perm_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "perm_roles_actions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perm_roles_actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perm_roles_actions_perm_actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "perm_actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_perm_roles_actions_perm_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "perm_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_facility_type_mappings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_facility_type_mappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_facility_type_mappings_tbl_facility_types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "tbl_facility_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_facility_type_mappings_tbl_health_facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "tbl_health_facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_doctor_specialty",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_doctor_specialty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_doctor_specialty_mcs_doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "mcs_doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_doctor_specialty_tbl_specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "tbl_specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_facility_specialty",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_facility_specialty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_facility_specialty_tbl_health_facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "tbl_health_facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_facility_specialty_tbl_specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "tbl_specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_health_services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameVn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    NameEn = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    DescriptionVn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    FacilitySpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_health_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_health_services_tbl_facility_specialty_FacilitySpecialtyId",
                        column: x => x.FacilitySpecialtyId,
                        principalTable: "tbl_facility_specialty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_health_services_tbl_service_types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "tbl_service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_bookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    HealthProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Status = table.Column<short>(type: "SMALLINT", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ServiceEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_bookings_tbl_health_profiles_HealthProfileId",
                        column: x => x.HealthProfileId,
                        principalTable: "tbl_health_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_bookings_tbl_health_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "tbl_health_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_service_time_rules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartBreakTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndBreakTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SlotDuration = table.Column<int>(type: "int", nullable: false),
                    MaxPatients = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_service_time_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_service_time_rules_tbl_health_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "tbl_health_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_booking_symptom",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "IX_mcs_customers_Code",
                table: "mcs_customers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mcs_customers_Email",
                table: "mcs_customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_customers_Name",
                table: "mcs_customers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_customers_Phone",
                table: "mcs_customers",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_doctors_Code",
                table: "mcs_doctors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mcs_doctors_Email",
                table: "mcs_doctors",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_doctors_Name",
                table: "mcs_doctors",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_doctors_Phone",
                table: "mcs_doctors",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_employees_Code",
                table: "mcs_employees",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mcs_employees_Email",
                table: "mcs_employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_employees_Name",
                table: "mcs_employees",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_mcs_employees_Phone",
                table: "mcs_employees",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_perm_actions_Code",
                table: "perm_actions",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_action_map_ActionId",
                table: "perm_employee_action_map",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_action_map_EmployeeId",
                table: "perm_employee_action_map",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_role_map_EmployeeId",
                table: "perm_employee_role_map",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_employee_role_map_RoleId",
                table: "perm_employee_role_map",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_roles_Code",
                table: "perm_roles",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_perm_roles_actions_ActionId",
                table: "perm_roles_actions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_perm_roles_actions_RoleId",
                table: "perm_roles_actions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_articles_Title",
                table: "tbl_articles",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_symptom_BookingId",
                table: "tbl_booking_symptom",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_symptom_SymptomId",
                table: "tbl_booking_symptom",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bookings_Code",
                table: "tbl_bookings",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bookings_HealthProfileId",
                table: "tbl_bookings",
                column: "HealthProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bookings_ServiceId",
                table: "tbl_bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_doctor_specialty_DoctorId",
                table: "tbl_doctor_specialty",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_doctor_specialty_SpecialtyId",
                table: "tbl_doctor_specialty",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_facility_specialty_FacilityId",
                table: "tbl_facility_specialty",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_facility_specialty_SpecialtyId",
                table: "tbl_facility_specialty",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_facility_type_mappings_FacilityId",
                table: "tbl_facility_type_mappings",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_facility_type_mappings_TypeId",
                table: "tbl_facility_type_mappings",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_FacilitySpecialtyId",
                table: "tbl_health_services",
                column: "FacilitySpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_services_TypeId",
                table: "tbl_health_services",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_service_time_rules_ServiceId",
                table: "tbl_service_time_rules",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "location_districts");

            migrationBuilder.DropTable(
                name: "location_provinces");

            migrationBuilder.DropTable(
                name: "location_wards");

            migrationBuilder.DropTable(
                name: "mcs_customers");

            migrationBuilder.DropTable(
                name: "mcs_login_histories");

            migrationBuilder.DropTable(
                name: "mcs_refresh_tokens");

            migrationBuilder.DropTable(
                name: "mcs_sequences");

            migrationBuilder.DropTable(
                name: "mcs_system_configurations");

            migrationBuilder.DropTable(
                name: "perm_employee_action_map");

            migrationBuilder.DropTable(
                name: "perm_employee_role_map");

            migrationBuilder.DropTable(
                name: "perm_roles_actions");

            migrationBuilder.DropTable(
                name: "tbl_articles");

            migrationBuilder.DropTable(
                name: "tbl_booking_symptom");

            migrationBuilder.DropTable(
                name: "tbl_distances");

            migrationBuilder.DropTable(
                name: "tbl_doctor_specialty");

            migrationBuilder.DropTable(
                name: "tbl_facility_type_mappings");

            migrationBuilder.DropTable(
                name: "tbl_feedbacks");

            migrationBuilder.DropTable(
                name: "tbl_service_time_rules");

            migrationBuilder.DropTable(
                name: "tbl_social_networks");

            migrationBuilder.DropTable(
                name: "mcs_employees");

            migrationBuilder.DropTable(
                name: "perm_actions");

            migrationBuilder.DropTable(
                name: "perm_roles");

            migrationBuilder.DropTable(
                name: "tbl_bookings");

            migrationBuilder.DropTable(
                name: "tbl_symptoms");

            migrationBuilder.DropTable(
                name: "mcs_doctors");

            migrationBuilder.DropTable(
                name: "tbl_facility_types");

            migrationBuilder.DropTable(
                name: "tbl_health_profiles");

            migrationBuilder.DropTable(
                name: "tbl_health_services");

            migrationBuilder.DropTable(
                name: "tbl_facility_specialty");

            migrationBuilder.DropTable(
                name: "tbl_service_types");

            migrationBuilder.DropTable(
                name: "tbl_health_facilities");

            migrationBuilder.DropTable(
                name: "tbl_specialties");
        }
    }
}
