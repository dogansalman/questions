
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.ModelViews
{
    public class PersonnelView
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string color { get; set; }
        public int? employee_type { get; set; }

    }
}