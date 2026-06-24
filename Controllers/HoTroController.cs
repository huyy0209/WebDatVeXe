using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Controllers;

[Authorize]
public class HoTroController : Controller
{
    private readonly ApplicationDbContext _db;
    public HoTroController(ApplicationDbContext db) => _db = db;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // Danh sach yeu cau ho tro cua toi + form gui moi.
    public async Task<IActionResult> Index()
    {
        ViewBag.DanhSach = await _db.SupportTickets
            .Where(t => t.KhachHangId == UserId)
            .OrderByDescending(t => t.NgayTao)
            .ToListAsync();
        return View(new SupportTicket());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Gui(SupportTicket model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DanhSach = await _db.SupportTickets
                .Where(t => t.KhachHangId == UserId).OrderByDescending(t => t.NgayTao).ToListAsync();
            return View("Index", model);
        }
        model.KhachHangId = UserId;
        model.TrangThai = TrangThaiTicket.Open;
        model.NgayTao = DateTime.Now;

        // Lien ket voi ve neu nhap dung ma ve cua chinh minh
        if (!string.IsNullOrWhiteSpace(model.MaVe))
        {
            var ve = await _db.Ves.FirstOrDefaultAsync(v => v.MaVe == model.MaVe && v.KhachHangId == UserId);
            if (ve != null) model.VeId = ve.Id;
        }

        _db.SupportTickets.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Đã gửi yêu cầu hỗ trợ. Chúng tôi sẽ phản hồi sớm.";
        return RedirectToAction(nameof(Index));
    }
}
