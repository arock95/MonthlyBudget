﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class BudgetItem
    {
        public long Id { get; set; }

        [Required]
        public string Category { get; set; }

        public string User { get; set; }

        [Required]
        public int Amount { get; set; }

        public int BudgetMonth { get; set; }
        public int BudgetYear { get; set; }
    }
}
