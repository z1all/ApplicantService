using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixMistakeInEducationLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternelId",
                table: "EducationLevels",
                newName: "ExternalId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EducationLevels_ExternalId",
                table: "EducationLevels",
                column: "ExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_EducationLevels_ExternalId",
                table: "EducationLevels");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "EducationLevels",
                newName: "ExternelId");
        }
    }
}
