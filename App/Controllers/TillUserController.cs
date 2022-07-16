using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TillUserController : Controller
    {
        private readonly AppDbContext _context;

        public TillUserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TillUser.Include(t => t.GLAccount);
            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TillUser == null)
            {
                return NotFound();
            }

            var tillUser = await _context.TillUser
                .Include(t => t.GLAccount)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tillUser == null)
            {
                return NotFound();
            }

            return View(tillUser);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["GLAccountID"] = new SelectList(_context.GLAccount, "AccountID", "AccountName");
            ViewData["User"] = new SelectList(_context.Users, "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,GLAccountID")] TillUser tillUser)
        {
            if (!ModelState.IsValid)
            {
              
                _context.Add(tillUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GLAccountID"] = new SelectList(_context.GLAccount, "AccountID", "AccountName", tillUser.GLAccountID);
            ViewData["User"] = new SelectList(_context.Users, "UserName", tillUser.UserName);
            return View(tillUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TillUser == null)
            {
                return NotFound();
            }

            var tillUser = await _context.TillUser.FindAsync(id);
            if (tillUser == null)
            {
                return NotFound();
            }
            ViewData["User"] = new SelectList(_context.Users, "UserName",tillUser.UserName);
            ViewData["GLAccountID"] = new SelectList(_context.GLAccount, "AccountID", "AccountName", tillUser.GLAccountID);
            return View(tillUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,GLAccountID")] TillUser tillUser)
        {
            if (id != tillUser.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(tillUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TillUserExists(tillUser.ID))
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
            ViewData["User"] = new SelectList(_context.Users, "UserName",  tillUser.UserName);
            ViewData["GLAccountID"] = new SelectList(_context.GLAccount, "AccountID", "AccountName", tillUser.GLAccountID);
            return View(tillUser);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TillUser == null)
            {
                return NotFound();
            }

            var tillUser = await _context.TillUser
                .Include(t => t.GLAccount)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tillUser == null)
            {
                return NotFound();
            }

            return View(tillUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TillUser == null)
            {
                return Problem("Entity set 'AppDbContext.TillUser'  is null.");
            }
            var tillUser = await _context.TillUser.FindAsync(id);
            if (tillUser != null)
            {
                _context.TillUser.Remove(tillUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TillUserExists(int id)
        {
          return (_context.TillUser?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
