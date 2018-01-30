using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using MonthlyBudget.Data;

namespace MonthlyBudget.Services
{
    public class SqlCategoryData : ICategorySvc
    {
        private BudgetDbContext _db;

        public SqlCategoryData(BudgetDbContext db)
        {
            _db = db;
        }

        public void Add(Category newCategory)
        {
            _db.Categories.Add(newCategory);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public bool Exists(Category cat)
        {
            if (_db.Categories.FirstOrDefault (x => x.User == cat.User && x.Name == cat.Name) != null) { return true; }
            else return false;
        }

        public List<string> FindAll(string user)
        {
            return _db.Categories.Where(x => x.User == user).Select(x => x.Name).ToList();
        }

        public bool Remove(Category cat)
        {
            var removeThis = _db.Categories.FirstOrDefault(x => x.Name == cat.Name && x.User == cat.User);
            if (removeThis != null)
            {
                _db.Categories.Remove(removeThis);
                return true;
            }
            else { return false; }
        }
    }
}
