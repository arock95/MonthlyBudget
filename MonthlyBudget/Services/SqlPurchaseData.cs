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

        public List<Purchase> FindByMonth(int month, int year, string user)
        {
            return _db.Purchases.Where(x => x.Month == month && x.Year == year && x.User == user).ToList();
        }

        public bool Remove(Purchase pur)
        {
            var removeThis = _db.Purchases.FirstOrDefault(x => x.User == pur.User && x.Year==pur.Year &&
                            x.Month == pur.Month && x.Category == pur.Category && x.Cost == pur.Cost);
            if (removeThis != null)
            {
                _db.Purchases.Remove(removeThis);
                return true;
            }
            else return false;
        }
    }
}
