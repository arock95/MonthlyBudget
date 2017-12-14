using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonthlyBudget.Models;

namespace MonthlyBudget.Models.ViewModels
{
    public class ReportViewModel
    {
        public int month { get; set; }
        public int year { get; set; }

        public List<ReportItem> ReportItems = new List<ReportItem>();
    }
}
