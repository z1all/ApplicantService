using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateStatusDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UpdateStatuses_DictionaryType",
                table: "UpdateStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UpdateStatuses",
                table: "UpdateStatuses");

            migrationBuilder.RenameTable(
                name: "UpdateStatuses",
                newName: "UpdateStatus");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UpdateStatus_DictionaryType",
                table: "UpdateStatus",
                column: "DictionaryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UpdateStatus",
                table: "UpdateStatus",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UpdateStatus_DictionaryType",
                table: "UpdateStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UpdateStatus",
                table: "UpdateStatus");

            migrationBuilder.RenameTable(
                name: "UpdateStatus",
                newName: "UpdateStatuses");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UpdateStatuses_DictionaryType",
                table: "UpdateStatuses",
                column: "DictionaryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UpdateStatuses",
                table: "UpdateStatuses",
                column: "Id");
        }
    }
}
