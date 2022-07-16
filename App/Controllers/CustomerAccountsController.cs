using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;

namespace App.Controllers
{
    public class CustomerAccountsController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerAccountsController(AppDbContext context)
        {
            _context = context;
        }


        private string GenerateCustomerAccountNumber(AccountTypes accountType, int customerId, int accountId)
        {
            int categoryId = (int)accountType;
            string accountNumber = categoryId.ToString() + customerId.ToString("D4") + accountId.ToString("D6");
            return accountNumber;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.CustomerAccount.Include(c => c.Customer);
            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerAccount == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccount
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,AccountNumber,AccountName,AccountBalance,AccountType,DateOpened,IsActivated")] CustomerAccount customerAccount)
        {
            if (!ModelState.IsValid)
            {
                customerAccount.AccountNumber = GenerateCustomerAccountNumber(customerAccount.AccountType, customerAccount.CustomerId, customerAccount.Id);
                _context.Add(customerAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", customerAccount.CustomerId);
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerAccount == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccount.FindAsync(id);
            if (customerAccount == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", customerAccount.CustomerId);
            return View(customerAccount);
        }

        // POST: CustomerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,AccountNumber,AccountName,AccountBalance,AccountType,DateOpened,IsActivated")] CustomerAccount customerAccount)
        {
            if (id != customerAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAccountExists(customerAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", customerAccount.CustomerId);
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerAccount == null)
            {
                return NotFound();
            }

            var customerAccount = await _context.CustomerAccount
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerAccount == null)
            {
                return NotFound();
            }

            return View(customerAccount);
        }

        // POST: CustomerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerAccount == null)
            {
                return Problem("Entity set 'AppDbContext.CustomerAccount'  is null.");
            }
            var customerAccount = await _context.CustomerAccount.FindAsync(id);
            if (customerAccount != null)
            {
                _context.CustomerAccount.Remove(customerAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerAccountExists(int id)
        {
          return (_context.CustomerAccount?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
