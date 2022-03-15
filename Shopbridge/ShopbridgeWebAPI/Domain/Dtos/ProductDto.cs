using System.ComponentModel.DataAnnotations;

namespace ShopbridgeWebAPI.Domain.Dtos
{
    public class ProductDto
    {
        public int Product_Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage = "Invalid Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99)]
        public decimal Price { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
