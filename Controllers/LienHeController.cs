using Microsoft.AspNetCore.Mvc;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Controllers;

public class LienHeController : Controller
{
    private readonly ApplicationDbContext _db;
    public LienHeController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Index() => View(new LienHe());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LienHe model)
    {
        if (!ModelState.IsValid) return View(model);
        model.TrangThai = TrangThaiLienHe.Pending;
        model.NgayGui = DateTime.Now;
        _db.LienHes.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cảm ơn bạn! Chúng tôi sẽ phản hồi sớm nhất.";
        return RedirectToAction(nameof(Index));
    }
}
