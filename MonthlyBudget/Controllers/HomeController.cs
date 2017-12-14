using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonthlyBudget.Models;
using MonthlyBudget.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MonthlyBudget.Models.ViewModels;

namespace MonthlyBudget.Controllers
{
    public class HomeController : Controller
    {
        private ICategorySvc _categories;
        private IBudgetItemSvc _budgets;
        private IPurchaseSvc _purchases;

        public HomeController(ICategorySvc categories, IBudgetItemSvc budgets, IPurchaseSvc purchases)
        {
            _categories = categories;
            _budgets = budgets;
            _purchases = purchases;
        }

        
        [Authorize]
        [HttpGet]
        public IActionResult Index(int? m, int? y)
        {
            var ViewModel = new ReportViewModel();

            ViewModel.month = (m.HasValue) ? (int)m : DateTime.Now.Month;
            ViewModel.year = (y.HasValue) ? (int)y : DateTime.Now.Year;

            List<BudgetItem> bud = _budgets.FindAll(User.Identity.Name, ViewModel.month, ViewModel.year);
            foreach (var b in bud)
            {
                List<Purchase> pur = _purchases.FindByMonthAndCategory(new DateTime(ViewModel.year, ViewModel.month, 1), User.Identity.Name,
                    b.Category);
                var sum = 0;
                foreach (var p in pur)
                {
                    if (p.Category == b.Category)
                    {
                        sum += p.Cost;
                    }
                }
                var report = new ReportItem()
                {
                    CategoryName = b.Category,
                    BudgetAmount = b.Amount,
                    SpentAmount = sum,
                    Difference = b.Amount - sum
                };
                ViewModel.ReportItems.Add(report);
            }

            return View(ViewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
