using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoggerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LogDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LogMessage = table.Column<string>(type: "text", nullable: false),
                    LogLevel = table.Column<int>(type: "integer", nullable: false),
                    LoggerName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
