using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.ModelViews
{
    public class QuestionList
    {
        [Required]
        public int[] questions { get; set; }
    }
}