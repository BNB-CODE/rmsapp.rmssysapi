using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace rmsapp.rmssysapi.repository.Migrations
{
    public partial class versionchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentMaster",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    SubjectName = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    Question = table.Column<string>(type: "text", nullable: true),
                    QuestionType = table.Column<string>(type: "text", nullable: true),
                    QuestionOptions = table.Column<string[]>(type: "text[]", nullable: true),
                    QuestionAnswers = table.Column<string[]>(type: "text[]", nullable: true),
                    QuestionAnswersIds = table.Column<string[]>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentMaster", x => new { x.QuestionId, x.Version, x.SubjectName });
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    CandidateId = table.Column<string>(type: "text", nullable: false),
                    CandidateName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.CandidateId);
                });

            migrationBuilder.CreateTable(
                name: "QuizSubmissions",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateId = table.Column<string>(type: "text", nullable: true),
                    QuizSets = table.Column<string>(type: "jsonb", nullable: true),
                    SubmittedAnswers = table.Column<string>(type: "jsonb", nullable: true),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    TotalAnsweredQuestions = table.Column<int>(type: "integer", nullable: false),
                    TotalUnAnsweredQuestions = table.Column<int>(type: "integer", nullable: false),
                    TotalCorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    TotalInCorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSubmissions", x => x.QuizId);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuizTopic = table.Column<string>(type: "text", nullable: true),
                    CandidateId = table.Column<string>(type: "text", nullable: true),
                    QuizSets = table.Column<string>(type: "jsonb", nullable: true),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    QuizTimeInMinutes = table.Column<int>(type: "integer", nullable: false),
                    ConfirmationCode = table.Column<string>(type: "text", nullable: true),
                    ConfirmationCodeExpiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    QuizSubmittedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    LastLoggedIn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentMaster");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "QuizSubmissions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
