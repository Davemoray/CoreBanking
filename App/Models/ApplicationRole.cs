using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsEnabled { get; set; } = true;
    }
}
