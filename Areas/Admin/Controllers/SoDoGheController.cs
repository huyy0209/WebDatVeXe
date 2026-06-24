using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class SoDoGheController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SoDoGheController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SoDoGhe
        public async Task<IActionResult> Index()
        {
            return View(await _context.SoDoGhes.ToListAsync());
        }

        // GET: SoDoGhe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soDoGhe = await _context.SoDoGhes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soDoGhe == null)
            {
                return NotFound();
            }

            return View(soDoGhe);
        }

        // GET: SoDoGhe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SoDoGhe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSoDo,TongSoGhe,SoTang,DanhSachGheCsv")] SoDoGhe soDoGhe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soDoGhe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(soDoGhe);
        }

        // GET: SoDoGhe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soDoGhe = await _context.SoDoGhes.FindAsync(id);
            if (soDoGhe == null)
            {
                return NotFound();
            }
            return View(soDoGhe);
        }

        // POST: SoDoGhe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSoDo,TongSoGhe,SoTang,DanhSachGheCsv")] SoDoGhe soDoGhe)
        {
            if (id != soDoGhe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soDoGhe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoDoGheExists(soDoGhe.Id))
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
            return View(soDoGhe);
        }

        // GET: SoDoGhe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soDoGhe = await _context.SoDoGhes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soDoGhe == null)
            {
                return NotFound();
            }

            return View(soDoGhe);
        }

        // POST: SoDoGhe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soDoGhe = await _context.SoDoGhes.FindAsync(id);
            if (soDoGhe != null)
            {
                _context.SoDoGhes.Remove(soDoGhe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoDoGheExists(int id)
        {
            return _context.SoDoGhes.Any(e => e.Id == id);
        }
    }
}
