using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using MonthlyBudget.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MonthlyBudget.Models.ViewModels;

namespace MonthlyBudget.Controllers
{
    public class CategoryController:Controller
    {
        private ICategorySvc _categories;
        public CategoryController(ICategorySvc categories)
        {
            _categories = categories;
        }
        // ******************* CATEGORIES ******************
        [HttpGet]
        [Authorize]
        public IActionResult Index(string err)
        {
            CategoryViewModel CatViewModel = new CategoryViewModel
            {
                UserCategories = _categories.FindAll(User.Identity.Name).ToList(),
                Error = err
            };
            CatViewModel.UserCategories.Sort();
            /* grab list of user's existing categories, sort 
            var categories = _categories.FindAll(User.Identity.Name).ToList();
            categories.Sort();

            // dump into viewbag so we can show on the page
            ViewBag.Categories = categories;

            // err may be passed in as url parameter, typically by POST if category exists already
            ViewBag.err = err; */
            return View(CatViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index([Bind(include: "Name")]CategoryViewModel cat)
        {
            // if the modelstate is okay and the category doesn't already exist
            // add it to the database
            if (ModelState.IsValid)
            {
                // take the user input, create a category with user's identity
                var category = new Category
                {
                    Name = cat.Name,
                    User = User.Identity.Name.ToString()
                };
                if (!_categories.Exists(category))
                {
                    _categories.Add(category);
                    _categories.Commit();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { err = "Category already exists" });
            }
            // otherwise, redirect back to the form, and send the error message
            return RedirectToAction("Index", new { err = "Category names must be between 5 and 50 characters long, and can only contain " +
                "letters, numbers and spaces" });
        }
    }
}
