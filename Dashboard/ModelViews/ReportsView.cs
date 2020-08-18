using QuestionsSYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionsSYS.ModelViews
{
    public class WorkDates
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }

    public class ReportsView
    {
        public string total_work_time { get; set; }
        public string employee_name { get; set; }
        public int orders { get; set; }
        public float? order_total { get; set; }
        public int tasks { get; set; }
        public List<AuthHistory> dates {get; set;}

    }
}