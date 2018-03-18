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

        private IPurchaseSvc _purchases;

        public ReportController(ICategorySvc categories, IPurchaseSvc purchases)
        {
            _categories = categories;
            _purchases = purchases;
        }

        [HttpGet]
        public IActionResult Index(string err)
        {
            var viewModel = new AllPurchaseViewModel
            {
                Categories = _categories.FindAll(User.Identity.Name),
                ToDate = DateTime.Now.ToString("MM/dd/yyyy"),
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy")
            };
            viewModel.Categories.Add("*");
            viewModel.Categories.Sort();
            if (err != "")
            {
                ViewBag.err = err;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string ToDate, string FromDate, string Category)
        {
            var viewModel = new AllPurchaseViewModel();

            if (DateTime.TryParse(ToDate, out DateTime ToDt) && DateTime.TryParse(FromDate, out DateTime FromDt) && 
                ToDt  > FromDt)
            {
                viewModel.Categories = _categories.FindAll(User.Identity.Name); // duplicate code, lazy
                if (Category == "*")
                {
                    viewModel.Purchases = _purchases.FindAllByDateRange(FromDt, ToDt, User.Identity.Name);
                }
                else
                {
                    if (viewModel.Categories.Contains(Category))
                    {
                        viewModel.Purchases = _purchases.FindByCategoryAndDateRange(FromDt, ToDt, Category, User.Identity.Name);
                    }
                    else
                    {
                        return RedirectToAction("Index", new { err = "Invalid category" });
                    }
                    
                }
            }
            else
            {
                //dates are in bad format...redirect with error msg
                return RedirectToAction("Index", new { err="Invalid dates" });
            }

            viewModel.FromDate = FromDate;
            viewModel.ToDate = ToDate;
            
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