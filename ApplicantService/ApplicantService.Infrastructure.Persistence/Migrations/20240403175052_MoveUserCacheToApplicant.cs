using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveUserCacheToApplicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_UsersCache_Id",
                table: "Applicants");

            migrationBuilder.DropTable(
                name: "UsersCache");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Applicants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Applicants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Applicants");

            migrationBuilder.CreateTable(
                name: "UsersCache",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCache", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_UsersCache_Id",
                table: "Applicants",
                column: "Id",
                principalTable: "UsersCache",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
