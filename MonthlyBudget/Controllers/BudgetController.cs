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
    public class BudgetController:Controller
    {
        private ICategorySvc _categories;
        private IBudgetItemSvc _budgets;
       // private IPurchaseSvc _purchases;

        public BudgetController(ICategorySvc categories, IBudgetItemSvc budgets)
        {
            _categories = categories;
            _budgets = budgets;
            //_purchases = purchases;
        }

        // ******************* BUDGETS ******************
        [HttpGet]
        [Authorize]
        public IActionResult Index(string err, int? m, int? y)
        {
            var ViewModel = new BudgetViewModel();

            ViewModel.Categories = _categories.FindAll(User.Identity.Name);
            ViewModel.Categories.Sort();

            if (m.HasValue)
                ViewModel.BudgetMonth = (int)m;
            else
                ViewModel.BudgetMonth = DateTime.Now.Month;

            if (y.HasValue)
                ViewModel.BudgetYear = (int)y;
            else
                ViewModel.BudgetYear = DateTime.Now.Year;

            ViewModel.BudgetItems = _budgets.FindAll(User.Identity.Name, ViewModel.BudgetMonth, ViewModel.BudgetYear);

            ViewBag.Err = err;

            return View(ViewModel);// new BudgetItem { Year = Year, Month = Month }); 
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(BudgetViewModel bud)
        {
            var Budget = new BudgetItem
            {
                Amount = bud.Amount,
                BudgetMonth = bud.BudgetMonth,
                BudgetYear = bud.BudgetYear,
                User = User.Identity.Name,
                Category = bud.Category
            };

            if (ModelState.IsValid && !_budgets.Exists(Budget))
            {
                _budgets.Add(Budget);
                _budgets.Commit();
                return RedirectToAction("Index", new { m = bud.BudgetMonth, y = bud.BudgetYear });//, new { m = bud.Month, y = bud.Year });
            }
            else
            {
                return RedirectToAction("Index", new { m = bud.BudgetMonth, y = bud.BudgetYear,
                                                        err = "Category already exists for this month." });
            }
        }

        // ******************* DELETES ******************
        [HttpPost]
        [Authorize]
        public IActionResult DeleteBudget(BudgetItem bud)
        {
            var budget = new BudgetItem
            {
                User = User.Identity.Name,
                BudgetMonth = bud.BudgetMonth,
                BudgetYear = bud.BudgetYear,
                Category = bud.Category
            };
            if (_budgets.Remove(budget) == true)
            {
                _budgets.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { err = "Unable to remove item" });
            }
        }
    }
}
