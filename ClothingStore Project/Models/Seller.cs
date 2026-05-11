using System.ComponentModel.DataAnnotations;

namespace ClothingStore_Project.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string ShopName { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
