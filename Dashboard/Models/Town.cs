using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QuestionsSYS.Models
{
    public class Town
    {
        [Key]
        public int id { get; set; }
        public string town_name { get; set; }
        public int city_id { get; set; }
    }
}