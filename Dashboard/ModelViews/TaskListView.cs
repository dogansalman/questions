using System;


namespace QuestionsSYS.ModelViews
{
    public class TaskListView
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string question { get; set; }
        public string note { get; set; }
        public string question_note { get; set; }
        public string phone2 { get; set; }
        public string phone { get; set; }
        public string source { get; set; }
        public string order_state { get; set; }
        public string user_id { get; set; }
        public string user_fullname { get; set; }
        public bool state { get; set; }
        public int order_unit { get; set; }
        public double order_price { get; set; }
        public double offer_price { get; set; }
        public DateTime? feedback_date { get; set; }
        public DateTime created_date { get; set; }
    }
}