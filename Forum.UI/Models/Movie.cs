using System;

namespace Forum.UI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Score { get; set; }
        public int Duration { get; set; }
        public string? Genre { get; set; }
        public string? Link { get; set; }
        public string? Poster { get; set; }
        public string? Description { get; set; }
        public string? Actors { get; set; }
    }
}
