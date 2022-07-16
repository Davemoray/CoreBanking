using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class GLAccount
    {
        [Key]
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Account name is required"), MaxLength(40)]
        [Display(Name = "Accont Name")]
        public string AccountName { get; set; }

       
        [Display(Name = "GLAccountCode")]
        public long GLAccountCode { get; set; }

        [Display(Name = "Account Balance")]
        public decimal AccountBalance { get; set; }

        [Required(ErrorMessage = "Please select a GL category")]
        [Display(Name = "Category")]
        public int GlCategoryID { get; set; }

        public virtual GLCategory GLCategory { get; set; }
        public string MainCategory { get; set; }

        [Required(ErrorMessage = "Please select a branch")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
        public virtual Branch Branch { get; set; }


    }
}
