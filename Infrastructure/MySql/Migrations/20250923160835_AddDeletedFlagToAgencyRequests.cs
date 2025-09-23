using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedFlagToAgencyRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AgencyRequests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AgencyRequests");
        }
    }
}
