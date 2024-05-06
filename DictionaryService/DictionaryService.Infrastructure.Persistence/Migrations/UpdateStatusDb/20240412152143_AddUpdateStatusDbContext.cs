using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations.UpdateStatusDb
{
    /// <inheritdoc />
    public partial class AddUpdateStatusDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DictionaryType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateStatuses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateStatuses");
        }
    }
}
