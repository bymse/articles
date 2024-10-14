using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bymse.Articles.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFromName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "from_name",
                table: "received_email",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "from_name",
                table: "received_email");
        }
    }
}
