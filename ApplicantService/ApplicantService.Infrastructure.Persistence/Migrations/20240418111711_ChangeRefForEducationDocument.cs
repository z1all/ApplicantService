using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRefForEducationDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments",
                column: "EducationDocumentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocuments_EducationDocumentTypeId",
                table: "EducationDocuments",
                column: "EducationDocumentTypeId",
                unique: true);
        }
    }
}
