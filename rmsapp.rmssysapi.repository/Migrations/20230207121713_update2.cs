using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace rmsapp.rmssysapi.repository.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizSubmission",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateMailId = table.Column<string>(type: "text", nullable: true),
                    QuizSets = table.Column<string>(type: "jsonb", nullable: true),
                    SubmittedAnswers = table.Column<string>(type: "jsonb", nullable: true),
                    MasterAnswers = table.Column<string>(type: "jsonb", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSubmission", x => x.QuizId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizSubmission");
        }
    }
}
