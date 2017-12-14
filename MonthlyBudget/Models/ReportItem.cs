using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class ReportItem
    {
        // helper class for Index View Model
        public string CategoryName { get; set; }

        public int BudgetAmount { get; set; }

        public int SpentAmount { get; set; }

        public int Difference { get; set; }
    }
}
