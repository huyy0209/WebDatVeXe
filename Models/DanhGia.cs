using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Danh gia cua khach hang cho mot chuyen xe da di.
public class DanhGia
{
    public int Id { get; set; }

    public string KhachHangId { get; set; } = string.Empty;
    public ApplicationUser? KhachHang { get; set; }

    public int ChuyenXeId { get; set; }
    public ChuyenXe? ChuyenXe { get; set; }

    [Range(1, 5), Display(Name = "Số sao")]
    public int SoSao { get; set; }

    [Required, Display(Name = "Nhận xét")]
    public string NhanXet { get; set; } = string.Empty;

    public DateTime NgayDanhGia { get; set; } = DateTime.Now;
}
