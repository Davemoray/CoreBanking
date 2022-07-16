using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class TillUser
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Select a User")]
        [Display(Name = "User")]

        public string UserName { get; set; }
        public ApplicationUser User { get; set; }

        public virtual GLAccount GLAccount { get; set; }

        public int GLAccountID { get; set; }
        
    }
}
