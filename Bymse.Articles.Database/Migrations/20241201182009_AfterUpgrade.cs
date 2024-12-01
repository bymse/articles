using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bymse.Articles.Database.Migrations
{
    /// <inheritdoc />
    public partial class AfterUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_received_email_uid_uid_validity",
                table: "received_email",
                columns: new[] { "uid", "uid_validity" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_received_email_uid_uid_validity",
                table: "received_email");
        }
    }
}
