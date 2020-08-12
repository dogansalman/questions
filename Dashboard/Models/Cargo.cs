using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class Cargo
    {
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Required]
        [Index("Cargo_Name", IsUnique = true)]
        public string cargo_company{ get; set; }
    }
}