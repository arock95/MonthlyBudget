using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MonthlyBudget.Models;
using MonthlyBudget.Services;
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
        public IActionResult Index(string err, int? month, int? year)
        {
            CategoryViewModel CatViewModel = new CategoryViewModel
            {
                UserCategories = _categories.FindAll(User.Identity.Name).ToList(),
                NavigateBackMonth = month,
                NavigateBackYear = year,
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
        public IActionResult Index([Bind(include: "Name, NavigateBackMonth, NavigateBackYear")]CategoryViewModel cat)
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
                    return RedirectToAction("Index", new
                    {
                        month = cat.NavigateBackMonth,
                        year = cat.NavigateBackYear
                    });
                }

                return RedirectToAction("Index", new
                {
                    err = "Category already exists",
                    month = cat.NavigateBackMonth,
                    year = cat.NavigateBackYear
                });
            }
            // otherwise, redirect back to the form, and send the error message
            return RedirectToAction("Index", new
            {
                err = "Category names must be between 5 and 50 characters long, and can only contain letters, numbers and spaces",
                month = cat.NavigateBackMonth,
                year = cat.NavigateBackYear
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteCategory([Bind(include: "Name")]Category cat)
        {
            var remove = new Category {
                Name = cat.Name,
                User = User.Identity.Name
            };
            if (_categories.Remove(remove))
            {
                _categories.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { err = "Could not delete category" });
            }
            
        }
    }
}
