using App.Models;
using App.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            Users = new List<ApplicationUser>();
           
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public bool Status { get; set; }
    }
}
