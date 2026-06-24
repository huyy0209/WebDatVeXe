using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Điểm đón hoặc điểm trả khách thuộc một tuyến xe.
public class DiemDungXe
{
    public int Id { get; set; }

    public int TuyenXeId { get; set; }
    public TuyenXe? TuyenXe { get; set; }

    [Display(Name = "Loại điểm")]
    public LoaiDiemDung Loai { get; set; } = LoaiDiemDung.Don;

    [Required, Display(Name = "Tên điểm")]
    public string TenDiem { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Required, Display(Name = "Tỉnh/Thành")]
    public string TinhThanh { get; set; } = string.Empty;

    public int ThuTu { get; set; }
}
