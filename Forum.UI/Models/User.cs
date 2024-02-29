using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Forum.UI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required and must be maximum 100 symbols!")]
        [Display(Name = "Username")]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required and must be maximum 100 symbols!")]
        [StringLength(100)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Compare("Password", ErrorMessage = "Confirm password doesn't match!")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; set; }

        public UserTypes UserType { get; set; }
        public string ImagePath { get; set; } = "./images/noimage.png";
    }
}
