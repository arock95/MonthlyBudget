using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using MonthlyBudget.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonthlyBudget.Models.ViewModels;

namespace MonthlyBudget.Controllers
{
    public class PurchaseController:Controller
    {
        private ICategorySvc _categories;
        private IBudgetItemSvc _budgets;
        private IPurchaseSvc _purchases;

        public PurchaseController(ICategorySvc categories, IBudgetItemSvc budgets, IPurchaseSvc purchases)
        {
            _categories = categories;
            _budgets = budgets;
            _purchases = purchases;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(string err, int? m, int? y)
        {
            DateTime LookupMonth;
            if (m.HasValue && y.HasValue)
            {
                LookupMonth = new DateTime((int)y, (int)m, 1);
                ViewBag.m = m;
                ViewBag.y = y;
            }
            else
            {
                LookupMonth = DateTime.Now;
                ViewBag.m = LookupMonth.Month;
                ViewBag.y = LookupMonth.Year;
            }
            

            var ViewModel = new PurchaseViewModel {
                Purchases = _purchases.FindByMonth(LookupMonth, User.Identity.Name),
                Categories = _categories.FindAll(User.Identity.Name),
                PurchaseDate = DateTime.Now.ToString("MM/dd/yyyy")
            };
            ViewModel.Categories.Sort();
            ViewModel.Purchases = ViewModel.Purchases.OrderBy(x => x.Category).ToList();

            ViewBag.err = err;

            return View(ViewModel);//(new Purchase { Year = Year, Month = Month });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(PurchaseViewModel purchase)
        {
            

            if (ModelState.IsValid) 
            {
                DateTime PurchaseDate = DateTime.Parse(purchase.PurchaseDate);
                var budget = new BudgetItem
                {
                    Category = purchase.Category,
                    BudgetYear = PurchaseDate.Year,
                    BudgetMonth = PurchaseDate.Month,
                    User = User.Identity.Name
                };

                var pur = new Purchase
                {
                    User = User.Identity.Name,
                    Category = purchase.Category,
                    Cost = purchase.Cost,
                    PurchaseName = purchase.PurchaseName,
                    PurchaseDate = PurchaseDate
                };
                if (_budgets.Exists(budget))
                {
                    _purchases.Add(pur);
                    _purchases.Commit();
                    return RedirectToAction("Index", new { m = PurchaseDate.Month, y = PurchaseDate.Year });
                }
                else
                {
                    return RedirectToAction("Index", new
                    {
                        err = "Please make sure the category is budgeted for"
                    });
                }
            }
            else
            {
                return RedirectToAction("Index", new
                {
                    err = "Please make sure all entered values are valid"
                });
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult DeletePurchase(Purchase pur)
        {
            if (ModelState.IsValid)
            {
                var purchase = new Purchase
                {
                    Category = pur.Category,
                    User = User.Identity.Name,
                    PurchaseDate = pur.PurchaseDate,
                    Cost = pur.Cost
                };

                if (_purchases.Remove(purchase) == true)
                {
                    _purchases.Commit();
                    return RedirectToAction("Index", new { m=pur.PurchaseDate.Month, y=pur.PurchaseDate.Year});
                }
                else
                {
                    return RedirectToAction("Index", new { err = "Unable to remove item" });
                }

            }
            else
            {
                return RedirectToAction("Index", new { err = "Please ensure all entered values are valid" });
            }
            
        }
    }
}
