using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [DisplayName("Status")]
        public bool IsActivated { get; set; }
        public List<CustomerAccount> Accounts { get; set; }
    }
}
