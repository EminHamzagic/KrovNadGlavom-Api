using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class InitializeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgencyId",
                table: "Users",
                type: "varchar(255)",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionCompanyId",
                table: "Users",
                type: "varchar(255)",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    Name = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    PIB = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Address = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Email = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Phone = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "ConstructionCompanies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    Name = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    PIB = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Address = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Email = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Phone = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionCompanies", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "UserAgencyFollows",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    AgencyId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgencyFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAgencyFollows_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAgencyFollows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    CompanyId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    ParcelNumber = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Area = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FloorCount = table.Column<int>(type: "int", nullable: false),
                    ElevatorCount = table.Column<int>(type: "int", nullable: false),
                    GarageSpotCount = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExtendedUntil = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_ConstructionCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "ConstructionCompanies",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "AgencyRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    AgencyId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    BuildingId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    RejectionReason = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyRequests_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgencyRequests_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    BuildingId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    ApartmentNumber = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Area = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    BalconyCount = table.Column<int>(type: "int", nullable: false),
                    Orientation = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartments_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Garages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    BuildingId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    SpotNumber = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Garages_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    BuildingId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    PricePerM2 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PenaltyPerM2 = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    AgencyId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    ApartmentId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    InstallmentCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "DiscountRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    AgencyId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    ApartmentId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    Percentage = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Reason = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRequests_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountRequests_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    ApartmentId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    FromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateTable(
                name: "Installments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    ContractId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PaymentProof = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    IsConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Installments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AgencyId",
                table: "Users",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ConstructionCompanyId",
                table: "Users",
                column: "ConstructionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyRequests_AgencyId",
                table: "AgencyRequests",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyRequests_BuildingId",
                table: "AgencyRequests",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CompanyId",
                table: "Buildings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AgencyId",
                table: "Contracts",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ApartmentId",
                table: "Contracts",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRequests_AgencyId",
                table: "DiscountRequests",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRequests_ApartmentId",
                table: "DiscountRequests",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRequests_UserId",
                table: "DiscountRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Garages_BuildingId",
                table: "Garages",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_ContractId",
                table: "Installments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_BuildingId",
                table: "PriceLists",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ApartmentId",
                table: "Reservations",
                column: "ApartmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAgencyFollows_AgencyId",
                table: "UserAgencyFollows",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAgencyFollows_UserId",
                table: "UserAgencyFollows",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Agencies_AgencyId",
                table: "Users",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ConstructionCompanies_ConstructionCompanyId",
                table: "Users",
                column: "ConstructionCompanyId",
                principalTable: "ConstructionCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Agencies_AgencyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ConstructionCompanies_ConstructionCompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AgencyRequests");

            migrationBuilder.DropTable(
                name: "DiscountRequests");

            migrationBuilder.DropTable(
                name: "Garages");

            migrationBuilder.DropTable(
                name: "Installments");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "UserAgencyFollows");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "ConstructionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Users_AgencyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ConstructionCompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConstructionCompanyId",
                table: "Users");
        }
    }
}
