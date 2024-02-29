using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.UI.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        [Required(ErrorMessage = "Message is required!")]
        [Display(Name = "Message")]
        public string? Message { get; set; }
        public DateTime Created { get; set; }
    }
}
