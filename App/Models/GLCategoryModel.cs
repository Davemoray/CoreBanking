using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{


    public enum MainGLCategory
    {
        Asset=1, Liability, Capital, Income, Expenses
    }

    public class GLCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = ("Category name is required")), MaxLength(40)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Category name should only contain characters and white spaces")]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string CategoryName { get; set; }

        public long CategoryCode { get; set; }

        [Required(ErrorMessage = ("Please enter a description")), MaxLength(150)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Main Account Category")]
        [Required(ErrorMessage = "You have to select a main GL Category")]
        public MainGLCategory MainCategory { get; set; }

    }

}
