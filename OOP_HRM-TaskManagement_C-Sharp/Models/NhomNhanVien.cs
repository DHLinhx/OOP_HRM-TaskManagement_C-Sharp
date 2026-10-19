namespace com.hrmis.models;

public class NhomNhanVien
{
    public int? maNhom { get; set; }
    public string tenNhom { get; set; } = string.Empty;
    public int? maTruongNhom { get; set; }
    public DateOnly ngayThanhLap { get; set; }
    public string MoTa { get; set; } = string.Empty;

    public NhomNhanVien() { }

    public NhomNhanVien(int? maNhom, string tenNhom, int? maTruongNhom, DateOnly ngayThanhLap, string MoTa)
    {
        this.maNhom = maNhom;
        this.tenNhom = tenNhom;
        this.maTruongNhom = maTruongNhom;
        this.ngayThanhLap = ngayThanhLap;
        this.MoTa = MoTa;
    }

    public int? getmaNhom() => maNhom;
    public string gettenNhom() => tenNhom;
    public int? getmaTruongNhom() => maTruongNhom;
    public DateOnly getngayThanhLap() => ngayThanhLap;

    public override string ToString() => $"ma Nhom: {maNhom}, ten nhom: {tenNhom}, ma truong phong: {maTruongNhom}, ngay thanh lap: {ngayThanhLap}, mo ta: {MoTa}";
}
