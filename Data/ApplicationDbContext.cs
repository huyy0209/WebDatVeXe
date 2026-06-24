using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDatVeXe.Models;

namespace WebDatVeXe.Data;

// DbContext chinh: ke thua IdentityDbContext de tich hop ASP.NET Core Identity
// voi user tuy bien la ApplicationUser (KhachHang).
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TuyenXe> TuyenXes => Set<TuyenXe>();
    public DbSet<DiemDungXe> DiemDungXes => Set<DiemDungXe>();
    public DbSet<SoDoGhe> SoDoGhes => Set<SoDoGhe>();
    public DbSet<Xe> Xes => Set<Xe>();
    public DbSet<ChuyenXe> ChuyenXes => Set<ChuyenXe>();
    public DbSet<Ve> Ves => Set<Ve>();
    public DbSet<HoaDon> HoaDons => Set<HoaDon>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<VoucherTuyenXe> VoucherTuyenXes => Set<VoucherTuyenXe>();
    public DbSet<DanhGia> DanhGias => Set<DanhGia>();
    public DbSet<ThongBao> ThongBaos => Set<ThongBao>();
    public DbSet<ThongBaoNguoiNhan> ThongBaoNguoiNhans => Set<ThongBaoNguoiNhan>();
    public DbSet<LienHe> LienHes => Set<LienHe>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ---- Luu enum duoi dang chuoi cho de doc trong DB ----
        builder.Entity<TuyenXe>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<DiemDungXe>().Property(x => x.Loai).HasConversion<string>();
        builder.Entity<Xe>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<ChuyenXe>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<Ve>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<HoaDon>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<Voucher>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<Voucher>().Property(x => x.LoaiGiamGia).HasConversion<string>();
        builder.Entity<Voucher>().Property(x => x.ApDungTuyen).HasConversion<string>();
        builder.Entity<LienHe>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<SupportTicket>().Property(x => x.TrangThai).HasConversion<string>();
        builder.Entity<ThongBao>().Property(x => x.Loai).HasConversion<string>();
        builder.Entity<ApplicationUser>().Property(x => x.TrangThai).HasConversion<string>();

        // ---- Index unique ----
        builder.Entity<Xe>().HasIndex(x => x.BienSo).IsUnique();
        builder.Entity<SoDoGhe>().HasIndex(x => x.TenSoDo).IsUnique();
        builder.Entity<Ve>().HasIndex(x => x.MaVe).IsUnique();
        builder.Entity<HoaDon>().HasIndex(x => x.MaHoaDon).IsUnique();
        builder.Entity<Voucher>().HasIndex(x => x.MaVoucher).IsUnique();

        // ---- Khoa chinh kep cho bang noi ----
        builder.Entity<VoucherTuyenXe>().HasKey(vt => new { vt.VoucherId, vt.TuyenXeId });

        // ---- Quan he + hanh vi xoa (tranh multiple cascade path tren SQL Server) ----
        builder.Entity<Xe>()
            .HasOne(x => x.SoDoGhe).WithMany(s => s.DanhSachXe)
            .HasForeignKey(x => x.SoDoGheId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Xe>()
            .HasOne(x => x.TuyenXe).WithMany(t => t.DanhSachXe)
            .HasForeignKey(x => x.TuyenXeId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ChuyenXe>()
            .HasOne(c => c.TuyenXe).WithMany(t => t.DanhSachChuyen)
            .HasForeignKey(c => c.TuyenXeId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ChuyenXe>()
            .HasOne(c => c.Xe).WithMany(x => x.DanhSachChuyen)
            .HasForeignKey(c => c.XeId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ve>()
            .HasOne(v => v.ChuyenXe).WithMany(c => c.DanhSachVe)
            .HasForeignKey(v => v.ChuyenXeId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ve>()
            .HasOne(v => v.KhachHang).WithMany(k => k.DanhSachVe)
            .HasForeignKey(v => v.KhachHangId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Ve>()
            .HasOne(v => v.Voucher).WithMany()
            .HasForeignKey(v => v.VoucherId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<HoaDon>()
            .HasOne(h => h.Ve).WithOne(v => v.HoaDon)
            .HasForeignKey<HoaDon>(h => h.VeId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<HoaDon>()
            .HasOne(h => h.KhachHang).WithMany()
            .HasForeignKey(h => h.KhachHangId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DanhGia>()
            .HasOne(d => d.KhachHang).WithMany(k => k.DanhSachDanhGia)
            .HasForeignKey(d => d.KhachHangId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DanhGia>()
            .HasOne(d => d.ChuyenXe).WithMany(c => c.DanhSachDanhGia)
            .HasForeignKey(d => d.ChuyenXeId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DiemDungXe>()
            .HasOne(d => d.TuyenXe).WithMany(t => t.DiemDungs)
            .HasForeignKey(d => d.TuyenXeId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<VoucherTuyenXe>()
            .HasOne(vt => vt.Voucher).WithMany(v => v.TuyenDuocApDung)
            .HasForeignKey(vt => vt.VoucherId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<VoucherTuyenXe>()
            .HasOne(vt => vt.TuyenXe).WithMany(t => t.VoucherApDung)
            .HasForeignKey(vt => vt.TuyenXeId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ThongBaoNguoiNhan>()
            .HasOne(t => t.ThongBao).WithMany(tb => tb.NguoiNhan)
            .HasForeignKey(t => t.ThongBaoId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ThongBaoNguoiNhan>()
            .HasOne(t => t.NguoiNhan).WithMany()
            .HasForeignKey(t => t.NguoiNhanId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SupportTicket>()
            .HasOne(s => s.Ve).WithMany()
            .HasForeignKey(s => s.VeId).OnDelete(DeleteBehavior.SetNull);
    }
}
