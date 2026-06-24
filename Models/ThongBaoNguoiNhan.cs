namespace WebDatVeXe.Models;

// Nguoi nhan cua mot thong bao + trang thai da doc.
public class ThongBaoNguoiNhan
{
    public int Id { get; set; }

    public int ThongBaoId { get; set; }
    public ThongBao? ThongBao { get; set; }

    public string NguoiNhanId { get; set; } = string.Empty;
    public ApplicationUser? NguoiNhan { get; set; }

    public bool IsRead { get; set; }
}
