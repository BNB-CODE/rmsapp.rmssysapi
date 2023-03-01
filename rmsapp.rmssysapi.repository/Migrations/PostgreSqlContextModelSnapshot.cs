﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using rmsapp.rmssysapi.repository;

namespace rmsapp.rmssysapi.repository.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    partial class PostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("CandidateName")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

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

                    b.Property<string>("Version")
                        .HasColumnType("text");

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

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("QuestionId", "Version", "SubjectName");

                    b.ToTable("AssignmentMaster");
                });

            modelBuilder.Entity("rmsapp.rmssysapi.service.Models.Quiz", b =>
                {
                    b.Property<int>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CandidateId")
                        .HasColumnType("text");

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationCodeExpiration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastLoggedIn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("integer");

                    b.Property<string>("QuizSets")
                        .HasColumnType("jsonb");

                    b.Property<DateTime?>("QuizSubmittedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("QuizTimeInMinutes")
                        .HasColumnType("integer");

                    b.Property<string>("QuizTopic")
                        .HasColumnType("text");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("integer");

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

                    b.Property<string>("CandidateId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("QuizSets")
                        .HasColumnType("jsonb");

                    b.Property<string>("SubmittedAnswers")
                        .HasColumnType("jsonb");

                    b.Property<int>("TotalAnsweredQuestions")
                        .HasColumnType("integer");

                    b.Property<int>("TotalCorrectAnswers")
                        .HasColumnType("integer");

                    b.Property<int>("TotalInCorrectAnswers")
                        .HasColumnType("integer");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("integer");

                    b.Property<int>("TotalUnAnsweredQuestions")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("QuizId");

                    b.ToTable("QuizSubmissions");
                });
#pragma warning restore 612, 618
        }
    }
}
