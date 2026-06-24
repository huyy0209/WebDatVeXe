using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Phương tiện (xe) gắn với một sơ đồ ghế và (tuỳ chọn) một tuyến cố định.
public class Xe
{
    public int Id { get; set; }

    [Required, Display(Name = "Biển số")]
    public string BienSo { get; set; } = string.Empty;

    [Required, Display(Name = "Loại xe")]
    public string LoaiXe { get; set; } = string.Empty;

    [Display(Name = "Tổng số ghế")]
    public int TongSoGhe { get; set; }

    public int SoDoGheId { get; set; }
    public SoDoGhe? SoDoGhe { get; set; }

    public int? TuyenXeId { get; set; }
    public TuyenXe? TuyenXe { get; set; }

    [Display(Name = "Số tầng")]
    public int SoTang { get; set; } = 1;

    [Display(Name = "Trạng thái")]
    public TrangThaiHoatDong TrangThai { get; set; } = TrangThaiHoatDong.Active;

    public ICollection<ChuyenXe> DanhSachChuyen { get; set; } = new List<ChuyenXe>();
}
