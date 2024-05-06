using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsFieldToUpdateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "UpdateStatuses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "UpdateStatuses");
        }
    }
}
