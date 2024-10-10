using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bymse.Articles.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddManualProcessingEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "headers",
                table: "received_email",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "received_email",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "manual_processing_email",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(26)", nullable: false),
                    received_email_id = table.Column<string>(type: "character varying(26)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manual_processing_email", x => x.id);
                    table.ForeignKey(
                        name: "fk_manual_processing_email_received_email_received_email_id",
                        column: x => x.received_email_id,
                        principalTable: "received_email",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_manual_processing_email_received_email_id",
                table: "manual_processing_email",
                column: "received_email_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "manual_processing_email");

            migrationBuilder.DropColumn(
                name: "headers",
                table: "received_email");

            migrationBuilder.DropColumn(
                name: "type",
                table: "received_email");
        }
    }
}
