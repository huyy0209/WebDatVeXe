using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebDatVeXe.Models;

// Tuyến đường cố định: điểm đi -> điểm đến, kèm giá vé và các điểm đón/trả.
public class TuyenXe
{
    public int Id { get; set; }

    [Required, Display(Name = "Điểm đi")]
    public string DiemDi { get; set; } = string.Empty;

    [Required, Display(Name = "Điểm đến")]
    public string DiemDen { get; set; } = string.Empty;

    [Display(Name = "Khoảng cách")]
    public string? KhoangCach { get; set; }

    [Display(Name = "Thời gian đi")]
    public string? ThoiGianDi { get; set; }

    [Precision(18, 2), Display(Name = "Giá vé")]
    public decimal GiaVe { get; set; }

    [Display(Name = "Trạng thái")]
    public TrangThaiHoatDong TrangThai { get; set; } = TrangThaiHoatDong.Active;

    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Quan hệ điều hướng
    public ICollection<DiemDungXe> DiemDungs { get; set; } = new List<DiemDungXe>();
    public ICollection<Xe> DanhSachXe { get; set; } = new List<Xe>();
    public ICollection<ChuyenXe> DanhSachChuyen { get; set; } = new List<ChuyenXe>();
    public ICollection<VoucherTuyenXe> VoucherApDung { get; set; } = new List<VoucherTuyenXe>();
}
