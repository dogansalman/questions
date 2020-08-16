using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class ReportsView
    {
        public string total_work_time { get; set; }
        public string employee_name {get; set;}
        public int orders { get; set; }
        public float? order_total { get; set; }
        public int tasks { get; set; }

    }
}