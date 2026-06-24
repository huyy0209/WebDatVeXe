using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;
using WebDatVeXe.Services;
using WebDatVeXe.ViewModels;

namespace WebDatVeXe.Controllers;

[Authorize]
public class VeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IDatVeService _datVe;

    public VeController(ApplicationDbContext db, IDatVeService datVe)
    {
        _db = db;
        _datVe = datVe;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // Trang chon ghe cho mot chuyen.
    [AllowAnonymous]
    public async Task<IActionResult> ChonGhe(int id)
    {
        var chuyen = await _db.ChuyenXes
            .Include(c => c.TuyenXe)
            .Include(c => c.Xe).ThenInclude(x => x!.SoDoGhe)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (chuyen == null) return NotFound();

        var vm = new ChonGheVM
        {
            ChuyenXe = chuyen,
            TatCaGhe = chuyen.Xe?.SoDoGhe?.DanhSachGhe ?? new List<string>(),
            GheDaDat = chuyen.GheDaDat,
            GiaVe = chuyen.TuyenXe?.GiaVe ?? 0
        };
        return View(vm);
    }

    // Giu cho (POST tu trang chon ghe).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DatVe(DatVeVM vm)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Vui lòng nhập đầy đủ thông tin và chọn ghế.";
            return RedirectToAction(nameof(ChonGhe), new { id = vm.ChuyenXeId });
        }

        var ghe = vm.GheChon.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        var kq = await _datVe.GiuChoAsync(vm.ChuyenXeId, ghe, UserId,
            vm.HoTen, vm.SoDienThoai, vm.Email, vm.MaVoucher);

        if (!kq.ThanhCong)
        {
            TempData["Error"] = kq.ThongBao;
            return RedirectToAction(nameof(ChonGhe), new { id = vm.ChuyenXeId });
        }
        return RedirectToAction(nameof(ThanhToan), new { id = kq.Ve!.Id });
    }

    // Trang thanh toan (gia lap).
    public async Task<IActionResult> ThanhToan(int id)
    {
        var ve = await _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .FirstOrDefaultAsync(v => v.Id == id && v.KhachHangId == UserId);
        if (ve == null) return NotFound();
        return View(ve);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhanThanhToan(int id, string phuongThuc)
    {
        var ve = await _db.Ves.FirstOrDefaultAsync(v => v.Id == id && v.KhachHangId == UserId);
        if (ve == null) return NotFound();

        var kq = await _datVe.XacNhanThanhToanAsync(id, phuongThuc);
        if (!kq.ThanhCong)
        {
            TempData["Error"] = kq.ThongBao;
            return RedirectToAction(nameof(ThanhToan), new { id });
        }
        TempData["Success"] = "Thanh toán thành công! Vé của bạn đã được xác nhận.";
        return RedirectToAction(nameof(ChiTiet), new { id });
    }

    // Ve cua toi.
    public async Task<IActionResult> VeCuaToi()
    {
        var ves = await _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .Where(v => v.KhachHangId == UserId)
            .OrderByDescending(v => v.NgayDat)
            .ToListAsync();
        return View(ves);
    }

    public async Task<IActionResult> ChiTiet(int id)
    {
        var ve = await _db.Ves
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.TuyenXe)
            .Include(v => v.ChuyenXe).ThenInclude(c => c!.Xe)
            .Include(v => v.HoaDon)
            .FirstOrDefaultAsync(v => v.Id == id && v.KhachHangId == UserId);
        if (ve == null) return NotFound();
        return View(ve);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Huy(int id)
    {
        var kq = await _datVe.HuyVeAsync(id, UserId);
        TempData[kq.ThanhCong ? "Success" : "Error"] = kq.ThongBao;
        return RedirectToAction(nameof(VeCuaToi));
    }
}
