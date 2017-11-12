using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Services
{
    public interface IBudgetItemSvc
    {
        void Add(BudgetItem newBudgetItem);
        List<BudgetItem> FindAll(string user, int month, int year);
        int Commit();
        bool Exists(BudgetItem bud);           //checks to see if category already exists for that user
        bool Remove(BudgetItem bud);
    }
}
