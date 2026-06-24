using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.ViewModels;

// Du lieu form dat ve (gui len khi giu cho).
public class DatVeVM
{
    public int ChuyenXeId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ghế")]
    public string GheChon { get; set; } = string.Empty; // CSV: "A01,A02"

    [Required(ErrorMessage = "Nhập họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    public string? MaVoucher { get; set; }
}
