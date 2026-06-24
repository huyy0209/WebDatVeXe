using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class LienHeController : Controller
{
    private readonly ApplicationDbContext _db;
    public LienHeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.LienHes.OrderByDescending(l => l.NgayGui).ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DanhDauXuLy(int id)
    {
        var lh = await _db.LienHes.FindAsync(id);
        if (lh == null) return NotFound();
        lh.TrangThai = TrangThaiLienHe.Resolved;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Đã đánh dấu xử lý.";
        return RedirectToAction(nameof(Index));
    }
}
