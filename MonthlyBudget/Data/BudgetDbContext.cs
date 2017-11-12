using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Data
{
    public class BudgetDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public BudgetDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
