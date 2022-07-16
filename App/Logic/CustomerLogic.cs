

using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Logic
{
    public class CustomerLogic
    {
        private readonly AppDbContext _context;

        public CustomerLogic(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CustomerExists(int id)
        {
            return await _context.Customer.AnyAsync(e => e.CustomerId == id);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await RetrieveCustomerAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task EditCustomerAsync(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> ListCustomersAsync()
        {
            var customers = await _context.Customer.ToListAsync();
            return customers;
        }

        public async Task<Customer> RetrieveCustomerAsync(int id)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(m => m.CustomerId == id);
            return customer;
        }
    }
}
