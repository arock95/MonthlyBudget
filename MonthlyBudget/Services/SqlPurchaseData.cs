using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using MonthlyBudget.Data;

namespace MonthlyBudget.Services
{
    public class SqlPurchaseData : IPurchaseSvc
    {
        private BudgetDbContext _db;

        public SqlPurchaseData(BudgetDbContext db)
        {
            _db = db;
        }

        public void Add(Purchase newPurchase)
        {
            _db.Purchases.Add(newPurchase);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public List<Purchase> FindAllByDateRange(DateTime From, DateTime To, string User)
        {
            return _db.Purchases.Where(x => x.PurchaseDate >= From && x.PurchaseDate <= To && x.User == User).OrderBy(x=>x.Category).ToList();
        }

        public List<Purchase> FindByCategoryAndDateRange(DateTime From, DateTime To, string category, string User)
        {
            return _db.Purchases.Where(x => x.PurchaseDate >= From && x.PurchaseDate <= To 
                                    && x.Category == category && x.User == User).ToList();
        }

        public List<Purchase> FindByMonth(DateTime PurchaseDate, string user)
        {
            return _db.Purchases.Where(x => x.PurchaseDate.Month == PurchaseDate.Month 
                                        && x.PurchaseDate.Year == PurchaseDate.Year && x.User == user).ToList();
        }

        public List<Purchase> FindByMonthAndCategory(DateTime PurchaseDate, string user, string category)
        {
            return _db.Purchases.Where(x => x.PurchaseDate.Month == PurchaseDate.Month
                                        && x.PurchaseDate.Year == PurchaseDate.Year && x.User == user && 
                                        x.Category == category).ToList();
        }

        public bool Remove(Purchase pur)
        {
            var removeThis = _db.Purchases.FirstOrDefault(x => x.User == pur.User && x.PurchaseDate.Year==pur.PurchaseDate.Year &&
                            x.PurchaseDate.Month == pur.PurchaseDate.Month && x.Category == pur.Category && x.Cost == pur.Cost);
            if (removeThis != null)
            {
                _db.Purchases.Remove(removeThis);
                return true;
            }
            else return false;
        }
    }
}
