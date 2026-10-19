using com.hrmis.enums;

namespace com.hrmis.models;

public class BangLuong
{
    public int MaBangLuong { get; set; }
    public int MaNhanVien { get; set; }
    public int KyThang { get; set; }
    public int KyNam { get; set; }
    public double TongThuNhap { get; set; }
    public double TongKhauTru { get; set; }
    public double ThucLanh { get; set; }
    public TrangThaiChiTra TrangThaiChiTra { get; set; }
    public DateOnly? NgayChiTra { get; set; }

    public BangLuong() { }

    public BangLuong(int maBangLuong, int maNhanVien, int kyThang, int kyNam, double tongThuNhap,
                     double tongKhauTru, double thucLanh, TrangThaiChiTra trangThaiChiTra, DateOnly? ngayChiTra)
    {
        MaBangLuong = maBangLuong;
        MaNhanVien = maNhanVien;
        KyThang = kyThang;
        KyNam = kyNam;
        TongThuNhap = tongThuNhap;
        TongKhauTru = tongKhauTru;
        ThucLanh = thucLanh;
        TrangThaiChiTra = trangThaiChiTra;
        NgayChiTra = ngayChiTra;
    }

    public int getMaBangLuong() => MaBangLuong;
    public int getMaNhanVien() => MaNhanVien;
    public int getKyThang() => KyThang;
    public int getKyNam() => KyNam;
    public double getTongThuNhap() => TongThuNhap;
    public double getTongKhauTru() => TongKhauTru;
    public double getThucLanh() => ThucLanh;
    public TrangThaiChiTra getTrangThaiChiTra() => TrangThaiChiTra;
    public DateOnly? getNgayChiTra() => NgayChiTra;
}
