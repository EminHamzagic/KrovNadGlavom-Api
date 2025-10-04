using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddLateCounterToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLate",
                table: "Installments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LateCount",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLate",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "LateCount",
                table: "Contracts");
        }
    }
}
