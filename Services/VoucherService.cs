using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Services;

public class VoucherService : IVoucherService
{
    private readonly ApplicationDbContext _db;

    public VoucherService(ApplicationDbContext db) => _db = db;

    public async Task<KetQuaVoucher> KiemTraAsync(string maVoucher, decimal tongTien, int tuyenXeId, string khachHangId)
    {
        var v = await _db.Vouchers
            .Include(x => x.TuyenDuocApDung)
            .FirstOrDefaultAsync(x => x.MaVoucher == maVoucher);

        if (v == null)
            return new KetQuaVoucher(false, "Mã voucher không tồn tại.", null, 0);

        if (v.TrangThai != TrangThaiHoatDong.Active)
            return new KetQuaVoucher(false, "Voucher đã ngừng hoạt động.", null, 0);

        var now = DateTime.Now;
        if (v.NgayBatDau > now)
            return new KetQuaVoucher(false, "Voucher chưa tới ngày sử dụng.", null, 0);
        if (v.NgayHetHan.HasValue && v.NgayHetHan.Value < now)
            return new KetQuaVoucher(false, "Voucher đã hết hạn.", null, 0);

        if (v.DaSuDung >= v.SoLuong)
            return new KetQuaVoucher(false, "Voucher đã hết lượt sử dụng.", null, 0);

        if (tongTien < v.GiaTriToiThieu)
            return new KetQuaVoucher(false, $"Đơn tối thiểu {v.GiaTriToiThieu:N0} đ để dùng voucher này.", null, 0);

        if (v.ApDungTuyen == ApDungTuyen.Selected &&
            !v.TuyenDuocApDung.Any(t => t.TuyenXeId == tuyenXeId))
            return new KetQuaVoucher(false, "Voucher không áp dụng cho tuyến này.", null, 0);

        // Gioi han luot dung moi nguoi
        var daDung = await _db.Ves.CountAsync(x =>
            x.VoucherId == v.Id && x.KhachHangId == khachHangId &&
            x.TrangThai != TrangThaiVe.Cancelled && x.TrangThai != TrangThaiVe.Expired);
        if (daDung >= v.LuotDungToiDaMoiNguoi)
            return new KetQuaVoucher(false, "Bạn đã dùng hết lượt cho voucher này.", null, 0);

        // Tinh so tien giam
        decimal giam = v.LoaiGiamGia == LoaiGiamGia.Percentage
            ? tongTien * v.GiaTriGiam / 100m
            : v.GiaTriGiam;

        if (v.GiamToiDa.HasValue && giam > v.GiamToiDa.Value)
            giam = v.GiamToiDa.Value;
        if (giam > tongTien) giam = tongTien;

        return new KetQuaVoucher(true, "Áp dụng voucher thành công.", v, Math.Round(giam));
    }
}
