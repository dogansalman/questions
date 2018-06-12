using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace QuestionsSYS.Migrations
{
    internal sealed class DatabaseConfig : DbMigrationsConfiguration<Context.DatabaseContexts>
    {

        public DatabaseConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }
    }

}