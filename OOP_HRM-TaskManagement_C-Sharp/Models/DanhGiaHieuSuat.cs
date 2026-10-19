using com.hrmis.enums;

namespace com.hrmis.models;

public class DanhGiaHieuSuat
{
    public int MaDanhGia { get; set; }
    public int MaNhanVien { get; set; }
    public int NguoiDanhGia { get; set; }
    public string KyDanhGia { get; set; } = string.Empty;
    public int Nam { get; set; }
    public double DiemTrungBinh { get; set; }
    public XepLoai XepLoai { get; set; }

    public DanhGiaHieuSuat() { }

    public DanhGiaHieuSuat(int maDanhGia, int maNhanVien, int nguoiDanhGia,
                           string kyDanhGia, int nam, double diemTrungBinh, XepLoai xepLoai)
    {
        MaDanhGia = maDanhGia;
        MaNhanVien = maNhanVien;
        NguoiDanhGia = nguoiDanhGia;
        KyDanhGia = kyDanhGia;
        Nam = nam;
        DiemTrungBinh = diemTrungBinh;
        XepLoai = xepLoai;
    }

    public int getMaDanhGia() => MaDanhGia;
    public int getMaNhanVien() => MaNhanVien;
    public int getNguoiDanhGia() => NguoiDanhGia;
    public string getKyDanhGia() => KyDanhGia;
    public int getNam() => Nam;
    public double getDiemTrungBinh() => DiemTrungBinh;
    public XepLoai getXepLoai() => XepLoai;
}
