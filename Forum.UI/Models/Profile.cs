namespace Forum.UI.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string ProfilePhoto { get; set; } = "/images/noimage.png";
        public string? Name { get; set; }
        public string? Username { get; set; }
        public int Age { get; set; }
        public string? Location { get; set; }
        public string? Gender { get; set; } 
    }
}
