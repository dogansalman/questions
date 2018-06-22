using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class TaskAdd
    {
        [Required]
        public string user_id { get; set; }
        [Required]
        public string[] questions { get; set; }
    }
}