using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Forum.UI.Controllers
{
    public class HomeController : MyController
    {        
        public IActionResult Index()
        {
            var temp = new List<Category>();
            foreach (var prod in Categories.GetCategories())
            {
                temp.Add(new Category()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description
                });
            }
            return View(temp);
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            // do not use ModelState.IsValid, because PasswordConfirm is not set during login
            if (ModelState["Username"]?.ValidationState == ModelValidationState.Valid && ModelState["Password"]?.ValidationState == ModelValidationState.Valid)
            {
                try
                {
                    var result = Authentication.GetUser(model.Username, model.Password);
                    if (result != null)
                    {
                        LoggedUser = model;
                        LoggedUser.Id = result.Id;
                        LoggedUser.UserType = result.UserType;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            LoggedUser = null;
            var temp = new List<Category>();
            foreach (var prod in Categories.GetCategories())
            {
                temp.Add(new Category()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description
                });
            }
            return View("Index", temp);
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Authentication.AddUser(model.Username, model.Password, UserTypes.User) == 1)
                    {
                        int userId = Authentication.GetUser(model.Username)!.Id;
                        Profiles.AddProfile(userId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Error creating account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}