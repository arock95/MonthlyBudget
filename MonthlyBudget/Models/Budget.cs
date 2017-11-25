using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class Budget
    {
        public long Id { get; set; }

        public DateTime BudgetDate { get; set; }

        public string User { get; set; }
    }
}
