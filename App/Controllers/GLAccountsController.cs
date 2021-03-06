using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.Logic;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GLAccountsController : Controller
    {
        private readonly AppDbContext _context;


        public GLAccountsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GLAccount.Include(g => g.Branch).Include(g => g.GLCategory);
            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GLAccount == null)
            {
                return NotFound();
            }

            var gLAccount = await _context.GLAccount
                .Include(g => g.Branch)
                .Include(g => g.GLCategory)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (gLAccount == null)
            {
                return NotFound();
            }

            return View(gLAccount);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName");
            ViewData["GlCategoryID"] = new SelectList(_context.GLCategory, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,AccountName,GLAccountCode,AccountBalance,GlCategoryID,MainCategory,BranchID")] GLAccount gLAccount)
        {
            if (!ModelState.IsValid)
            {
                GLCategory glcategory = await _context.GLCategory.FindAsync(gLAccount.GlCategoryID);
                gLAccount.GLAccountCode = Generator.GenerateGLAccountCode(glcategory.MainCategory.ToString());
                _context.Add(gLAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName", gLAccount.BranchID);
            ViewData["GlCategoryID"] = new SelectList(_context.GLCategory, "CategoryId", "CategoryName", gLAccount.GlCategoryID);
            return View(gLAccount);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GLAccount == null)
            {
                return NotFound();
            }

            var gLAccount = await _context.GLAccount.FindAsync(id);
            if (gLAccount == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName", gLAccount.BranchID);
            ViewData["GlCategoryID"] = new SelectList(_context.GLCategory, "CategoryId", "CategoryName", gLAccount.GlCategoryID);
            return View(gLAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountID,AccountName,GLAccountCode,AccountBalance,GlCategoryID,MainCategory,BranchID")] GLAccount gLAccount)
        {
            if (id != gLAccount.AccountID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(gLAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GLAccountExists(gLAccount.AccountID))
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
            ViewData["BranchID"] = new SelectList(_context.Branch, "BranchID", "BranchName", gLAccount.BranchID);
            ViewData["GlCategoryID"] = new SelectList(_context.GLCategory, "CategoryId", "CategoryName", gLAccount.GlCategoryID);
            return View(gLAccount);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GLAccount == null)
            {
                return NotFound();
            }

            var gLAccount = await _context.GLAccount
                .Include(g => g.Branch)
                .Include(g => g.GLCategory)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (gLAccount == null)
            {
                return NotFound();
            }

            return View(gLAccount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GLAccount == null)
            {
                return Problem("Entity set 'AppDbContext.GLAccount'  is null.");
            }
            var gLAccount = await _context.GLAccount.FindAsync(id);
            if (gLAccount != null)
            {
                _context.GLAccount.Remove(gLAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GLAccountExists(int id)
        {
          return (_context.GLAccount?.Any(e => e.AccountID == id)).GetValueOrDefault();
        }
    }
}
