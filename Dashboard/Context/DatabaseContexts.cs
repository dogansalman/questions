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
        public DbSet<Customer> customers { get; set; }
        public DbSet<CustomerHistory> customer_history { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Town> towns { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderProduct> order_products { get; set; }

        public DbSet<EmployeeType> employee_type { get; set; }
        public DbSet<Cargo> cargo { get; set; }
        public DbSet<Jobs> jobs { get; set; }
        public DbSet<AuthHistory> auth_history { get; set; }

        public DbSet<TaskHistory> task_history { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<States>().ToTable("States");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Question>().Property(e => e.added).HasColumnType("datetime2");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().Property(e => e.added).HasColumnType("datetime2");
            modelBuilder.Entity<CustomerHistory>().ToTable("CustomerHistory");
            modelBuilder.Entity<CustomerHistory>().Property(e => e.date).HasColumnType("datetime2");
            modelBuilder.Entity<Soruce>().ToTable("Soruce");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
            modelBuilder.Entity<QuestionTasks>().ToTable("QuestionTasks");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Town>().ToTable("Town");
            modelBuilder.Entity<Jobs>().ToTable("Jobs");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().Property(e => e.added).HasColumnType("datetime2");
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<EmployeeType>().ToTable("EmployeeType");
            modelBuilder.Entity<AuthHistory>().ToTable("AuthHistory");
            modelBuilder.Entity<AuthHistory>().Property(e => e.login_date).HasColumnType("datetime2");
            modelBuilder.Entity<AuthHistory>().Property(e => e.logout_date).HasColumnType("datetime2");

            modelBuilder.Entity<TaskHistory>().ToTable("TaskHistory");
            modelBuilder.Entity<TaskHistory>().Property(e => e.added).HasColumnType("datetime2");



        }
       
    }
}