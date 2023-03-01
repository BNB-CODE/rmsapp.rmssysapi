using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace rmsapp.rmssysapi.repository
{

    [ExcludeFromCodeCoverage]
    public class PostgreSqlContext: DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {

        }
        public DbSet<MasterQuiz> AssignmentMaster { get; set; }
        public DbSet<Candidate> Candidate { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(_tableConf.DATABASESCHEMA);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MasterQuiz>()
           .HasKey(c => new { c.QuestionId, c.Version, c.SubjectName});
            modelBuilder.Entity<Candidate>()
            .HasKey(c => new { c.CandidateId});
            modelBuilder.Entity<Quiz>()
            .HasKey(c => new { c.QuizId });
            modelBuilder.Entity<Quiz>()
                .Property(x => x.QuizSets)
                .HasColumnType("jsonb");

            modelBuilder.Entity<QuizSubmission>()
            .HasKey(c => new { c.QuizId });
            modelBuilder.Entity<Quiz>()
                .Property(x => x.QuizSets)
                .HasColumnType("jsonb");
            modelBuilder.Entity<QuizSubmission>()
                .Property(x => x.SubmittedAnswers)
                .HasColumnType("jsonb");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
