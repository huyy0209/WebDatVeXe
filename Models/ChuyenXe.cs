using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatVeXe.Models;

// Mot chuyen xe cu the: gan tuyen + xe + gio khoi hanh. GheDaDat luu CSV.
public class ChuyenXe
{
    public int Id { get; set; }

    public int TuyenXeId { get; set; }
    public TuyenXe? TuyenXe { get; set; }

    public int XeId { get; set; }
    public Xe? Xe { get; set; }

    [Display(Name = "Thời gian khởi hành")]
    public DateTime ThoiGianKhoiHanh { get; set; }

    [Display(Name = "Thời gian đến (dự kiến)")]
    public DateTime? ThoiGianDen { get; set; }

    [Display(Name = "Trạng thái")]
    public TrangThaiChuyenXe TrangThai { get; set; } = TrangThaiChuyenXe.Active;

    public string GheDaDatCsv { get; set; } = string.Empty;

    [NotMapped]
    public List<string> GheDaDat
    {
        get => string.IsNullOrWhiteSpace(GheDaDatCsv)
            ? new List<string>()
            : GheDaDatCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        set => GheDaDatCsv = string.Join(',', value);
    }

    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<Ve> DanhSachVe { get; set; } = new List<Ve>();
    public ICollection<DanhGia> DanhSachDanhGia { get; set; } = new List<DanhGia>();
}
