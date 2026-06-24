namespace WebDatVeXe.Models;

// Bang noi nhieu-nhieu: voucher ap dung cho nhung tuyen nao (khi ApDungTuyen = Selected).
public class VoucherTuyenXe
{
    public int VoucherId { get; set; }
    public Voucher? Voucher { get; set; }

    public int TuyenXeId { get; set; }
    public TuyenXe? TuyenXe { get; set; }
}
