using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescAndLogoCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Users",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ConstructionCompanies",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Buildings",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_bin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ConstructionCompanies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Agencies");
        }
    }
}
