using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string lastname { get; set; }
        [Required]
        public int city { get; set; }
        [Required]
        public int town { get; set; }
        [Required]
        [StringLength(500)]
        public string address { get; set; }
        [Required]
        [StringLength(255)]
        public string phone { get; set; }

      

        public int? birth_year { get; set; }

        [StringLength(255)]
        public string job { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime added { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string user_id { get; set; }
    }
}