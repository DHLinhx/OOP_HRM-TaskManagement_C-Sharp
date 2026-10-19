using com.hrmis.enums;

namespace com.hrmis.models;

public class ChiTietBangLuong
{
    public int MaChiTietLuong { get; set; }
    public int MaBangLuong { get; set; }
    public string TenKhoanMuc { get; set; } = string.Empty;
    public LoaiKhoanMuc LoaiKhoanMuc { get; set; }
    public double SoTien { get; set; }
    public string GhiChu { get; set; } = string.Empty;

    public ChiTietBangLuong() { }

    public ChiTietBangLuong(int maChiTietLuong, int maBangLuong, string tenKhoanMuc,
                            LoaiKhoanMuc loaiKhoanMuc, double soTien, string ghiChu)
    {
        MaChiTietLuong = maChiTietLuong;
        MaBangLuong = maBangLuong;
        TenKhoanMuc = tenKhoanMuc;
        LoaiKhoanMuc = loaiKhoanMuc;
        SoTien = soTien;
        GhiChu = ghiChu;
    }

    public int getMaChiTietLuong() => MaChiTietLuong;
    public int getMaBangLuong() => MaBangLuong;
    public string getTenKhoanMuc() => TenKhoanMuc;
    public LoaiKhoanMuc getLoaiKhoanMuc() => LoaiKhoanMuc;
    public double getSoTien() => SoTien;
    public string getGhiChu() => GhiChu;
}
