using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using App.ViewModels;

namespace App.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
            Claims = new List<UserClaim>();
            Roles = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
      
        public List<UserClaim> Claims { get; set; }
        public string RoleId { get; set; }
        public List<string> Roles { get; set; }
    }
}
