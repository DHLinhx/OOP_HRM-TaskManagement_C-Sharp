namespace com.hrmis.models;

public class ThanhVienNhom
{
    public int? maNhom { get; set; }
    public int maNhanVien { get; set; }
    public DateOnly ngayThamGia { get; set; }
    public string VaiTro { get; set; } = string.Empty;

    public ThanhVienNhom() { }

    public ThanhVienNhom(int? maNhom, int maNhanVien, DateOnly ngayThamGia, string VaiTro)
    {
        this.maNhom = maNhom;
        this.maNhanVien = maNhanVien;
        this.ngayThamGia = ngayThamGia;
        this.VaiTro = VaiTro;
    }

    public int? getmaNhom() => maNhom;
    public int getnhomNhanVien() => maNhanVien;
    public DateOnly getngayThamGia() => ngayThamGia;
    public string getVaiTro() => VaiTro;

    public override string ToString() => $"Thanh vien nhom{{ ma nhom {maNhom} vai tro: {VaiTro}";
}
