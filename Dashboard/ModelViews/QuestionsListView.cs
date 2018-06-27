using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class QuestionsListView
    {
    
        public int id { get; set; }
        public string question { get; set; }
        public string note { get; set; }
        public bool state { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string phone2 { get; set; }
        public string user_id { get; set; }
        public string source { get; set; }
        public DateTime added { get; set; } = DateTime.Now;
        public string user_fullname { get; set; }
    }
}