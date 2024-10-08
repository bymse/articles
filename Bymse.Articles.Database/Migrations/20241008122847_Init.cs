using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bymse.Articles.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                name: "mailbox",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    uid_validity = table.Column<long>(type: "bigint", nullable: true),
                    last_uid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mailbox", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sources",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    receiver_email = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    web_page = table.Column<string>(type: "text", nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    tenant_type = table.Column<int>(type: "integer", nullable: false),
                    confirmed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sources", x => x.id);
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
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    idp_provider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    idp_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "received_email",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    uid = table.Column<long>(type: "bigint", nullable: false),
                    uid_validity = table.Column<long>(type: "bigint", nullable: false),
                    mailbox_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    to_email = table.Column<string>(type: "text", nullable: false),
                    from_email = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    html_body = table.Column<string>(type: "text", nullable: true),
                    text_body = table.Column<string>(type: "text", nullable: true),
                    received_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_received_email", x => x.id);
                    table.ForeignKey(
                        name: "fk_received_email_mailbox_mailbox_id",
                        column: x => x.mailbox_id,
                        principalTable: "mailbox",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_received_email_mailbox_id",
                table: "received_email",
                column: "mailbox_id");

            migrationBuilder.CreateIndex(
                name: "ix_sources_receiver_email",
                table: "sources",
                column: "receiver_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_idp_provider_idp_user_id",
                table: "users",
                columns: new[] { "idp_provider", "idp_user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feed_sources");

            migrationBuilder.DropTable(
                name: "received_email");

            migrationBuilder.DropTable(
                name: "sources");

            migrationBuilder.DropTable(
                name: "user_sources");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "feeds");

            migrationBuilder.DropTable(
                name: "mailbox");
        }
    }
}
