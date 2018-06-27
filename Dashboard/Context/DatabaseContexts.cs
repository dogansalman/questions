using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using QuestionsSYS.Models;

namespace QuestionsSYS.Context
{
    public class DatabaseContexts: IdentityDbContext<Identity.ApplicationUser>
    {
        public DatabaseContexts() : base(System.Configuration.ConfigurationManager.AppSettings["connection:name"]) { }


        public DbSet<States> states { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<Soruce> sources { get; set; }

        public DbSet<Tasks> tasks { get; set; }
        public DbSet<QuestionTasks> question_tasks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<States>().ToTable("States");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Soruce>().ToTable("Soruce");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
            modelBuilder.Entity<QuestionTasks>().ToTable("QuestionTasks");
        }
       
    }
}