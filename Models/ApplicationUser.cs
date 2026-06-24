using Microsoft.AspNetCore.Identity;

namespace WebDatVeXe.Models;

// Người dùng hệ thống = Khách hàng. Admin/Staff cũng là ApplicationUser nhưng gắn Role tương ứng.
// Kế thừa IdentityUser để tận dụng Email, PhoneNumber, PasswordHash... của ASP.NET Core Identity.
public class ApplicationUser : IdentityUser
{
    public string HoTen { get; set; } = string.Empty;
    public string? DiaChi { get; set; }
    public string GioiTinh { get; set; } = "nam";
    public DateTime? NgaySinh { get; set; }
    public string? NgheNghiep { get; set; }
    public TrangThaiTaiKhoan TrangThai { get; set; } = TrangThaiTaiKhoan.Active;
    public string? LyDoKhoa { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Quan hệ điều hướng
    public ICollection<Ve> DanhSachVe { get; set; } = new List<Ve>();
    public ICollection<DanhGia> DanhSachDanhGia { get; set; } = new List<DanhGia>();
}
