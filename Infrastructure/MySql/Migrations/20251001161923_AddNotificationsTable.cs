using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace krov_nad_glavom_api.Infrastructure.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_bin"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_bin"),
                    Label = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Title = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin"),
                    Message = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
