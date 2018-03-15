using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Services
{
    public interface IPurchaseSvc
    {
        void Add(Purchase newPurchase);
        List<Purchase> FindByMonth(DateTime PurchaseDate, string user);
        List<Purchase> FindByMonthAndCategory(DateTime PurchaseDate, string user, string category);
        int Commit();                       //writes the changes to the database
        bool Remove(Purchase pur);
        List<Purchase> FindByCategoryAndDateRange(DateTime From, DateTime To, string category);
        List<Purchase> FindAllByDateRange(DateTime From, DateTime To);
    }
}
