using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Form lien he tu khach (khong can dang nhap).
public class LienHe
{
    public int Id { get; set; }

    [Required, Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Required, Display(Name = "Tiêu đề")]
    public string TieuDe { get; set; } = string.Empty;

    [Required, Display(Name = "Nội dung")]
    public string NoiDung { get; set; } = string.Empty;

    public TrangThaiLienHe TrangThai { get; set; } = TrangThaiLienHe.Pending;
    public DateTime NgayGui { get; set; } = DateTime.Now;
}
