﻿// <auto-generated />
using System;
using LinguaContext.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinguaContext.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231025171803_addPersonalFactors")]
    partial class addPersonalFactors
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("LinguaContext.Models.Context", b =>
                {
                    b.Property<int>("ContextId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("ContextId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastSeenAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ContextId");

                    b.HasIndex("UserId");

                    b.ToTable("Contexts");
                });

            modelBuilder.Entity("LinguaContext.Models.FavoriteSentence", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("SentenceId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId", "SentenceId");

                    b.ToTable("FavoriteSentences");
                });

            modelBuilder.Entity("LinguaContext.Models.Folder", b =>
                {
                    b.Property<int>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("FolderId"));

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.Property<string>("SharingStatusCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("LinguaContext.Models.PersonalFactors", b =>
                {
                    b.Property<int>("PersonalFactorsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("PersonalFactorsId"));

                    b.Property<double>("EasyIntervalModifier")
                        .HasColumnType("double precision");

                    b.Property<double>("FailIntervalModifier")
                        .HasColumnType("double precision");

                    b.Property<double>("HardIntervalModifier")
                        .HasColumnType("double precision");

                    b.Property<double>("IntervalModifier")
                        .HasColumnType("double precision");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PersonalFactorsId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("PersonalFactors");
                });

            modelBuilder.Entity("LinguaContext.Models.Sentence", b =>
                {
                    b.Property<int>("SentenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("SentenceId"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("AnswerTranslation")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Phrase")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int?>("UserSentenceInfoId")
                        .HasColumnType("integer");

                    b.HasKey("SentenceId");

                    b.HasIndex("UserSentenceInfoId")
                        .IsUnique();

                    b.ToTable("Sentences");
                });

            modelBuilder.Entity("LinguaContext.Models.SentenceInFolder", b =>
                {
                    b.Property<int>("FolderId")
                        .HasColumnType("integer");

                    b.Property<int>("SentenceId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("FolderId", "SentenceId");

                    b.ToTable("SentencesInFolder");
                });

            modelBuilder.Entity("LinguaContext.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("UserId"));

                    b.Property<DateOnly>("BirthDay")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.Property<string>("LastName")
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("character varying(35)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LinguaContext.Models.UserSentenceInfo", b =>
                {
                    b.Property<int>("UserSentenceInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("UserSentenceInfoId"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastEditedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("PositionInText")
                        .HasColumnType("integer");

                    b.Property<int?>("SourceContextId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserSentenceInfoId");

                    b.HasIndex("SourceContextId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSentenceInfos");
                });

            modelBuilder.Entity("LinguaContext.Models.UserTask", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("SentenceId")
                        .HasColumnType("integer");

                    b.Property<int>("CurrentInterval")
                        .HasColumnType("integer");

                    b.Property<double>("EaseFactor")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("FirstReview")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastReview")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("NextReview")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RepetitionNumber")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "SentenceId");

                    b.HasIndex("SentenceId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("LinguaContext.Models.Context", b =>
                {
                    b.HasOne("LinguaContext.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LinguaContext.Models.PersonalFactors", b =>
                {
                    b.HasOne("LinguaContext.Models.User", "User")
                        .WithOne("Factors")
                        .HasForeignKey("LinguaContext.Models.PersonalFactors", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LinguaContext.Models.Sentence", b =>
                {
                    b.HasOne("LinguaContext.Models.UserSentenceInfo", "UserSentenceInfo")
                        .WithOne("Sentence")
                        .HasForeignKey("LinguaContext.Models.Sentence", "UserSentenceInfoId");

                    b.Navigation("UserSentenceInfo");
                });

            modelBuilder.Entity("LinguaContext.Models.UserSentenceInfo", b =>
                {
                    b.HasOne("LinguaContext.Models.Context", "Context")
                        .WithMany()
                        .HasForeignKey("SourceContextId");

                    b.HasOne("LinguaContext.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Context");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LinguaContext.Models.UserTask", b =>
                {
                    b.HasOne("LinguaContext.Models.Sentence", "Sentence")
                        .WithMany()
                        .HasForeignKey("SentenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LinguaContext.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sentence");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LinguaContext.Models.User", b =>
                {
                    b.Navigation("Factors");
                });

            modelBuilder.Entity("LinguaContext.Models.UserSentenceInfo", b =>
                {
                    b.Navigation("Sentence");
                });
#pragma warning restore 612, 618
        }
    }
}