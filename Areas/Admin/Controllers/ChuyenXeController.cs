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
    public class ChuyenXeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChuyenXeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChuyenXe
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChuyenXes.Include(c => c.TuyenXe).Include(c => c.Xe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChuyenXe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuyenXe = await _context.ChuyenXes
                .Include(c => c.TuyenXe)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyenXe == null)
            {
                return NotFound();
            }

            return View(chuyenXe);
        }

        // GET: ChuyenXe/Create
        public IActionResult Create()
        {
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen");
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "BienSo");
            return View();
        }

        // POST: ChuyenXe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TuyenXeId,XeId,ThoiGianKhoiHanh,ThoiGianDen,TrangThai,GheDaDatCsv,NgayTao")] ChuyenXe chuyenXe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chuyenXe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", chuyenXe.TuyenXeId);
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "BienSo", chuyenXe.XeId);
            return View(chuyenXe);
        }

        // GET: ChuyenXe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuyenXe = await _context.ChuyenXes.FindAsync(id);
            if (chuyenXe == null)
            {
                return NotFound();
            }
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", chuyenXe.TuyenXeId);
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "BienSo", chuyenXe.XeId);
            return View(chuyenXe);
        }

        // POST: ChuyenXe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TuyenXeId,XeId,ThoiGianKhoiHanh,ThoiGianDen,TrangThai,GheDaDatCsv,NgayTao")] ChuyenXe chuyenXe)
        {
            if (id != chuyenXe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuyenXe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuyenXeExists(chuyenXe.Id))
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
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", chuyenXe.TuyenXeId);
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "BienSo", chuyenXe.XeId);
            return View(chuyenXe);
        }

        // GET: ChuyenXe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuyenXe = await _context.ChuyenXes
                .Include(c => c.TuyenXe)
                .Include(c => c.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyenXe == null)
            {
                return NotFound();
            }

            return View(chuyenXe);
        }

        // POST: ChuyenXe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuyenXe = await _context.ChuyenXes.FindAsync(id);
            if (chuyenXe != null)
            {
                _context.ChuyenXes.Remove(chuyenXe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChuyenXeExists(int id)
        {
            return _context.ChuyenXes.Any(e => e.Id == id);
        }
    }
}
