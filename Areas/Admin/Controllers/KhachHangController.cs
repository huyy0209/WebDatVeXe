using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class KhachHangController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public KhachHangController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _db.Users.OrderByDescending(u => u.NgayTao).ToListAsync();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DoiTrangThai(string id, string? lyDo)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (user.TrangThai == TrangThaiTaiKhoan.Active)
        {
            user.TrangThai = TrangThaiTaiKhoan.Inactive;
            user.LyDoKhoa = lyDo;
            user.LockoutEnd = DateTimeOffset.MaxValue;
        }
        else
        {
            user.TrangThai = TrangThaiTaiKhoan.Active;
            user.LyDoKhoa = null;
            user.LockoutEnd = null;
        }
        await _userManager.UpdateAsync(user);
        TempData["Success"] = "Đã cập nhật trạng thái tài khoản.";
        return RedirectToAction(nameof(Index));
    }
}
