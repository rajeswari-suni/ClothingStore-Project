using System;

namespace ClothingStore_Project.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string? ProductName { get; set; }

        public int Price { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public int Quantity { get; set; }

        public string? BuyerName { get; set; }

        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }="Placed";

        public bool IsReviewed { get; set; } = false;

        public string? CustomerType { get; set; }
        public string? UserMobile { get; set; }

    }
}
