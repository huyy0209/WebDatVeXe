using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;

namespace WebDatVeXe.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class VeController : Controller
{
    private readonly ApplicationDbContext _db;
    public VeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        var q = _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .Include(v => v.KhachHang)
            .OrderByDescending(v => v.NgayDat)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            q = q.Where(v => v.MaVe.Contains(tuKhoa) || v.SoDienThoai!.Contains(tuKhoa) || v.HoTen!.Contains(tuKhoa));

        ViewBag.TuKhoa = tuKhoa;
        return View(await q.Take(200).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var ve = await _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.Xe)
            .Include(v => v.KhachHang)
            .Include(v => v.HoaDon)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (ve == null) return NotFound();
        return View(ve);
    }
}
