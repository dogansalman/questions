using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class TaskListView
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string question { get; set; }
        public string note { get; set; }
        public string phone2 { get; set; }
        public string phone { get; set; }
        public string source { get; set; }
        public string order_state { get; set; }
        public bool state { get; set; }
        public DateTime created_date { get; set; }
    }
}