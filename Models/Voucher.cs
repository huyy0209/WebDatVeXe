using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebDatVeXe.Models;

// Ma giam gia ap dung khi dat ve.
public class Voucher
{
    public int Id { get; set; }

    [Required, Display(Name = "Tên voucher")]
    public string TenVoucher { get; set; } = string.Empty;

    [Required, Display(Name = "Mã voucher")]
    public string MaVoucher { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Display(Name = "Loại giảm giá")]
    public LoaiGiamGia LoaiGiamGia { get; set; } = LoaiGiamGia.Fixed;

    [Precision(18, 2), Display(Name = "Giá trị giảm")]
    public decimal GiaTriGiam { get; set; }

    [Precision(18, 2), Display(Name = "Đơn tối thiểu")]
    public decimal GiaTriToiThieu { get; set; }

    [Precision(18, 2), Display(Name = "Giảm tối đa")]
    public decimal? GiamToiDa { get; set; }

    public DateTime NgayBatDau { get; set; } = DateTime.Now;
    public DateTime? NgayHetHan { get; set; }

    [Display(Name = "Số lượng")]
    public int SoLuong { get; set; } = 100;
    public int DaSuDung { get; set; }

    [Display(Name = "Trạng thái")]
    public TrangThaiHoatDong TrangThai { get; set; } = TrangThaiHoatDong.Active;

    public bool ChoKhachHangMoi { get; set; }
    public int LuotDungToiDaMoiNguoi { get; set; } = 1;

    [Display(Name = "Phạm vi áp dụng")]
    public ApDungTuyen ApDungTuyen { get; set; } = ApDungTuyen.All;

    public ICollection<VoucherTuyenXe> TuyenDuocApDung { get; set; } = new List<VoucherTuyenXe>();
}
