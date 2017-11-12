﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using MonthlyBudget.Data;

namespace MonthlyBudget.Services
{
    public class SqlBudgetItemData : IBudgetItemSvc
    {
        private BudgetDbContext _db;

        public SqlBudgetItemData(BudgetDbContext db)
        {
            _db = db;
        }

        public void Add(BudgetItem newBudgetItem)
        {
            _db.BudgetItems.Add(newBudgetItem);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public bool Exists(BudgetItem bud)
        {
            if (_db.BudgetItems.FirstOrDefault(x => x.User == bud.User && x.Category == bud.Category
                    && x.Month == bud.Month && x.Year == bud.Year) != null) { return true; }
            else return false;
        }

        public List<BudgetItem> FindAll(string user, int month, int year)
        {
            return _db.BudgetItems.Where(x => x.User == user && x.Year == year && x.Month == month).ToList();
        }

        public bool Remove(BudgetItem bud)
        {
            var removeThis = _db.BudgetItems.FirstOrDefault(x => x.User == bud.User && x.Category == bud.Category
                    && x.Month == bud.Month && x.Year == bud.Year);
            if (removeThis != null)
            {
                _db.BudgetItems.Remove(removeThis);
                return true;
            }
            else return false;
        }
    }
}
