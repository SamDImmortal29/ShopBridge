using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopbridgeWebAPI.Domain.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        [Required, Column(TypeName = "nvarchar(300)")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
