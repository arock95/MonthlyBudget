using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Models.ViewModels
{
    public class ReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int FromMonth { get; set; }
        public int FromYear { get; set; }

        public List<ReportItem> ReportItems = new List<ReportItem>();

        public Dictionary<int, string> Months = new Dictionary<int, string>()
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
