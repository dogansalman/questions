using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace QuestionsSYS.Context
{
    public class IdentityContexts: IdentityDbContext<Identity.ApplicationUser>
    {
        public IdentityContexts() : base("ConnectionStrStaff") { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
       
    }
}