using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebDatVeXe.Models;

// Hoa don thanh toan cho mot ve. Ma hoa don tu sinh dang HD-yyyyMMdd-XXXXXX.
public class HoaDon
{
    public int Id { get; set; }

    [Display(Name = "Mã hoá đơn")]
    public string MaHoaDon { get; set; } = string.Empty;

    public int VeId { get; set; }
    public Ve? Ve { get; set; }

    public string KhachHangId { get; set; } = string.Empty;
    public ApplicationUser? KhachHang { get; set; }

    [Precision(18, 2), Display(Name = "Tổng tiền")]
    public decimal TongTien { get; set; }

    [Display(Name = "Phương thức thanh toán")]
    public string PhuongThucThanhToan { get; set; } = string.Empty;

    [Display(Name = "Trạng thái")]
    public TrangThaiHoaDon TrangThai { get; set; } = TrangThaiHoaDon.Pending;

    public DateTime NgayThanhToan { get; set; } = DateTime.Now;
}
