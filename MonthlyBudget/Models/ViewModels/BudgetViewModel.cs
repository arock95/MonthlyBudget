using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;
using System.ComponentModel.DataAnnotations;

namespace MonthlyBudget.Models.ViewModels
{
    public class BudgetViewModel
    {
        public List<BudgetItem> BudgetItems = new List<BudgetItem>();
        public List<string> Categories = new List<string>();
        public DateTime SelectedMonth { get; set; }

        public string Category { get; set; }

        public int Amount { get; set; }

        [Display(Name ="Month")]
        public int BudgetMonth { get; set; }

        [Display(Name = "Year")]
        public int BudgetYear { get; set; }

        public Dictionary<int, string> Months = new Dictionary<int, string>( )
        {
            { 1,"January" },
            { 2,"February" },
            {3,"March" },
            { 4,"April" },
            {5,"May" },
            {6,"June" },
            {7,"July" },
            {8,"August" },
            {9,"September" },
            {10,"October" },
            {11,"November" },
            {12,"December" }
        };

        public List<int> Years = Enumerable.Range(2017, 10).ToList();
    }
}
