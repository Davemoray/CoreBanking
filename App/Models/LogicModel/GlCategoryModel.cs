using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.LogicModel
{
   

        public enum MainGlCategory
        {
            Asset, Liability, Capital, Income, Expenses
        }

        public class GlCategory
        {
            [Key]
            public int CategoryID { get; set; }

            [Required(ErrorMessage = ("Category name is required")), MaxLength(40)]
            [Display(Name = "CategoryName")]
            public string CategoryName { get; set; }

            [Required(ErrorMessage = ("Please enter a description")), MaxLength(150)]
            [Display(Name = "Description")]
            public string Description { get; set; }

            [Required(ErrorMessage = "You have to select a main GL Category")]
            [Display(Name ="Main GL Category")]
            public MainGlCategory MainCategory { get; set; }

            public DbSet<GlCategory> GlCategories { get; set; }
        }
    
}
