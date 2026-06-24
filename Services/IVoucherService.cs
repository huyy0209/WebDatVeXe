using WebDatVeXe.Models;

namespace WebDatVeXe.Services;

// Ket qua kiem tra voucher.
public record KetQuaVoucher(bool HopLe, string? ThongBao, Voucher? Voucher, decimal SoTienGiam);

public interface IVoucherService
{
    // Kiem tra ma voucher co the ap dung cho don hang khong, va tinh so tien giam.
    Task<KetQuaVoucher> KiemTraAsync(string maVoucher, decimal tongTien, int tuyenXeId, string khachHangId);
}
