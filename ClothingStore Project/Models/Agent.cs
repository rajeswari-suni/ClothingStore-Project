using System.ComponentModel.DataAnnotations;

namespace ClothingStore_Project.Models
{
    public class Agent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Agent Code is required")]
        public string AgentCode { get; set; }

        public string MembershipType { get; set; }

        public decimal SubscriptionAmount { get; set; }
        public int DiscountPercentage { get; set; }
    }
}