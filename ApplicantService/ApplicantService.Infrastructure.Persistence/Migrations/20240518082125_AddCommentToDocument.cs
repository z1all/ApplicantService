using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Documents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Documents");
        }
    }
}
