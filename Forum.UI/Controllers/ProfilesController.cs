using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Forum.UI.Controllers
{
    public class ProfilesController : MyController
    {
        public IActionResult ViewProfile(int id)
        {
            var currentProfile = Profiles.GetProfile(id);
            Profile profile = new()
            {
                Bio = currentProfile.Bio,
                Age = currentProfile.Age,
                Id = currentProfile.Id,
                Gender = currentProfile.Gender,
                Location = currentProfile.Location,
                Name = currentProfile.Name,
                ProfilePhoto = currentProfile.ProfilePhoto,
                UserId = currentProfile.UserId,
                Username = currentProfile.Username
            };
            return View("Profile", profile);
        }

        [HttpPost]
        public IActionResult UpdateProfile(Profile model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Profiles.UpdateProfile(LoggedUser.Id, model.Bio, model.ProfilePhoto, model.Name, model.Age, model.Location, model.Gender) == 1)
                        return RedirectToAction("ViewProfile", "Profiles", LoggedUser.Id);
                    else
                        ModelState.AddModelError(string.Empty, "Error updating profile");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View("Profile", model);
        }
    }
}
