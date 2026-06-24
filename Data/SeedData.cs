using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Models;

namespace WebDatVeXe.Data;

// Khoi tao du lieu ban dau: roles, tai khoan admin va du lieu mau.
public static class SeedData
{
    public static async Task KhoiTaoAsync(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<ApplicationDbContext>();
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        // ---- Roles ----
        foreach (var role in new[] { VaiTro.Admin, VaiTro.Staff, VaiTro.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // ---- Tai khoan Admin mac dinh ----
        const string adminEmail = "admin@datvexe.vn";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                HoTen = "Quản trị viên",
                PhoneNumber = "0900000000"
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, VaiTro.Admin);
        }

        // ---- Tai khoan khach mau ----
        const string userEmail = "user@datvexe.vn";
        if (await userManager.FindByEmailAsync(userEmail) == null)
        {
            var user = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true,
                HoTen = "Nguyễn Văn A",
                PhoneNumber = "0911111111"
            };
            await userManager.CreateAsync(user, "User@123");
            await userManager.AddToRoleAsync(user, VaiTro.User);
        }

        // ---- Du lieu mau (chi seed khi chua co tuyen nao) ----
        if (await db.TuyenXes.AnyAsync()) return;

        var soDo = new SoDoGhe
        {
            TenSoDo = "Xe giường nằm 34 chỗ",
            TongSoGhe = 34,
            SoTang = 2,
            DanhSachGhe = TaoDanhSachGhe()
        };
        db.SoDoGhes.Add(soDo);
        await db.SaveChangesAsync();

        var tuyen1 = new TuyenXe
        {
            DiemDi = "TP. Hồ Chí Minh",
            DiemDen = "Đà Lạt",
            KhoangCach = "310 km",
            ThoiGianDi = "08:00",
            GiaVe = 290000,
            DiemDungs = new List<DiemDungXe>
            {
                new() { Loai = LoaiDiemDung.Don, TenDiem = "Bến xe Miền Đông", TinhThanh = "TP. Hồ Chí Minh", ThuTu = 1 },
                new() { Loai = LoaiDiemDung.Tra, TenDiem = "Bến xe Đà Lạt", TinhThanh = "Đà Lạt", ThuTu = 1 }
            }
        };
        var tuyen2 = new TuyenXe
        {
            DiemDi = "TP. Hồ Chí Minh",
            DiemDen = "Nha Trang",
            KhoangCach = "430 km",
            ThoiGianDi = "21:00",
            GiaVe = 320000,
            DiemDungs = new List<DiemDungXe>
            {
                new() { Loai = LoaiDiemDung.Don, TenDiem = "Bến xe Miền Đông", TinhThanh = "TP. Hồ Chí Minh", ThuTu = 1 },
                new() { Loai = LoaiDiemDung.Tra, TenDiem = "Bến xe phía Nam Nha Trang", TinhThanh = "Nha Trang", ThuTu = 1 }
            }
        };
        db.TuyenXes.AddRange(tuyen1, tuyen2);
        await db.SaveChangesAsync();

        var xe1 = new Xe { BienSo = "51B-12345", LoaiXe = "Giường nằm", TongSoGhe = 34, SoTang = 2, SoDoGheId = soDo.Id, TuyenXeId = tuyen1.Id };
        var xe2 = new Xe { BienSo = "51B-67890", LoaiXe = "Giường nằm", TongSoGhe = 34, SoTang = 2, SoDoGheId = soDo.Id, TuyenXeId = tuyen2.Id };
        db.Xes.AddRange(xe1, xe2);
        await db.SaveChangesAsync();

        db.ChuyenXes.AddRange(
            new ChuyenXe { TuyenXeId = tuyen1.Id, XeId = xe1.Id, ThoiGianKhoiHanh = DateTime.Today.AddDays(1).AddHours(8), ThoiGianDen = DateTime.Today.AddDays(1).AddHours(14) },
            new ChuyenXe { TuyenXeId = tuyen2.Id, XeId = xe2.Id, ThoiGianKhoiHanh = DateTime.Today.AddDays(1).AddHours(21), ThoiGianDen = DateTime.Today.AddDays(2).AddHours(5) }
        );

        db.Vouchers.Add(new Voucher
        {
            TenVoucher = "Giảm 50K cho khách mới",
            MaVoucher = "WELCOME50",
            MoTa = "Giảm 50.000đ cho đơn từ 200.000đ",
            LoaiGiamGia = LoaiGiamGia.Fixed,
            GiaTriGiam = 50000,
            GiaTriToiThieu = 200000,
            SoLuong = 1000,
            ChoKhachHangMoi = true
        });

        await db.SaveChangesAsync();
    }

    // Tao so do ghe 2 tang: tang A va tang B, moi tang 17 ghe.
    private static List<string> TaoDanhSachGhe()
    {
        var ghe = new List<string>();
        foreach (var tang in new[] { "A", "B" })
            for (int i = 1; i <= 17; i++)
                ghe.Add($"{tang}{i:D2}");
        return ghe;
    }
}
