using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedAtColsAndBankNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "ConstructionCompanies",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Buildings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Buildings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "ConstructionCompanies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Agencies");
        }
    }
}
