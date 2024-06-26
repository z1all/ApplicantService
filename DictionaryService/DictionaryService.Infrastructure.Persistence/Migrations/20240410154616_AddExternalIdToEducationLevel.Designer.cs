﻿// <auto-generated />
using System;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240410154616_AddExternalIdToEducationLevel")]
    partial class AddExternalIdToEducationLevel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DictionaryService.Core.Domain.EducationDocumentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<Guid>("EducationLevelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.ToTable("EducationDocumentTypes");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.EducationLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<int>("ExternelId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EducationLevels");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.EducationProgram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<string>("EducationForm")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EducationLevelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.HasIndex("FacultyId");

                    b.ToTable("EducationPrograms");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.UpdateStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comments")
                        .HasColumnType("text");

                    b.Property<int>("DictionaryType")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasAlternateKey("DictionaryType");

                    b.ToTable("UpdateStatuses");
                });

            modelBuilder.Entity("EducationDocumentTypeEducationLevel", b =>
                {
                    b.Property<Guid>("EducationDocumentTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("NextEducationLevelsId")
                        .HasColumnType("uuid");

                    b.HasKey("EducationDocumentTypeId", "NextEducationLevelsId");

                    b.HasIndex("NextEducationLevelsId");

                    b.ToTable("EducationDocumentTypeEducationLevel");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.EducationDocumentType", b =>
                {
                    b.HasOne("DictionaryService.Core.Domain.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.EducationProgram", b =>
                {
                    b.HasOne("DictionaryService.Core.Domain.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DictionaryService.Core.Domain.Faculty", "Faculty")
                        .WithMany("EducationPrograms")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("EducationDocumentTypeEducationLevel", b =>
                {
                    b.HasOne("DictionaryService.Core.Domain.EducationDocumentType", null)
                        .WithMany()
                        .HasForeignKey("EducationDocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DictionaryService.Core.Domain.EducationLevel", null)
                        .WithMany()
                        .HasForeignKey("NextEducationLevelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DictionaryService.Core.Domain.Faculty", b =>
                {
                    b.Navigation("EducationPrograms");
                });
#pragma warning restore 612, 618
        }
    }
}
