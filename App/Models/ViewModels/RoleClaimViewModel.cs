namespace App.Models.ViewModels
{
    public class RoleClaimViewModel
    {

        public RoleClaimViewModel()
        {
            Claims = new List<RoleClaim>();
        }

        public string RoleId { get; set; }
        public List<RoleClaim> Claims { get; set; }
    }
}
