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
    public class XeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public XeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Xe
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Xes.Include(x => x.SoDoGhe).Include(x => x.TuyenXe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Xe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes
                .Include(x => x.SoDoGhe)
                .Include(x => x.TuyenXe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        // GET: Xe/Create
        public IActionResult Create()
        {
            ViewData["SoDoGheId"] = new SelectList(_context.SoDoGhes, "Id", "TenSoDo");
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen");
            return View();
        }

        // POST: Xe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BienSo,LoaiXe,TongSoGhe,SoDoGheId,TuyenXeId,SoTang,TrangThai")] Xe xe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(xe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SoDoGheId"] = new SelectList(_context.SoDoGhes, "Id", "TenSoDo", xe.SoDoGheId);
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", xe.TuyenXeId);
            return View(xe);
        }

        // GET: Xe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes.FindAsync(id);
            if (xe == null)
            {
                return NotFound();
            }
            ViewData["SoDoGheId"] = new SelectList(_context.SoDoGhes, "Id", "TenSoDo", xe.SoDoGheId);
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", xe.TuyenXeId);
            return View(xe);
        }

        // POST: Xe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BienSo,LoaiXe,TongSoGhe,SoDoGheId,TuyenXeId,SoTang,TrangThai")] Xe xe)
        {
            if (id != xe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XeExists(xe.Id))
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
            ViewData["SoDoGheId"] = new SelectList(_context.SoDoGhes, "Id", "TenSoDo", xe.SoDoGheId);
            ViewData["TuyenXeId"] = new SelectList(_context.TuyenXes, "Id", "DiemDen", xe.TuyenXeId);
            return View(xe);
        }

        // GET: Xe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes
                .Include(x => x.SoDoGhe)
                .Include(x => x.TuyenXe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        // POST: Xe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xe = await _context.Xes.FindAsync(id);
            if (xe != null)
            {
                _context.Xes.Remove(xe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XeExists(int id)
        {
            return _context.Xes.Any(e => e.Id == id);
        }
    }
}
