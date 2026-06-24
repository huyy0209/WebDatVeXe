using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class HoTroController : Controller
{
    private readonly ApplicationDbContext _db;
    public HoTroController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.SupportTickets.OrderByDescending(t => t.NgayTao).ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PhanHoi(int id, string phanHoi)
    {
        var t = await _db.SupportTickets.FindAsync(id);
        if (t == null) return NotFound();
        t.PhanHoi = phanHoi;
        t.TrangThai = TrangThaiTicket.Resolved;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Đã phản hồi yêu cầu.";
        return RedirectToAction(nameof(Index));
    }
}
