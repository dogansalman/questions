using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class CustomerHistory
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(1000)]
        public string description { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime date { get; set; }
        
        [Required]
        public int customer_id { get; set; }

    }
}