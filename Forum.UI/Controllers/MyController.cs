using Microsoft.AspNetCore.Mvc;
using Forum.UI.Models;

namespace Forum.UI.Controllers
{
    public class MyController : Controller
    {
        public static User? LoggedUser { get; set; } 
    }
}
