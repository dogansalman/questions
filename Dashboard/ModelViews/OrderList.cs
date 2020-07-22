using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.ModelViews
{
    public class OrderList
    {
 
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string phone { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime added { get; set; }
        public string user_id { get; set; }
        public string order_state { get; set; }
        public float total { get; set; }
    }
}