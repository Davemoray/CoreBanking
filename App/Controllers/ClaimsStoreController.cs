using App.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Controllers
{
    public class ClaimsStore: Controller
    {

        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role", "Edit Role"),
            new Claim("Delete Role", "Delete Role"),
            new Claim("Create User", "Create User"),
            new Claim("Activate/Deactivate User", "Activate/Deactive User"),
        };
        
       
    }
}
