namespace QuestionsSYS.Migrations
{
    using QuestionsSYS.Identity;
    using Microsoft.AspNet.Identity;
    using System.Data.Entity.Migrations;
    using QuestionsSYS.Context;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class IdentityConfig : DbMigrationsConfiguration<Context.IdentityContexts>
    {
        public IdentityConfig()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        private UserManager<ApplicationUser> userManager;

        protected override void Seed(Context.IdentityContexts context)
        {
            IdentityContexts db = new IdentityContexts();
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);


            // Add roles
            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore);

            if (!roleManager.RoleExists("Admin"))
            {
                ApplicationRole adminRole = new ApplicationRole("Admin", "Sistem yöneticisi");
                roleManager.Create(adminRole);
            }
            if (!roleManager.RoleExists("User"))
            {
                ApplicationRole userRole = new ApplicationRole("User", "Sistem kullanýcýsý, yorum eklemek için gereklidir");
                roleManager.Create(userRole);
            }

            ApplicationUser user = new ApplicationUser();
            user.Name = "Ad";
            user.Surname = "Soyad";
            user.Email = "admin@mail.com";
            user.UserName = "admin";
            IdentityResult Iresult = userManager.Create(user, "password");
            userManager.AddToRole(user.Id, "Admin");

        }
    }



}
