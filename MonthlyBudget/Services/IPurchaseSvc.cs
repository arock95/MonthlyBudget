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
        int Commit();                       //writes the changes to the database
        bool Remove(Purchase pur);
    }
}
