# 🚌 Web Đặt Vé Xe — ASP.NET Core MVC + Entity Framework Core

Dự án mô phỏng hệ thống **đặt vé xe khách trực tuyến**, xây dựng lại từ dự án Node.js sang nền tảng **.NET** để học MVC + EF Core. Toàn bộ code & giao diện bằng **tiếng Việt**.

## 🛠️ Công nghệ

| Thành phần | Lựa chọn |
|---|---|
| Nền tảng | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 10 (Code-First + Migrations) |
| CSDL | SQL Server |
| Xác thực | ASP.NET Core Identity (role: Admin / Staff / User) |
| Giao diện | Razor Views + Bootstrap 5 + Font Awesome 6 |
| Thanh toán | Giả lập (MoMo / VNPay / ZaloPay / Tiền mặt) |

## 📁 Cấu trúc thư mục

```
WebDatVeXe/
├─ Models/              # Entities (TuyenXe, Xe, ChuyenXe, Ve, HoaDon, Voucher...) + Enums
├─ Data/
│  ├─ ApplicationDbContext.cs   # DbContext: DbSet, quan hệ, enum→string, index
│  ├─ SeedData.cs               # Khởi tạo roles, tài khoản, dữ liệu mẫu
│  └─ Migrations/               # Migration EF Core
├─ Repositories/        # IRepository<T> + Repository<T> (generic CRUD)
├─ Services/            # Nghiệp vụ: VoucherService, DatVeService (giữ chỗ, thanh toán)
├─ ViewModels/          # DTO cho View (TimChuyenVM, ChonGheVM, DatVeVM)
├─ Controllers/         # Trang người dùng (Home, Ve, LienHe)
├─ Areas/
│  ├─ Admin/            # Khu quản trị riêng (Controllers + Views)
│  └─ Identity/Pages/   # Trang đăng nhập & đăng ký tùy chỉnh (tiếng Việt)
├─ Views/               # Razor Views người dùng
└─ wwwroot/             # CSS, JS, Bootstrap
```

## 🧱 Kiến trúc phân lớp

```
Controller  →  Service  →  Repository / DbContext  →  SQL Server
   (HTTP)      (nghiệp vụ)      (truy cập dữ liệu)
```

- **Controller**: nhận request, gọi service, trả View.
- **Service** (`DatVeService`, `VoucherService`): chứa logic (giữ ghế, tính tiền, áp voucher, thanh toán).
- **Repository** (`Repository<T>`): CRUD chung trên EF Core.

## 🚀 Cách chạy

```bash
# 1. Khôi phục package
dotnet restore

# 2. Cấu hình connection string trong appsettings.json
#    "DefaultConnection": "Server=<TÊN_MÁY>;Database=WebDatVeXe;Trusted_Connection=True;TrustServerCertificate=True;"

# 3. Áp migration (app cũng tự chạy khi khởi động)
dotnet ef database update

# 4. Chạy
dotnet run --profile http
# Mở: http://localhost:5249
```

> Database tự tạo + seed dữ liệu mẫu ở lần chạy đầu (xem `Data/SeedData.cs`).

## 👤 Tài khoản mẫu

| Vai trò | Email | Mật khẩu |
|---|---|---|
| Admin | `admin@datvexe.vn` | `Admin@123` |
| Khách | `user@datvexe.vn` | `User@123` |

## ✨ Chức năng

**Người dùng**
- Trang chủ tìm chuyến (điểm đi / điểm đến / ngày)
- Chọn ghế trực quan theo sơ đồ, áp voucher
- Giữ chỗ 15 phút → thanh toán (giả lập) → xuất vé + hoá đơn
- Xem "Vé của tôi", chi tiết vé, huỷ vé
- Đăng ký / đăng nhập với giao diện tùy chỉnh tiếng Việt
- Gửi liên hệ

**Quản trị (Admin/Staff)**
- Dashboard thống kê (tuyến, xe, chuyến, vé, doanh thu)
- CRUD: Tuyến xe, Xe, Sơ đồ ghế, Chuyến xe, Voucher
- Quản lý vé đã đặt, khoá/mở tài khoản khách hàng

## 🔑 Quy ước đặt tên

- Tên class/Controller/Action: **PascalCase tiếng Việt không dấu** (`TuyenXeController`, `DatVe`, `ChonGhe`).
- Enum lưu DB dạng chuỗi (`HasConversion<string>()`) cho dễ đọc.
- Mã vé: `VE-yyyyMMdd-XXXXXX`, mã hoá đơn: `HD-yyyyMMdd-XXXXXX`.

## 🧩 Mô hình dữ liệu (chính)

`ApplicationUser` · `TuyenXe`──`DiemDungXe` · `SoDoGhe`──`Xe`──`ChuyenXe`
`ChuyenXe`──`Ve`──`HoaDon` · `Voucher`──`VoucherTuyenXe` · `DanhGia` · `ThongBao` · `LienHe` · `SupportTicket`
