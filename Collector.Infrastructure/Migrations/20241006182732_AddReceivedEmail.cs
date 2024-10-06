using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collector.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivedEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "received_email");
        }
    }
}
