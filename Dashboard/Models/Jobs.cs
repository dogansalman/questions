using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class Jobs
    {
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        [Required]
        [Index("Job_Name", IsUnique = true)]
        public string job_name{ get; set; }
    }
}