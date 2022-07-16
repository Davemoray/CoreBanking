using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class CustomerAccount
    {

        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }

        [EnumDataType(typeof(AccountTypes)), DisplayName("Account Type")]
        public AccountTypes AccountType { get; set; }

        [DisplayName("Date Opened")]
        public DateTime DateOpened { get; set; }

        [DisplayName("Account Status")]
        public bool IsActivated { get; set; }

    }
}