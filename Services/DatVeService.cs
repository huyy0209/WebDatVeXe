using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Data;
using WebDatVeXe.Models;

namespace WebDatVeXe.Services;

// Xu ly nghiep vu dat ve: giu cho, ap voucher, thanh toan gia lap.
public class DatVeService : IDatVeService
{
    private readonly ApplicationDbContext _db;
    private readonly IVoucherService _voucherService;

    // Thoi gian giu cho (phut) truoc khi het han.
    private const int PHUT_GIU_CHO = 15;

    public DatVeService(ApplicationDbContext db, IVoucherService voucherService)
    {
        _db = db;
        _voucherService = voucherService;
    }

    public string TaoMaVe() => $"VE-{DateTime.Now:yyyyMMdd}-{RandomHex()}";
    public string TaoMaHoaDon() => $"HD-{DateTime.Now:yyyyMMdd}-{RandomHex()}";

    private static string RandomHex() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(3)).ToUpperInvariant();

    public async Task<KetQuaDatVe> GiuChoAsync(int chuyenXeId, List<string> ghe, string khachHangId,
        string? hoTen, string? soDienThoai, string? email, string? maVoucher)
    {
        if (ghe == null || ghe.Count == 0)
            return new KetQuaDatVe(false, "Vui lòng chọn ít nhất một ghế.", null);

        var chuyen = await _db.ChuyenXes
            .Include(c => c.TuyenXe)
            .Include(c => c.Xe)
            .FirstOrDefaultAsync(c => c.Id == chuyenXeId);

        if (chuyen == null)
            return new KetQuaDatVe(false, "Chuyến xe không tồn tại.", null);
        if (chuyen.TrangThai is TrangThaiChuyenXe.Cancelled or TrangThaiChuyenXe.Completed)
            return new KetQuaDatVe(false, "Chuyến xe không còn nhận đặt vé.", null);

        // Don dep cac ve hold da het han cua chuyen nay truoc khi kiem tra
        await GiaiPhongGheHetHanAsync(chuyenXeId);

        var daDat = chuyen.GheDaDat;
        var trung = ghe.Where(g => daDat.Contains(g)).ToList();
        if (trung.Count > 0)
            return new KetQuaDatVe(false, $"Ghế {string.Join(", ", trung)} đã có người đặt.", null);

        decimal giaVe = chuyen.TuyenXe?.GiaVe ?? 0;
        decimal tongTien = giaVe * ghe.Count;
        decimal soTienGiam = 0;
        int? voucherId = null;
        string? maVoucherApDung = null;

        if (!string.IsNullOrWhiteSpace(maVoucher))
        {
            var kq = await _voucherService.KiemTraAsync(maVoucher, tongTien,
                chuyen.TuyenXeId, khachHangId);
            if (!kq.HopLe)
                return new KetQuaDatVe(false, kq.ThongBao, null);
            soTienGiam = kq.SoTienGiam;
            voucherId = kq.Voucher!.Id;
            maVoucherApDung = kq.Voucher.MaVoucher;
        }

        var ve = new Ve
        {
            MaVe = TaoMaVe(),
            KhachHangId = khachHangId,
            ChuyenXeId = chuyenXeId,
            DanhSachGhe = ghe,
            TongTien = tongTien - soTienGiam,
            SoTienGiam = soTienGiam,
            VoucherId = voucherId,
            MaVoucher = maVoucherApDung,
            HoTen = hoTen,
            SoDienThoai = soDienThoai,
            Email = email,
            TrangThai = TrangThaiVe.Hold,
            HoldExpires = DateTime.Now.AddMinutes(PHUT_GIU_CHO),
            NgayDat = DateTime.Now
        };

        await _db.Ves.AddAsync(ve);
        await _db.SaveChangesAsync();
        return new KetQuaDatVe(true, "Đã giữ chỗ. Vui lòng thanh toán trong 15 phút.", ve);
    }

    public async Task<KetQuaDatVe> XacNhanThanhToanAsync(int veId, string phuongThuc)
    {
        var ve = await _db.Ves.Include(v => v.ChuyenXe).FirstOrDefaultAsync(v => v.Id == veId);
        if (ve == null)
            return new KetQuaDatVe(false, "Vé không tồn tại.", null);
        if (ve.TrangThai == TrangThaiVe.Paid || ve.TrangThai == TrangThaiVe.Confirmed)
            return new KetQuaDatVe(true, "Vé đã được thanh toán.", ve);
        if (ve.TrangThai == TrangThaiVe.Expired || ve.TrangThai == TrangThaiVe.Cancelled)
            return new KetQuaDatVe(false, "Vé đã hết hạn hoặc bị huỷ.", null);

        // Them ghe vao danh sach da dat cua chuyen
        var chuyen = ve.ChuyenXe!;
        var daDat = chuyen.GheDaDat;
        foreach (var g in ve.DanhSachGhe)
            if (!daDat.Contains(g)) daDat.Add(g);
        chuyen.GheDaDat = daDat;

        ve.TrangThai = TrangThaiVe.Paid;
        ve.PhuongThucThanhToan = phuongThuc;
        ve.MaGiaoDich = $"TXN-{Guid.NewGuid():N}".Substring(0, 16).ToUpperInvariant();
        ve.HoldExpires = null;

        // Cap nhat luot dung voucher
        if (ve.VoucherId.HasValue)
        {
            var voucher = await _db.Vouchers.FindAsync(ve.VoucherId.Value);
            if (voucher != null) voucher.DaSuDung += 1;
        }

        var hoaDon = new HoaDon
        {
            MaHoaDon = TaoMaHoaDon(),
            VeId = ve.Id,
            KhachHangId = ve.KhachHangId,
            TongTien = ve.TongTien,
            PhuongThucThanhToan = phuongThuc,
            TrangThai = TrangThaiHoaDon.Completed,
            NgayThanhToan = DateTime.Now
        };
        await _db.HoaDons.AddAsync(hoaDon);

        await _db.SaveChangesAsync();
        return new KetQuaDatVe(true, "Thanh toán thành công!", ve);
    }

    public async Task<KetQuaDatVe> HuyVeAsync(int veId, string khachHangId)
    {
        var ve = await _db.Ves.Include(v => v.ChuyenXe).FirstOrDefaultAsync(v => v.Id == veId);
        if (ve == null) return new KetQuaDatVe(false, "Vé không tồn tại.", null);
        if (ve.KhachHangId != khachHangId)
            return new KetQuaDatVe(false, "Bạn không có quyền huỷ vé này.", null);
        if (ve.TrangThai == TrangThaiVe.Completed)
            return new KetQuaDatVe(false, "Vé đã hoàn thành, không thể huỷ.", null);

        // Tra ghe lai cho chuyen
        if (ve.ChuyenXe != null)
        {
            var daDat = ve.ChuyenXe.GheDaDat;
            daDat.RemoveAll(g => ve.DanhSachGhe.Contains(g));
            ve.ChuyenXe.GheDaDat = daDat;
        }

        ve.TrangThai = TrangThaiVe.Cancelled;
        await _db.SaveChangesAsync();
        return new KetQuaDatVe(true, "Đã huỷ vé.", ve);
    }

    // Giai phong ghe cua cac ve Hold da qua han cho mot chuyen.
    private async Task GiaiPhongGheHetHanAsync(int chuyenXeId)
    {
        var now = DateTime.Now;
        var heetHan = await _db.Ves
            .Where(v => v.ChuyenXeId == chuyenXeId &&
                        v.TrangThai == TrangThaiVe.Hold &&
                        v.HoldExpires != null && v.HoldExpires < now)
            .ToListAsync();
        foreach (var v in heetHan)
            v.TrangThai = TrangThaiVe.Expired;
        if (heetHan.Count > 0) await _db.SaveChangesAsync();
    }
}
