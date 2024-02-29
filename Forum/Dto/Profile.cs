using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Dto
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Location { get; set; }
        public string? Gender { get; set; }
    }
}
