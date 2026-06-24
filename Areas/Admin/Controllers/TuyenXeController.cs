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
    public class TuyenXeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TuyenXeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TuyenXe
        public async Task<IActionResult> Index()
        {
            return View(await _context.TuyenXes.ToListAsync());
        }

        // GET: TuyenXe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyenXe = await _context.TuyenXes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tuyenXe == null)
            {
                return NotFound();
            }

            return View(tuyenXe);
        }

        // GET: TuyenXe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TuyenXe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DiemDi,DiemDen,KhoangCach,ThoiGianDi,GiaVe,TrangThai,NgayTao")] TuyenXe tuyenXe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tuyenXe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tuyenXe);
        }

        // GET: TuyenXe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyenXe = await _context.TuyenXes.FindAsync(id);
            if (tuyenXe == null)
            {
                return NotFound();
            }
            return View(tuyenXe);
        }

        // POST: TuyenXe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DiemDi,DiemDen,KhoangCach,ThoiGianDi,GiaVe,TrangThai,NgayTao")] TuyenXe tuyenXe)
        {
            if (id != tuyenXe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tuyenXe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuyenXeExists(tuyenXe.Id))
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
            return View(tuyenXe);
        }

        // GET: TuyenXe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyenXe = await _context.TuyenXes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tuyenXe == null)
            {
                return NotFound();
            }

            return View(tuyenXe);
        }

        // POST: TuyenXe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tuyenXe = await _context.TuyenXes.FindAsync(id);
            if (tuyenXe != null)
            {
                _context.TuyenXes.Remove(tuyenXe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TuyenXeExists(int id)
        {
            return _context.TuyenXes.Any(e => e.Id == id);
        }
    }
}
