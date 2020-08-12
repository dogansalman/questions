using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsSYS.Models
{
    public class Order
    {
        [Key]
        public int id { get; set; }
        public string user_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime added { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string address { get; set; }
        [Required]
        public int city { get; set; }
        [Required]
        public int town { get; set; }
        [Required]
        public int task_id { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        [Required]
        public string last_name { get; set; }
        [Required]
        [StringLength(255)]
        public string phone { get; set; }
        public int customer_id { get; set; }
        public float total { get; set; }
        public float discount { get; set; }

        [StringLength(255)]
        public string state { get; set; } = "Onay Bekliyor";
        public List<OrderProduct> order_products {get; set; }
        
        [StringLength(255)]
        public string cargo { get; set; }

        [StringLength(255)]
        public string cargo_barcode { get; set; }
    }
}