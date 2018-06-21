using System;
using System.ComponentModel.DataAnnotations;

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
        public int user_id { get; set; }
        [Required]
        public string source { get; set; }
        public DateTime added { get; set; } = DateTime.Now;


    }
}