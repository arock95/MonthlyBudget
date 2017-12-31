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
            ViewModel.BudgetItems = ViewModel.BudgetItems.OrderBy(x => x.Category).ToList();
            int TotalBudgetedAmount = 0;
            foreach (var b in ViewModel.BudgetItems)
            {
                TotalBudgetedAmount += b.Amount;
            }
            ViewModel.BudgetItems.Add(new BudgetItem { Category = "Total", Amount=TotalBudgetedAmount});

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
                return RedirectToAction("Index", new { m = bud.BudgetMonth, y = bud.BudgetYear });
            }
            else
            {
                return RedirectToAction("Index", new { err = "Unable to remove item", m=bud.BudgetMonth, y=bud.BudgetYear });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CopyLastMonth(int BudgetMonth, int BudgetYear)
        {
            // figure out 'last month/yr'
            int lastMonth, lastYear;

            if (BudgetMonth == 1)
            {
                lastMonth = 12;
                lastYear = BudgetYear -1;
            }
            else
            {
                lastMonth = BudgetMonth - 1;
                lastYear = BudgetYear;
            }

            var lastMonthsBudget = _budgets.FindAll(User.Identity.Name, lastMonth, lastYear);
            var thisMonthsBudget = _budgets.FindAll(User.Identity.Name, BudgetMonth, BudgetYear);

            if (lastMonthsBudget.Count == 0) { return RedirectToAction("Index", new { err = "No budget for last month!", m=BudgetMonth,
                                                                        y=BudgetYear}); }

            // first delete this month's budget
            foreach (var b in thisMonthsBudget)
            {
                _budgets.Remove(new BudgetItem {Category=b.Category, Amount=b.Amount, User=User.Identity.Name,
                                                BudgetMonth = b.BudgetMonth, BudgetYear=b.BudgetYear});
            }

            // then copy last month's budget to this month's
            foreach (var b in lastMonthsBudget)
            {
                _budgets.Add(new BudgetItem { Category = b.Category, Amount=b.Amount, User = User.Identity.Name, BudgetYear = BudgetYear,
                                            BudgetMonth = BudgetMonth});
                
            }
            _budgets.Commit();
            return RedirectToAction("Index", new { m=BudgetMonth, y=BudgetYear});
        }

    }
}
