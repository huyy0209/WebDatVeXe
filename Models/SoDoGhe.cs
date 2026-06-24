using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatVeXe.Models;

// Sơ đồ ghế của một loại xe. Danh sách ghế lưu dạng CSV: "A01,A02,B01,...".
public class SoDoGhe
{
    public int Id { get; set; }

    [Required, Display(Name = "Tên sơ đồ")]
    public string TenSoDo { get; set; } = string.Empty;

    [Display(Name = "Tổng số ghế")]
    public int TongSoGhe { get; set; }

    [Display(Name = "Số tầng")]
    public int SoTang { get; set; } = 1;

    // Lưu danh sách ghế ngăn cách bằng dấu phẩy.
    [Display(Name = "Danh sách ghế")]
    public string DanhSachGheCsv { get; set; } = string.Empty;

    [NotMapped]
    public List<string> DanhSachGhe
    {
        get => string.IsNullOrWhiteSpace(DanhSachGheCsv)
            ? new List<string>()
            : DanhSachGheCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        set => DanhSachGheCsv = string.Join(',', value);
    }

    public ICollection<Xe> DanhSachXe { get; set; } = new List<Xe>();
}
