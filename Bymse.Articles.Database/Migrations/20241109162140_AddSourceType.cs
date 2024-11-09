using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bymse.Articles.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "sources",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "sources");
        }
    }
}
