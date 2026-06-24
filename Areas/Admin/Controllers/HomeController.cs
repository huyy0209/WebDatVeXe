using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.SoTuyen = await _db.TuyenXes.CountAsync();
        ViewBag.SoXe = await _db.Xes.CountAsync();
        ViewBag.SoChuyen = await _db.ChuyenXes.CountAsync();
        ViewBag.SoVe = await _db.Ves.CountAsync(v => v.TrangThai == TrangThaiVe.Paid || v.TrangThai == TrangThaiVe.Completed);
        ViewBag.SoKhach = await _db.Users.CountAsync();
        ViewBag.DoanhThu = await _db.HoaDons
            .Where(h => h.TrangThai == TrangThaiHoaDon.Completed)
            .SumAsync(h => (decimal?)h.TongTien) ?? 0;

        var veGanDay = await _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .OrderByDescending(v => v.NgayDat)
            .Take(8)
            .ToListAsync();
        return View(veGanDay);
    }
}
