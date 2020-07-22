using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QuestionsSYS.Models
{
    public class OrderProduct
    {
        public int id { get; set; }
        [Required]
        [StringLength(255)]
        public string product_name { get; set; }
        [Required]
        public float price { get; set; }
        [Required]
        public int qty { get; set; }
        [ForeignKey("order")]
        public int orderId { get; set; }

        public Order order { get; set; }
    }
}