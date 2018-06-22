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
        public string order_state { get; set; }
        public int order_unit { get; set; }
        public double order_price { get; set; }
        public double offer_price { get; set; }
        public DateTime? feedback_date { get; set; }
        public string note { get; set; }
    }
}