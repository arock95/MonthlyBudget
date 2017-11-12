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

        // ******************* CATEGORIES ******************

        [HttpGet]
        [Authorize]
        public IActionResult CreateCategory(string err)
        {
            // grab list of user's existing categories, sort 
            var categories = _categories.FindAll(User.Identity.Name);
            categories.Sort();

            // dump into viewbag so we can show on the page
            ViewBag.Categories = categories;

            // err may be passed in as url parameter, typically by POST if category exists already
            ViewBag.err = err;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateCategory(Category cat)
        {
            // take the user input, create a category with user's identity
            var category = new Category
            {
                Name = cat.Name,
                User = User.Identity.Name.ToString()
            };

            // if the modelstate is okay and the category doesn't already exist
            // add it to the database
            if (ModelState.IsValid && !_categories.Exists(category))
            {
                
                _categories.Add(category);
                _categories.Commit();
                return RedirectToAction("CreateCategory");
            }
            // otherwise, redirect back to the form, and send the error message
            else
            {
                return RedirectToAction("CreateCategory", new { err="Invalid Category!"});
            }

        }

        // ******************* BUDGETS ******************
        [HttpGet]
        [Authorize]
        public IActionResult CreateBudget(int? m, int? y, string err)
        {
            List<string> cats = _categories.FindAll(User.Identity.Name);
            cats.Sort();

            int Month, Year;
            if (y.HasValue && m.HasValue)
            {
                Year = y.Value;
                Month = m.Value;
            }
            else
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }

            List<BudgetItem> items = _budgets.FindAll(User.Identity.Name, Month ,Year );

            List<int> Months = Enumerable.Range(1, 12).ToList();
            int ThisYear = DateTime.Now.Year;
            List<int> Years = Enumerable.Range(ThisYear, 2).ToList();

            ViewBag.Months = new SelectList(Months);
            ViewBag.Years = new SelectList(Years);
            ViewBag.Categories = new SelectList(cats);
            ViewBag.Err = err;

            ViewBag.BudgetItems = items;
            return View(new BudgetItem { Year = Year, Month = Month }); 
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateBudget(BudgetItem bud)
        {
            var Budget = new BudgetItem
            {
                Amount = bud.Amount,
                Year = bud.Year,
                Month = bud.Month,
                User = User.Identity.Name,
                Category = bud.Category
            };

            if (ModelState.IsValid && !_budgets.Exists(Budget))
            {
                _budgets.Add(Budget);
                _budgets.Commit();
                return RedirectToAction("CreateBudget", new { m = bud.Month, y = bud.Year });
            }
            else {
                return RedirectToAction("CreateBudget", new { m = bud.Month, y = bud.Year,
                    err ="Category already exists for this month." });
            }
        }

        // ******************* PURCHASES ******************
        [HttpGet]
        [Authorize]
        public IActionResult EnterPurchase(int? m, int? y, string err)
        {
            List<string> cats = _categories.FindAll(User.Identity.Name);
            cats.Sort();

            int Month, Year;
            if (y.HasValue && m.HasValue)
            {
                Year = y.Value;
                Month = m.Value;
            }
            else
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }
            List<int> Months = Enumerable.Range(1, 12).ToList();
            int ThisYear = DateTime.Now.Year;
            List<int> Years = Enumerable.Range(ThisYear, 2).ToList();

            ViewBag.Months = new SelectList(Months);
            ViewBag.Years = new SelectList(Years);
            ViewBag.Categories = new SelectList(cats);
            ViewBag.err = err;

            return View(new Purchase { Year = Year, Month = Month });
        }

        [HttpPost]
        [Authorize]
        public IActionResult EnterPurchase(Purchase purchase)
        {
            var budget = new BudgetItem {
                Category = purchase.Category,
                Month = purchase.Month,
                Year = purchase.Year,
                User = User.Identity.Name
            };

            var pur = new Purchase
            {
                User = User.Identity.Name,
                Category = purchase.Category,
                Cost = purchase.Cost,
                PurchaseName = purchase.PurchaseName,
                Month = purchase.Month,
                Year = purchase.Year
            };
            if (ModelState.IsValid && _budgets.Exists(budget)) // and the category is budgeted for....
            {
                _purchases.Add(pur);
                _purchases.Commit();
                return RedirectToAction("EnterPurchase");
            }
            else
            {
                return RedirectToAction("EnterPurchase", new {m=purchase.Month, y=purchase.Year,
                    err ="Please make sure the category is budgeted for"});
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
                Month = bud.Month,
                Year = bud.Year,
                Category = bud.Category
            };
            if (_budgets.Remove(budget) == true)
            {
                _budgets.Commit();
                return RedirectToAction("CreateBudget");
            } 
            else
            {
                return RedirectToAction("CreateBudget", new { err = "Unable to remove item" });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeletePurchase(Purchase pur)
        {
            var purchase = new Purchase
            {
                Category = pur.Category,
                User = User.Identity.Name,
                Month = pur.Month,
                Year = pur.Year,
                Cost = pur.Cost
            };

            if (_purchases.Remove(purchase) == true)
            {
                _purchases.Commit();
                return RedirectToAction("EnterPurchase");
            }
            else
            {
                return RedirectToAction("EnterPurchase", new { err = "Unable to remove item"});
            }
        }
        
        [Authorize]
        public IActionResult Index()
        {
            var Month = DateTime.Now.Month;
            var Year = DateTime.Now.Year;
            var usr = User.Identity.Name;



            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
