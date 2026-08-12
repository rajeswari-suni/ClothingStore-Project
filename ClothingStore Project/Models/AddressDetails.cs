using System.ComponentModel.DataAnnotations;

namespace ClothingStore_Project.Models
{
    public class AddressDetails
    {
        [Key]
        public int Id { get; set; }

        public string UserMobile { get; set; } = "";

        public string Name { get; set; } = "";

        public string Mobile { get; set; } = "";

        public string DoorNumber { get; set; } = "";

        public string AddressLine { get; set; } = "";

        public string City { get; set; } = "";

        public string State { get; set; } = "";

        public string? Landmark { get; set; }

        public string Pincode { get; set; } = "";
    }
}
