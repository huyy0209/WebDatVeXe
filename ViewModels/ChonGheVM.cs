using WebDatVeXe.Models;

namespace WebDatVeXe.ViewModels;

// Du lieu trang chon ghe cho mot chuyen.
public class ChonGheVM
{
    public ChuyenXe ChuyenXe { get; set; } = default!;
    public List<string> TatCaGhe { get; set; } = new();
    public List<string> GheDaDat { get; set; } = new();
    public decimal GiaVe { get; set; }
}
