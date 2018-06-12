using System;
using System.ComponentModel.DataAnnotations;


namespace QuestionsSYS.Models
{
    public class Question
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string question { get; set; }
        public string note { get; set; }
        [Required]
        public int state { get; set; }
        public int user_id { get; set; }
        [Required]
        public int source_id { get; set; }
        public DateTime added { get; set; } = DateTime.Now;


    }
}