using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MonthlyBudget.Models.ViewModels;
using MonthlyBudget.Services;

namespace MonthlyBudget.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {   // Dependency injection
        private ICategorySvc _categories;
        private IBudgetItemSvc _budgets;
        private IPurchaseSvc _purchases;

        public ReportController(ICategorySvc categories, IBudgetItemSvc budgets, IPurchaseSvc purchases)
        {
            _categories = categories;
            _budgets = budgets;
            _purchases = purchases;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new AllPurchaseViewModel
            {
                Categories = _categories.FindAll(User.Identity.Name),
                FromDate = DateTime.Now.ToString("MM/dd/yyyy"), // eventually set to 1st of month
                ToDate = DateTime.Now.ToString("MM/dd/yyyy") // eventually set to end of month
            };
            viewModel.Categories.Add("*");
            viewModel.Categories.Sort();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string ToDate, string FromDate, string Category)
        {
            DateTime ToDt, FromDt;
            var viewModel = new AllPurchaseViewModel();

            if (DateTime.TryParse(ToDate, out ToDt) && DateTime.TryParse(FromDate, out FromDt))
            {
                if (Category == "*")
                {
                    viewModel.Purchases = _purchases.FindAllByDateRange(FromDt, ToDt, User.Identity.Name);
                }
                else
                {
                    viewModel.Purchases = _purchases.FindByCategoryAndDateRange(FromDt, ToDt, Category, User.Identity.Name);
                }
            }
            else
            {
                //dates are in bad format...redirect with error msg
            }

            viewModel.FromDate = FromDate;
            viewModel.ToDate = ToDate;
            viewModel.Categories = _categories.FindAll(User.Identity.Name); // duplicate code, lazy
            viewModel.Categories.Add("*");
            viewModel.Categories.Sort();

            foreach(var p in viewModel.Purchases)
            {
                viewModel.Total += p.Cost;
            }

            return View(viewModel);
        }
    }
}