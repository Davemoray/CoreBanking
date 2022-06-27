using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.LogicModel;

namespace App.Controllers.LogicController
{
    public class GlCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public GlCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GlCategory
        public async Task<IActionResult> Index()
        {
              return _context.GlCategory != null ? 
                          View(await _context.GlCategory.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.GlCategory'  is null.");
        }

        // GET: GlCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GlCategory == null)
            {
                return NotFound();
            }

            var glCategory = await _context.GlCategory
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (glCategory == null)
            {
                return NotFound();
            }

            return View(glCategory);
        }

        // GET: GlCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GlCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,Description,MainCategory")] GlCategory glCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(glCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(glCategory);
        }

        // GET: GlCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GlCategory == null)
            {
                return NotFound();
            }

            var glCategory = await _context.GlCategory.FindAsync(id);
            if (glCategory == null)
            {
                return NotFound();
            }
            return View(glCategory);
        }

        // POST: GlCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,Description,MainCategory")] GlCategory glCategory)
        {
            if (id != glCategory.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(glCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GlCategoryExists(glCategory.CategoryID))
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
            return View(glCategory);
        }

        // GET: GlCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GlCategory == null)
            {
                return NotFound();
            }

            var glCategory = await _context.GlCategory
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (glCategory == null)
            {
                return NotFound();
            }

            return View(glCategory);
        }

        // POST: GlCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GlCategory == null)
            {
                return Problem("Entity set 'AppDbContext.GlCategory'  is null.");
            }
            var glCategory = await _context.GlCategory.FindAsync(id);
            if (glCategory != null)
            {
                _context.GlCategory.Remove(glCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GlCategoryExists(int id)
        {
          return (_context.GlCategory?.Any(e => e.CategoryID == id)).GetValueOrDefault();
        }
    }
}
