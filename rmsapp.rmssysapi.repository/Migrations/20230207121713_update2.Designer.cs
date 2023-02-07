﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using rmsapp.rmssysapi.repository;

namespace rmsapp.rmssysapi.repository.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    [Migration("20230207121713_update2")]
    partial class update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("rmsapp.rmssysapi.service.Models.Candidate", b =>
                {
                    b.Property<string>("CandidateId")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Skills")
                        .HasColumnType("text");

                    b.Property<string>("SubjectName")
                        .HasColumnType("text");

                    b.Property<double>("TotalExperience")
                        .HasColumnType("double precision");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("CandidateId");

                    b.ToTable("Candidate");
                });

            modelBuilder.Entity("rmsapp.rmssysapi.service.Models.MasterQuiz", b =>
                {
                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<int>("SetNumber")
                        .HasColumnType("integer");

                    b.Property<string>("SubjectName")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Question")
                        .HasColumnType("text");

                    b.Property<string[]>("QuestionAnswers")
                        .HasColumnType("text[]");

                    b.Property<string[]>("QuestionAnswersIds")
                        .HasColumnType("text[]");

                    b.Property<string[]>("QuestionOptions")
                        .HasColumnType("text[]");

                    b.Property<string>("QuestionType")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("QuestionId", "SetNumber", "SubjectName");

                    b.ToTable("AssignmentMaster");
                });

            modelBuilder.Entity("rmsapp.rmssysapi.service.Models.Quiz", b =>
                {
                    b.Property<int>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationCodeExpiration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastLoggedIn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("integer");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("QuizSets")
                        .HasColumnType("jsonb");

                    b.Property<DateTime?>("QuizSubmittedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("QuizId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("rmsapp.rmssysapi.service.Models.QuizSubmission", b =>
                {
                    b.Property<int>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CandidateMailId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("MasterAnswers")
                        .HasColumnType("jsonb");

                    b.Property<string>("QuizSets")
                        .HasColumnType("jsonb");

                    b.Property<string>("SubmittedAnswers")
                        .HasColumnType("jsonb");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("QuizId");

                    b.ToTable("QuizSubmission");
                });
#pragma warning restore 612, 618
        }
    }
}
