using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class City
    {
        [Key]
        public int id { get; set; }
        public string city_name { get; set; }

    }
}