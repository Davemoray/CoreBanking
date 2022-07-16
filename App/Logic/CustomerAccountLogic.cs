//using App.Data;
//using App.Interface;
//using App.Models;
//using Microsoft.EntityFrameworkCore;

//namespace App.Logic
//{
//    public class CustomerAccountLogic : ICustomerAccountLogic
//    {

//        private readonly AppDbContext _context;

//        public CustomerAccountLogic(AppDbContext context)
//        {
//            _context = context;
//        }
//        private string GenerateCustomerAccountNumber(AccountTypes accountType, int customerId, int accountId)
//        {
//            int categoryId = (int)accountType;
//            string accountNumber = categoryId.ToString() + customerId.ToString("D4") + accountId.ToString("D6");
//            return accountNumber;
//        }

//        public async Task<CustomerAccount> AddCustomerAccountAsync(CustomerAccount customerAccountss)
//        {
//            var customer = await _context.Customer.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.CustomerId == customerAccountss.CustomerId);
//            if (customer.Accounts.Any(a => a.AccountType == customerAccountss.AccountType))
//            {
//                throw new Exception($"Unable to create account. Customer already has a {customerAccountss.AccountType} account in the system.");
//            }
//            var customerAccount = new CustomerAccount()
//            {
//                CustomerId = customerAccountss.CustomerId,
//                IsActivated = customerAccountss.IsActivated,
//                AccountType = customerAccountss.AccountType,
//                AccountName = $"{customer.FirstName} {customer.LastName} {customerAccountss.AccountType}",
//                DateOpened = DateTime.Now
//            };
//            _context.Add(customerAccount);
//            await _context.SaveChangesAsync();
//            customerAccount.AccountNumber = GenerateCustomerAccountNumber (customerAccount.AccountType, customerAccount.CustomerId, customerAccount.Id);
//            _context.Update(customerAccount);
//            await _context.SaveChangesAsync();
//            return customerAccount;
//        }

//        public async Task EditCustomerAccountAsync(CustomerAccount customerAccountss)
//        {
//            var customerAccount = await RetrieveCustomerAccountAsync(customerAccountss.Id);
//            customerAccount.AccountName = customerAccountss.AccountName;
//            customerAccount.IsActivated = customerAccountss.IsActivated;
//            _context.Update(customerAccount);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<CustomerAccount> RetrieveCustomerAccountAsync(int id)
//        {
//            var account = await _context.CustomerAccounts
//                .Include(c => c.Customer)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            return account;
//        }

//        public async Task<List<CustomerAccount>> ListCustomerAccountsAsync()
//        {
//            var accounts = _context.CustomerAccounts.Include(c => c.Customer);
//            return await accounts.ToListAsync();
//        }

//        public CustomerAccount GetAddCustomerAccount(int customerId, AccountTypes accountType)
//        {
//            var viewModel = new CustomerAccount
//            {
//                CustomerId = customerId,
//                AccountType = accountType
//            };
//            return viewModel;
//        }

//        public async Task<CustomerAccount> GetEditCustomerAccount(int id)
//        {
//            var account = await RetrieveCustomerAccountAsync(id);
//            var viewModel = new CustomerAccount
//            {
//                Id = account.Id,
//                CustomerId = account.CustomerId,
//                AccountType = account.AccountType,
//                IsActivated = account.IsActivated,
//                AccountName = account.AccountName,
//                AccountNumber = account.AccountNumber
//            };
//            return viewModel;
//        }


//    }
//}
