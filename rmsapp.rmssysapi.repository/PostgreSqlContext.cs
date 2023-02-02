using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rmsapp.rmssysapi.service.Models;
using System.Diagnostics.CodeAnalysis;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(_tableConf.DATABASESCHEMA);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MasterQuiz>()
           .HasKey(c => new { c.QuestionId, c.SetNumber, c.SubjectName});
            modelBuilder.Entity<Candidate>()
            .HasKey(c => new { c.CandidateId});
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
