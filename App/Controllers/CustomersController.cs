using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Logic;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CustomerLogic customerLogic;
        public CustomersController(AppDbContext context,
                                     CustomerLogic customerLogic)
        {
            _context = context;
            customerLogic = customerLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View (await _context.Customer.ToListAsync());
            //return View(await customerLogic.ListCustomersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }
            var customer = await _context.Customer.FirstOrDefaultAsync(m => m.CustomerId == id);
            //var customer = await customerLogic.RetrieveCustomerAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,FirstName,LastName,Email,Phone,IsActivated")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                // await customerLogic.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }
            var customer = await _context.Customer.FirstOrDefaultAsync(m => m.CustomerId == id);
            //var customer = await customerLogic.RetrieveCustomerAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,FirstName,LastName,Email,Phone,IsActivated")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    //await customerLogic.EditCustomerAsync(customer);
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CustomerExists(customer.Id))
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
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await customerLogic.RetrieveCustomerAsync(id.Value);
                
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await customerLogic.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _context.Customer.AnyAsync(e => e.CustomerId == id);
            // return await customerLogic.CustomerExists(id);
        }
    }
}
