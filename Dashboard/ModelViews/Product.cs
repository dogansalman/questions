using System;
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.ModelViews
{
    public class Product
    {
 
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string product_name { get; set; }
        [Required]
        public float price { get; set; }
        [Required]
        public int qty { get; set; }
    }
}