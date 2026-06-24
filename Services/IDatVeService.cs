using WebDatVeXe.Models;

namespace WebDatVeXe.Services;

public record KetQuaDatVe(bool ThanhCong, string? ThongBao, Ve? Ve);

public interface IDatVeService
{
    // Sinh ma duy nhat
    string TaoMaVe();
    string TaoMaHoaDon();

    // Giu cho (tao ve trang thai Hold) - kiem tra ghe con trong.
    Task<KetQuaDatVe> GiuChoAsync(int chuyenXeId, List<string> ghe, string khachHangId,
        string? hoTen, string? soDienThoai, string? email, string? maVoucher);

    // Xac nhan thanh toan (gia lap): chuyen ve sang Paid, tao hoa don, cap nhat ghe da dat.
    Task<KetQuaDatVe> XacNhanThanhToanAsync(int veId, string phuongThuc);

    // Huy ve
    Task<KetQuaDatVe> HuyVeAsync(int veId, string khachHangId);
}
