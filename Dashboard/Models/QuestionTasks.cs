using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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