using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class AccountConfiguration
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("% Interest Rate")]
        public float? SavingsInterestRate { get; set; }

        [DisplayName("% Interest Rate")]
        public float? LoanInterestRate { get; set; }

        [DisplayName("Minimum Balance")]
        public float? SavingsMinBalance { get; set; }

        [DisplayName("Minimum Balance")]
        public float? CurrentMinBalance { get; set; }

        [DisplayName("Maximum Daily Withdrawal")]
        public float? SavingsMaxDailyWithdrawal { get; set; }

        [DisplayName("Maximum Daily Withdrawal")]
        public float? CurrentMaxDailyWithdrawal { get; set; }
        public DateTime FinancialDate { get; set; }
    }
}
