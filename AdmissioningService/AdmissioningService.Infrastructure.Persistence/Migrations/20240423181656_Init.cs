using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdmissioningService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdmissionCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventYear = table.Column<int>(type: "integer", nullable: false),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevelCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevelCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacultyCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacultyCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationDocumentTypeCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false),
                    EducationLevelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDocumentTypeCaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationDocumentTypeCaches_EducationLevelCaches_EducationL~",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevelCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationProgramCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    EducationForm = table.Column<string>(type: "text", nullable: false),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uuid", nullable: false),
                    EducationLevelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationProgramCaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationProgramCaches_EducationLevelCaches_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevelCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationProgramCaches_FacultyCaches_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "FacultyCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_FacultyCaches_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "FacultyCaches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Managers_UserCaches_Id",
                        column: x => x.Id,
                        principalTable: "UserCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantCacheEducationDocumentTypeCache",
                columns: table => new
                {
                    AddedDocumentTypesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantCacheId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantCacheEducationDocumentTypeCache", x => new { x.AddedDocumentTypesId, x.ApplicantCacheId });
                    table.ForeignKey(
                        name: "FK_ApplicantCacheEducationDocumentTypeCache_ApplicantCaches_Ap~",
                        column: x => x.ApplicantCacheId,
                        principalTable: "ApplicantCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantCacheEducationDocumentTypeCache_EducationDocumentT~",
                        column: x => x.AddedDocumentTypesId,
                        principalTable: "EducationDocumentTypeCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationDocumentTypeCacheEducationLevelCache",
                columns: table => new
                {
                    EducationDocumentTypeCacheId = table.Column<Guid>(type: "uuid", nullable: false),
                    NextEducationLevelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDocumentTypeCacheEducationLevelCache", x => new { x.EducationDocumentTypeCacheId, x.NextEducationLevelId });
                    table.ForeignKey(
                        name: "FK_EducationDocumentTypeCacheEducationLevelCache_EducationDocu~",
                        column: x => x.EducationDocumentTypeCacheId,
                        principalTable: "EducationDocumentTypeCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationDocumentTypeCacheEducationLevelCache_EducationLeve~",
                        column: x => x.NextEducationLevelId,
                        principalTable: "EducationLevelCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantAdmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdmissionStatus = table.Column<int>(type: "integer", nullable: false),
                    AdmissionCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAdmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantAdmissions_AdmissionCompanies_AdmissionCompanyId",
                        column: x => x.AdmissionCompanyId,
                        principalTable: "AdmissionCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantAdmissions_ApplicantCaches_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "ApplicantCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantAdmissions_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdmissionPrograms",
                columns: table => new
                {
                    EducationProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantAdmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionPrograms", x => new { x.ApplicantAdmissionId, x.EducationProgramId });
                    table.ForeignKey(
                        name: "FK_AdmissionPrograms_ApplicantAdmissions_ApplicantAdmissionId",
                        column: x => x.ApplicantAdmissionId,
                        principalTable: "ApplicantAdmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdmissionPrograms_EducationProgramCaches_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationProgramCaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionPrograms_EducationProgramId",
                table: "AdmissionPrograms",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAdmissions_AdmissionCompanyId",
                table: "ApplicantAdmissions",
                column: "AdmissionCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAdmissions_ApplicantId",
                table: "ApplicantAdmissions",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAdmissions_ManagerId",
                table: "ApplicantAdmissions",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantCacheEducationDocumentTypeCache_ApplicantCacheId",
                table: "ApplicantCacheEducationDocumentTypeCache",
                column: "ApplicantCacheId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocumentTypeCacheEducationLevelCache_NextEducation~",
                table: "EducationDocumentTypeCacheEducationLevelCache",
                column: "NextEducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDocumentTypeCaches_EducationLevelId",
                table: "EducationDocumentTypeCaches",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramCaches_EducationLevelId",
                table: "EducationProgramCaches",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramCaches_FacultyId",
                table: "EducationProgramCaches",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_FacultyId",
                table: "Managers",
                column: "FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmissionPrograms");

            migrationBuilder.DropTable(
                name: "ApplicantCacheEducationDocumentTypeCache");

            migrationBuilder.DropTable(
                name: "EducationDocumentTypeCacheEducationLevelCache");

            migrationBuilder.DropTable(
                name: "ApplicantAdmissions");

            migrationBuilder.DropTable(
                name: "EducationProgramCaches");

            migrationBuilder.DropTable(
                name: "EducationDocumentTypeCaches");

            migrationBuilder.DropTable(
                name: "AdmissionCompanies");

            migrationBuilder.DropTable(
                name: "ApplicantCaches");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "EducationLevelCaches");

            migrationBuilder.DropTable(
                name: "FacultyCaches");

            migrationBuilder.DropTable(
                name: "UserCaches");
        }
    }
}
