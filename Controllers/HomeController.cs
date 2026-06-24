using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;
using WebDatVeXe.ViewModels;

namespace WebDatVeXe.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db) => _db = db;

    // Trang chu: form tim chuyen + tuyen noi bat.
    public async Task<IActionResult> Index()
    {
        ViewBag.TuyenNoiBat = await _db.TuyenXes
            .Where(t => t.TrangThai == TrangThaiHoatDong.Active)
            .Take(6).ToListAsync();
        // Danh sach diem (dung cho goi y)
        ViewBag.DanhSachDiem = await _db.TuyenXes
            .Select(t => t.DiemDi).Union(_db.TuyenXes.Select(t => t.DiemDen))
            .Distinct().ToListAsync();
        return View(new TimChuyenVM());
    }

    // Ket qua tim chuyen.
    public async Task<IActionResult> TimChuyen(TimChuyenVM vm)
    {
        var q = _db.ChuyenXes
            .Include(c => c.TuyenXe)
            .Include(c => c.Xe)
            .Where(c => c.TrangThai == TrangThaiChuyenXe.Active)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(vm.DiemDi))
            q = q.Where(c => c.TuyenXe!.DiemDi.Contains(vm.DiemDi));
        if (!string.IsNullOrWhiteSpace(vm.DiemDen))
            q = q.Where(c => c.TuyenXe!.DiemDen.Contains(vm.DiemDen));
        if (vm.NgayDi.HasValue)
        {
            var d = vm.NgayDi.Value.Date;
            q = q.Where(c => c.ThoiGianKhoiHanh.Date == d);
        }
        else
        {
            q = q.Where(c => c.ThoiGianKhoiHanh >= DateTime.Now);
        }

        var ketQua = await q.OrderBy(c => c.ThoiGianKhoiHanh).ToListAsync();
        ViewBag.TimKiem = vm;
        return View(ketQua);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
