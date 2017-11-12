using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class Purchase
    {
        public long Id { get; set; }

        [Required]
        public string PurchaseName { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public string User { get; set; }
    }
}
