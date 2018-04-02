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
        public IActionResult Index(int? m, int? y, int? fm, int? fy, string err)
        {
            ViewBag.Err = err;

            var ViewModel = new ReportViewModel
            {
                Month = (m.HasValue) ? (int)m : DateTime.Now.Month,
                Year = (y.HasValue) ? (int)y : DateTime.Now.Year,
                FromMonth = (fm.HasValue) ? (int)fm : DateTime.Now.Month,
                FromYear = (fy.HasValue) ? (int)fy : DateTime.Now.Year
            };

            List<BudgetItem> bud = new List<BudgetItem>();
            List<BudgetItem> range= new List<BudgetItem>();
            int totalBudget = 0, totalSpent = 0;

            // if everything is valid, start processing
            if (Helper.IsValidMonth(ViewModel.Month) && Helper.IsValidMonth(ViewModel.FromMonth) &&
                Helper.IsValidYear(ViewModel.Year) && Helper.IsValidYear(ViewModel.FromYear) 
                && Helper.ToLaterThanFrom(ViewModel.Month,ViewModel.Year,ViewModel.FromMonth,ViewModel.FromYear))
            {
                if (ViewModel.Month == ViewModel.FromMonth && ViewModel.Year == ViewModel.FromYear)
                {
                    bud = _budgets.FindAll(User.Identity.Name, ViewModel.Month, ViewModel.Year);
                    
                }
                else
                {
                    range = _budgets.FindAllRange(User.Identity.Name, ViewModel.Month, ViewModel.Year,
                        ViewModel.FromMonth, ViewModel.FromYear);
                    // now consolidate...
                    var counts = from r in range
                                 group r by r.Category into g
                                 select new {
                                     g.Key,
                                     amt = g.Sum(x => x.Amount)
                                 };
                    foreach (var c in counts)
                    {
                        bud.Add(new BudgetItem { Category = c.Key, Amount = c.amt });
                    }
                }
            }
            else
            {
                //invalid dates
                return RedirectToAction("Index", new { err = "Invalid Dates!" });
            }
            DateTime from = new DateTime(ViewModel.FromYear, ViewModel.FromMonth, 1);

            DateTime to = Helper.ReturnToDate(ViewModel.Month, ViewModel.Year);
            
            string user = User.Identity.Name;

            foreach (var b in bud)
            {
                // findbycategoryanddaterange -- need to set 'to' to last date in month

                List<Purchase> pur = _purchases.FindByCategoryAndDateRange(from, to, b.Category, user);

                var sum = 0;
                foreach (var p in pur)
                {
                    if (p.Category == b.Category)
                    {
                        sum += p.Cost;
                    }
                }
                totalSpent += sum;
                totalBudget += b.Amount;

                var report = new ReportItem()
                {
                    CategoryName = b.Category,
                    BudgetAmount = b.Amount,
                    SpentAmount = sum,
                    Difference = b.Amount - sum
                };
                ViewModel.ReportItems.Add(report);
            }
            
            ViewModel.ReportItems = ViewModel.ReportItems.OrderBy(x => x.CategoryName).ToList();
            ViewModel.ReportItems.Add(new ReportItem
            {
                CategoryName = "Total",
                BudgetAmount = totalBudget,
                SpentAmount = totalSpent,
                Difference = totalBudget - totalSpent
            });

            return View(ViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(int Month, int Year, int FromMonth, int FromYear)
        {
            return RedirectToAction("Index", new {m=Month, y=Year, fm=FromMonth,fy=FromYear });
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
