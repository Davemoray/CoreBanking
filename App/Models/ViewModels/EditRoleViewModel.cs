using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using App.ViewModels;
using App.Models;
using App.Models.ViewModels;

namespace App.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should contain characters and white spaces only")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
      
        public List<string> Claims { get; set; }


        public IList<string> Roles { get; set; }

    }
}
