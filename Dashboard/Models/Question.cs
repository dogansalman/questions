using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class Question
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(5000)]
        public string question { get; set; }
        public string note { get; set; }
        [Required]
        public bool state { get; set; }
        [Required]
        [StringLength(255)]
        public string fullname{ get; set; }
        [StringLength(255)]
        public string phone { get; set; }
        [StringLength(255)]
        public string phone2 { get; set; }
        [Required]
        public string source { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime added { get; set; } = DateTime.Now;


    }
}