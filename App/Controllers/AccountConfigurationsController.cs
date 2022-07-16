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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountConfigurationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AccountConfigurationLogic accountConfigurationLogic;

        public AccountConfigurationsController(AccountConfigurationLogic accountConfigurationLogic,
                                                 AppDbContext context)
        {
            accountConfigurationLogic = accountConfigurationLogic;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

           // var accountConfiguration = await accountConfigurationLogic.RetrieveAccountConfiguration();

            var accountConfiguration =  await _context.AccountConfiguration.FirstOrDefaultAsync();
            //if (TempData["Message"] != null)
            //{
            //    ViewBag.Message = TempData["Message"];  
            //}
            return View(accountConfiguration);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            //if (id == null || _context.AccountConfiguration == null)
            //{
            //    return NotFound();
            //}

            var accountConfiguration = await _context.AccountConfiguration.FirstOrDefaultAsync();
            //var accountConfiguration = await accountConfigurationLogic.RetrieveAccountConfiguration();
            return View(accountConfiguration);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SavingsInterestRate,LoanInterestRate,SavingsMinBalance,CurrentMinBalance,SavingsMaxDailyWithdrawal,CurrentMaxDailyWithdrawal,FinancialDate")] AccountConfiguration accountConfiguration)
        {
            //if (id != accountConfiguration.Id)
            //{
            //    return NotFound();
            //}

            if (!ModelState.IsValid)
            {
                try
                {
                        accountConfiguration.FinancialDate = DateTime.Now;
                        _context.Add(accountConfiguration);
                        await _context.SaveChangesAsync();
                        return View(nameof(Index));
                   
                    //await accountConfigurationLogic.UpdateAccountConfiguration(accountConfiguration);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _context.AccountConfiguration.FirstOrDefaultAsync() != null)
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
            return View(accountConfiguration);
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            var accountConfiguration = await _context.AccountConfiguration.FirstOrDefaultAsync();
            accountConfiguration.SavingsInterestRate = 0;
            accountConfiguration.LoanInterestRate = 0;
            accountConfiguration.SavingsMinBalance = 0;
            accountConfiguration.CurrentMinBalance = 0;
            accountConfiguration.SavingsMaxDailyWithdrawal = 0;
            accountConfiguration.CurrentMaxDailyWithdrawal = 0;

            _context.AccountConfiguration.Update(accountConfiguration);
            await _context.SaveChangesAsync();
            //await accountConfigurationLogic.ClearAccountConfiguration();
            return RedirectToAction(nameof(Index));
        }
       
        [HttpGet]
        public async Task<IActionResult> RunEODAsync()
        {
            ViewBag.ErrorMessage = "Unable to run EOD. Account configuration incomplete.";
            return View("NotFound");

            if (_context.AccountConfiguration.Any(
                a => a.CurrentMaxDailyWithdrawal == null || a.CurrentMinBalance == null ||
                a.FinancialDate == null || a.LoanInterestRate == null || a.SavingsInterestRate == null ||
                a.SavingsMaxDailyWithdrawal == null || a.SavingsMinBalance == null))

            {
                var accountConfiguration = await _context.AccountConfiguration.FirstOrDefaultAsync();
                accountConfiguration.FinancialDate = accountConfiguration.FinancialDate.AddDays(1);
                _context.Update(accountConfiguration);
                await _context.SaveChangesAsync();
                //accountConfigurationLogic.RunEOD();
                ViewBag.Message = "EOD successfully completed.";
            }
            ViewBag.Message = TempData["Message"];
            return RedirectToAction(nameof(Index));
        }

        private bool AccountConfigurationExists(int id)
        {
          return (_context.AccountConfiguration?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
