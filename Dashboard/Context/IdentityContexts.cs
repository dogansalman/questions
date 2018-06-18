using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace QuestionsSYS.Context
{
    public class IdentityContexts: IdentityDbContext<Identity.ApplicationUser>
    {
        public IdentityContexts() : base(System.Configuration.ConfigurationManager.AppSettings["connection:name"]) { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
       
    }
}