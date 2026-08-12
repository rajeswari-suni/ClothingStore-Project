namespace ClothingStore_Project.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public string? ProductName { get; set; }

        public int Stars { get; set; }
        public string? UserName { get; set; }

        public string? Review { get; set; } 
        public DateTime? ReviewDate { get; set; } 
              
    }
}
