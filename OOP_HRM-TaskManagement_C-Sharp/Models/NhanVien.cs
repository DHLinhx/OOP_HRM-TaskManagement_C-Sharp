using com.hrmis.enums;

namespace com.hrmis.models;

public class NhanVien
{
    public int MaNhanVien { get; set; }
    public string Ho { get; set; } = string.Empty;
    public string Ten { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public DateOnly NgaySinh { get; set; }
    public GioiTinh GioiTinh { get; set; }
    public DateOnly NgayVaoLam { get; set; }
    public TrangThaiLamViec TrangThaiLamViec { get; set; }
    public int MaPhongBan { get; set; }
    public int MaChucVu { get; set; }
    public int? MaQuanLy { get; set; }

    public NhanVien() { }

    public NhanVien(int maNhanVien, string ho, string ten, string email, string soDienThoai,
                    DateOnly ngaySinh, GioiTinh gioiTinh, DateOnly ngayVaoLam,
                    TrangThaiLamViec trangThaiLamViec, int maPhongBan, int maChucVu, int? maQuanLy)
    {
        MaNhanVien = maNhanVien;
        Ho = ho;
        Ten = ten;
        Email = email;
        SoDienThoai = soDienThoai;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        NgayVaoLam = ngayVaoLam;
        TrangThaiLamViec = trangThaiLamViec;
        MaPhongBan = maPhongBan;
        MaChucVu = maChucVu;
        MaQuanLy = maQuanLy;
    }

    public int getMaNhanVien() => MaNhanVien;
    public string getHo() => Ho;
    public string getTen() => Ten;
    public string getEmail() => Email;
    public string getSoDienThoai() => SoDienThoai;
    public DateOnly getNgaySinh() => NgaySinh;
    public GioiTinh getGioiTinh() => GioiTinh;
    public DateOnly getNgayVaoLam() => NgayVaoLam;
    public TrangThaiLamViec getTrangThaiLamViec() => TrangThaiLamViec;
    public int getMaPhongBan() => MaPhongBan;
    public int getMaChucVu() => MaChucVu;
    public int? getMaQuanLy() => MaQuanLy;

    public string HoTenNV() => $"{Ho} {Ten}";

    public override string ToString() => $"NhanVien{{maNhanVien={MaNhanVien}, hoTen='{Ho} {Ten}', email='{Email}', soDienThoai='{SoDienThoai}', gioiTinh={GioiTinh}, trangThaiLamViec={TrangThaiLamViec}}}";
}
