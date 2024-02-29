using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.UI.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [Display(Name = "Title")] 
        public string? Title { get; set; }
        [Required(ErrorMessage = "Message is required!")]
        [Display(Name = "Title")] 
        public string? Message { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public int TopicId { get; set; }
    }
}
