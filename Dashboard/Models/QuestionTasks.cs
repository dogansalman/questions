using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.Models
{
    public class QuestionTasks
    {
        [Key]
        public int id { get; set; }
        public int question_id { get; set; }
        public string user_id { get; set; }
    }
}