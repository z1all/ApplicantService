using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_UsersCache_UserId",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Applicants",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_UsersCache_Id",
                table: "Applicants",
                column: "Id",
                principalTable: "UsersCache",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_UsersCache_Id",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Applicants",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_UsersCache_UserId",
                table: "Applicants",
                column: "UserId",
                principalTable: "UsersCache",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
