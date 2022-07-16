using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Logic
{
    public class AccountConfigurationLogic
    {
        public readonly AppDbContext _context;
        public AccountConfigurationLogic(AppDbContext context)
        {
            _context = context;
        }


        public async Task<AccountConfiguration> RetrieveAccountConfiguration()
        {
            return await _context.AccountConfiguration.FirstOrDefaultAsync();
        }

        public async Task ClearAccountConfiguration()
        {
            var accountConfiguration = await RetrieveAccountConfiguration();
            accountConfiguration.SavingsInterestRate = 0; 
            accountConfiguration.LoanInterestRate = 0;  
            accountConfiguration.SavingsMinBalance = 0;
            accountConfiguration.CurrentMinBalance = 0;
            accountConfiguration.SavingsMaxDailyWithdrawal = 0;
            accountConfiguration.CurrentMaxDailyWithdrawal = 0;

            _context.AccountConfiguration.Update(accountConfiguration);
            await _context.SaveChangesAsync();
        }

        public bool IsAccountConfigurationComplete()
        {
            var incomplete = _context.AccountConfiguration.Any(
                a => a.CurrentMaxDailyWithdrawal == null || a.CurrentMinBalance == null ||
                a.FinancialDate == null || a.LoanInterestRate == null || a.SavingsInterestRate == null ||
                a.SavingsMaxDailyWithdrawal == null || a.SavingsMinBalance == null
                );
            return incomplete;  
        }

        public async Task UpdateAccountConfiguration(AccountConfiguration accountConfiguration)
        { 
        if (accountConfiguration.Id == 0)
            {
                accountConfiguration.FinancialDate = DateTime.Now;
                _context.Add(accountConfiguration);
                await _context.SaveChangesAsync();
                return;
            }
        }

        public async Task RunEOD()
        {
            var accountConfiguration = await RetrieveAccountConfiguration();
            accountConfiguration.FinancialDate = accountConfiguration.FinancialDate.AddDays(1);
            _context.Update(accountConfiguration);
            await _context.SaveChangesAsync();
        }

    }
}
