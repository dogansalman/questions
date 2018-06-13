using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Models;
using QuestionsSYS.Identity;

namespace QuestionsSYS.ModelViews
{
    public class QuestionView
    {

        public int id { get; set; }
        public string question { get; set; }
        public string note { get; set; }
        public int state { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string phone2 { get; set; }
        public ApplicationUser user { get; set; }
        public int source_id { get; set; }
        public DateTime added { get; set; } = DateTime.Now;
    }
}