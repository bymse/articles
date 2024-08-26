using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feeder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedAndUserSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feeds",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    user_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feeds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_sources",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    source_id = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_sources", x => new { x.user_id, x.source_id });
                });

            migrationBuilder.CreateTable(
                name: "feed_sources",
                columns: table => new
                {
                    feed_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    source_id = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feed_sources", x => new { x.feed_id, x.source_id });
                    table.ForeignKey(
                        name: "fk_feed_sources_feeds_feed_id",
                        column: x => x.feed_id,
                        principalTable: "feeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feed_sources");

            migrationBuilder.DropTable(
                name: "user_sources");

            migrationBuilder.DropTable(
                name: "feeds");
        }
    }
}
