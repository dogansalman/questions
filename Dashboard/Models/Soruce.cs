using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestionsSYS.Models
{
    public class Soruce
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}