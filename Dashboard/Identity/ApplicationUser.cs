using Microsoft.AspNet.Identity.EntityFramework;

namespace QuestionsSYS.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}