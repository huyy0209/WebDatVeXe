namespace WebDatVeXe.Models;

// Các kiểu trạng thái/loại dùng chung trong toàn hệ thống.
// Lưu xuống DB dưới dạng chuỗi (cấu hình HasConversion<string>() trong DbContext).

public enum TrangThaiTaiKhoan { Active, Inactive }

public enum TrangThaiHoatDong { Active, Inactive, Maintenance }

public enum TrangThaiChuyenXe { Active, Running, Completed, Cancelled, Inactive }

public enum TrangThaiVe { Hold, Pending, Paid, Confirmed, Completed, Cancelled, Expired }

public enum TrangThaiHoaDon { Pending, Completed, Refunded }

public enum LoaiGiamGia { Percentage, Fixed }

public enum LoaiDiemDung { Don, Tra }

public enum TrangThaiLienHe { Pending, Resolved }

public enum TrangThaiTicket { Open, InProgress, Resolved, Closed }

public enum LoaiThongBao { TripCancelled, NewBooking, System, Support, Cancel }

public enum ApDungTuyen { All, Selected }

// Vai trò người dùng (khớp với Identity Roles được seed sẵn).
public static class VaiTro
{
    public const string Admin = "Admin";
    public const string Staff = "Staff";
    public const string User = "User";
}
