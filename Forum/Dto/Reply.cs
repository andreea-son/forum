using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Dto
{
    public class Reply
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
        public DateTime Created { get; set; }
    }
}
