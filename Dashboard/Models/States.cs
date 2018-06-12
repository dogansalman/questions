using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.Models
{
    public class States
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}