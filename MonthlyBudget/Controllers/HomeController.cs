using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonthlyBudget.Models;
using MonthlyBudget.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
