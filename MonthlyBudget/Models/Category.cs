using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Models
{
    public class Category
    {
        public long Id { get; set; }

        [Required, MaxLength(50)]
        [Display(Name="Category Name")]
        public string Name { get; set; }

        public string User { get; set; }
    }
}
