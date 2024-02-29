using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Dto
{
    public class Subject
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public int TopicId { get; set; }
        public List<Reply> Replies { get; set; } = new();
    }
}
