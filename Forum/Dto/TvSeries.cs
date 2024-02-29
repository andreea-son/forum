using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Dto
{
    public class TvSeries
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime FirstAirDate { get; set; }
        public float Score { get; set; }
        public int AverageEpisodeDuration { get; set; }
        public int NumberOfSeasons { get; set; }
        public string? Genre { get; set; }
        public string? Link { get; set; }
        public string? Poster { get; set; }
        public string? Description { get; set; }
        public string? Actors { get; set; }
    }
}
