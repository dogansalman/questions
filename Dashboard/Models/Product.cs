using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QuestionsSYS.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(500)]
        [Index(IsUnique = true)]
        public string product_name { get; set; }
        [Required]
        public float price { get; set; }

    }
}