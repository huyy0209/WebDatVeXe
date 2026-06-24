using System.ComponentModel.DataAnnotations;

namespace WebDatVeXe.Models;

// Thong bao gui toi nguoi dung (he thong, dat ve, ho tro...).
public class ThongBao
{
    public int Id { get; set; }

    [Required, Display(Name = "Tiêu đề")]
    public string TieuDe { get; set; } = string.Empty;

    [Required, Display(Name = "Nội dung")]
    public string NoiDung { get; set; } = string.Empty;

    public LoaiThongBao Loai { get; set; } = LoaiThongBao.System;
    public string Sender { get; set; } = "System";

    public int? RelatedId { get; set; }
    public bool IsAdminOnly { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<ThongBaoNguoiNhan> NguoiNhan { get; set; } = new List<ThongBaoNguoiNhan>();
}
