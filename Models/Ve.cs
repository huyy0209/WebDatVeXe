using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebDatVeXe.Models;

// Ve dat cho mot chuyen xe. Ma ve tu sinh dang VE-yyyyMMdd-XXXXXX.
public class Ve
{
    public int Id { get; set; }

    [Display(Name = "Mã vé")]
    public string MaVe { get; set; } = string.Empty;

    public string KhachHangId { get; set; } = string.Empty;
    public ApplicationUser? KhachHang { get; set; }

    public int ChuyenXeId { get; set; }
    public ChuyenXe? ChuyenXe { get; set; }

    public string DanhSachGheCsv { get; set; } = string.Empty;

    [NotMapped]
    public List<string> DanhSachGhe
    {
        get => string.IsNullOrWhiteSpace(DanhSachGheCsv)
            ? new List<string>()
            : DanhSachGheCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        set => DanhSachGheCsv = string.Join(',', value);
    }

    [Precision(18, 2), Display(Name = "Tổng tiền")]
    public decimal TongTien { get; set; }

    public string? HoTen { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }

    public string? MaVoucher { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
    [Precision(18, 2)]
    public decimal SoTienGiam { get; set; }

    public string? DiemDonTen { get; set; }
    public string? DiemDonDiaChi { get; set; }
    public string? DiemTraTen { get; set; }
    public string? DiemTraDiaChi { get; set; }

    [Display(Name = "Phương thức thanh toán")]
    public string? PhuongThucThanhToan { get; set; }
    public string? MaGiaoDich { get; set; }
    public string? GhiChu { get; set; }

    [Display(Name = "Trạng thái")]
    public TrangThaiVe TrangThai { get; set; } = TrangThaiVe.Hold;

    public DateTime? HoldExpires { get; set; }
    public DateTime NgayDat { get; set; } = DateTime.Now;

    public HoaDon? HoaDon { get; set; }
}
