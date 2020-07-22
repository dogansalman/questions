using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.ModelViews
{
    public class CustomerList
    {
 
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime added { get; set; } = DateTime.Now;
        public string user_id { get; set; }
    }
}