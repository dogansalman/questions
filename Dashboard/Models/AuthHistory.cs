using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestionsSYS.Models
{
    public class AuthHistory
    {
        [Key]
        public int id { get; set; }
        public string user_id { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime login_date { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime logout_date{ get; set; }

    }
}