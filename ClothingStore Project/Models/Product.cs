using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore_Project.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
  
        public string ProductName { get; set; } = string.Empty;

        public int Price { get; set; }

        public string Brand { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public string Colour { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? SellerId { get; set; }
    }
}

