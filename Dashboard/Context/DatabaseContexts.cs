﻿using System.Data.Entity;
using QuestionsSYS.Models;

namespace QuestionsSYS.Context
{
    public class DatabaseContexts : DbContext
    {


        public DatabaseContexts() : base("Name=ConnectionStr")
        {
            Database.SetInitializer<DatabaseContexts>(null);

        }


        public DbSet<States> states { get; set; }
        public DbSet<Question> questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<States>().ToTable("States");
            modelBuilder.Entity<Question>().ToTable("Questions");
        }
    }
      
}
