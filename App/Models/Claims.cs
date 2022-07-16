using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Claims
    {
        [Key]
        public int Id { get; set; }
        public string ClaimsName { get; set; }
    }
}
