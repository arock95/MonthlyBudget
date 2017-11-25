using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models.ViewModels
{
    public class PurchaseViewModel
    {
        [Required]
        public string PurchaseName { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public string Category { get; set; }

        public string PurchaseDate { get; set; }

        public List<Purchase> Purchases = new List<Purchase>();

        public List<string> Categories = new List<string>();
    }
}
