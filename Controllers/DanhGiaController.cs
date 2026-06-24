using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Controllers;

[Authorize]
public class DanhGiaController : Controller
{
    private readonly ApplicationDbContext _db;
    public DanhGiaController(ApplicationDbContext db) => _db = db;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // Form danh gia cho mot chuyen (chi khi khach da co ve da thanh toan).
    [HttpGet]
    public async Task<IActionResult> Them(int chuyenXeId)
    {
        var coVe = await _db.Ves.AnyAsync(v => v.ChuyenXeId == chuyenXeId &&
            v.KhachHangId == UserId &&
            (v.TrangThai == TrangThaiVe.Paid || v.TrangThai == TrangThaiVe.Completed));
        if (!coVe)
        {
            TempData["Error"] = "Bạn cần có vé đã thanh toán của chuyến này để đánh giá.";
            return RedirectToAction("VeCuaToi", "Ve");
        }

        var chuyen = await _db.ChuyenXes.Include(c => c.TuyenXe).FirstOrDefaultAsync(c => c.Id == chuyenXeId);
        if (chuyen == null) return NotFound();
        ViewBag.Chuyen = chuyen;
        return View(new DanhGia { ChuyenXeId = chuyenXeId, SoSao = 5 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Them(DanhGia model)
    {
        if (!ModelState.IsValid)
        {
            var chuyen = await _db.ChuyenXes.Include(c => c.TuyenXe).FirstOrDefaultAsync(c => c.Id == model.ChuyenXeId);
            ViewBag.Chuyen = chuyen;
            return View(model);
        }
        model.KhachHangId = UserId;
        model.NgayDanhGia = DateTime.Now;
        _db.DanhGias.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cảm ơn bạn đã đánh giá!";
        return RedirectToAction("VeCuaToi", "Ve");
    }
}
