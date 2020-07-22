using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.Models
{
    public class Tasks
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string user_id { get; set; }
        [Required]
        public int question_id { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? called_date { get; set; }
        public bool state { get; set; }
        public bool is_ordered { get; set; } = false;
        public string order_state { get; set; }
        public DateTime? feedback_date { get; set; }
        public DateTime? contact_date { get; set; }
        public string note { get; set; }
    }
}