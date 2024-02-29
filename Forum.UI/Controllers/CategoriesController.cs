using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Forum.UI.Controllers
{
    public class CategoriesController : MyController
    {
        public ActionResult AllCategories()
        {
            var temp = new List<Category>();
            foreach (var cat in Categories.GetCategories())
            {
                temp.Add(new Category()
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description
                });
            }
            return View(temp);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Categories.AddCategory(model.Name, model.Description) == 1)
                        return RedirectToAction("AllCategories", "Categories");
                    else
                        ModelState.AddModelError(string.Empty, "Error adding category");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult DeleteCategory(int id)
        {
            Categories.DeleteCategory(id);
            return RedirectToAction("AllCategories", "Categories");
        }
    }
}
