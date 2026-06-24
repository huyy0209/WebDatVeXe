using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Yeu cau ho tro lien quan toi mot ve cu the.
public class SupportTicket
{
    public int Id { get; set; }

    public string? KhachHangId { get; set; }
    public ApplicationUser? KhachHang { get; set; }

    [Required, Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [EmailAddress, Display(Name = "Email")]
    public string? Email { get; set; }

    [Required, Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required, Display(Name = "Tiêu đề")]
    public string TieuDe { get; set; } = string.Empty;

    [Required, Display(Name = "Nội dung")]
    public string NoiDung { get; set; } = string.Empty;

    public string? MaVe { get; set; }
    public int? VeId { get; set; }
    public Ve? Ve { get; set; }

    public TrangThaiTicket TrangThai { get; set; } = TrangThaiTicket.Open;
    public string? PhanHoi { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
