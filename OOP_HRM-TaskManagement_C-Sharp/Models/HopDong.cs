using com.hrmis.enums;

namespace com.hrmis.models;

public class HopDong
{
    public int MaHopDong { get; set; }
    public int MaNhanVien { get; set; }
    public string SoHopDong { get; set; } = string.Empty;
    public LoaiHopDong LoaiHopDong { get; set; }
    public DateOnly NgayHieuLuc { get; set; }
    public DateOnly? NgayHetHan { get; set; }
    public double LuongThoaThuan { get; set; }
    public TrangThaiHopDong TrangThai { get; set; }

    public HopDong() { }

    public HopDong(int maHopDong, int maNhanVien, string soHopDong, LoaiHopDong loaiHopDong,
                   DateOnly ngayHieuLuc, DateOnly? ngayHetHan, double luongThoaThuan, TrangThaiHopDong trangThai)
    {
        MaHopDong = maHopDong;
        MaNhanVien = maNhanVien;
        SoHopDong = soHopDong;
        LoaiHopDong = loaiHopDong;
        NgayHieuLuc = ngayHieuLuc;
        NgayHetHan = ngayHetHan;
        LuongThoaThuan = luongThoaThuan;
        TrangThai = trangThai;
    }

    public int getMaHopDong() => MaHopDong;
    public int getMaNhanVien() => MaNhanVien;
    public string getSoHopDong() => SoHopDong;
    public LoaiHopDong getLoaiHopDong() => LoaiHopDong;
    public DateOnly getNgayHieuLuc() => NgayHieuLuc;
    public DateOnly? getNgayHetHan() => NgayHetHan;
    public double getLuongThoaThuan() => LuongThoaThuan;
    public TrangThaiHopDong getTrangThai() => TrangThai;
}
