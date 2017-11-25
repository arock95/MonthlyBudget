using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class BudgetCategory
    {
        public long Id { get; set; }

        public long BudgetId { get; set; }

        public long CategoryId { get; set; }

        public int Amount { get; set; }
    }
}
