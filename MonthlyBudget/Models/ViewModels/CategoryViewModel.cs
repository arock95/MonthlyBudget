using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonthlyBudget.Models.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage ="Category name is required")]
        [RegularExpression("^[a-zA-Z0-9\\s]*$")]
        [StringLength(50, ErrorMessage ="Category name can be between 5 and 50 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public List<string> UserCategories;

        public int? NavigateBackMonth { get; set; }

        public int? NavigateBackYear { get; set; }

        public string Error;
    }
}
