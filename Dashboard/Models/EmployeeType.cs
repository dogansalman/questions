using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class EmployeeType
    {
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Required]
        [Index("TypeName", IsUnique = true)]
        public string type_name{ get; set; }
    }
}