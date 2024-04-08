using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNameAndTypeFieldsToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PathName",
                table: "FilesInfo",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FilesInfo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FilesInfo");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "FilesInfo",
                newName: "PathName");
        }
    }
}
