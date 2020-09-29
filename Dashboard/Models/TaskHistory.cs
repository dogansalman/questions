using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestionsSYS.Models
{
    public class TaskHistory
    {
        [Key]
        public int id { get; set; }
        public int task_id { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime added { get; set; }
        public string note { get; set; }

        public string state { get; set; }
    }
}