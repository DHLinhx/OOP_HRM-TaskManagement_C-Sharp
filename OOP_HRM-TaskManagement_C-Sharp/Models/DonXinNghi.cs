using com.hrmis.enums;

namespace com.hrmis.models;

public class DonXinNghi
{
    public int MaDonNghi { get; set; }
    public int MaNhanVien { get; set; }
    public LoaiNghi LoaiNghi { get; set; }
    public DateOnly NgayBatDau { get; set; }
    public DateOnly NgayKetThuc { get; set; }
    public string LyDo { get; set; } = string.Empty;
    public TrangThaiDuyet TrangThaiDuyet { get; set; }
    public int? NguoiDuyet { get; set; }

    public DonXinNghi() { }

    public DonXinNghi(int maDonNghi, int maNhanVien, LoaiNghi loaiNghi, DateOnly ngayBatDau,
                      DateOnly ngayKetThuc, string lyDo, TrangThaiDuyet trangThaiDuyet, int? nguoiDuyet)
    {
        MaDonNghi = maDonNghi;
        MaNhanVien = maNhanVien;
        LoaiNghi = loaiNghi;
        NgayBatDau = ngayBatDau;
        NgayKetThuc = ngayKetThuc;
        LyDo = lyDo;
        TrangThaiDuyet = trangThaiDuyet;
        NguoiDuyet = nguoiDuyet;
    }

    public int getMaDonNghi() => MaDonNghi;
    public int getMaNhanVien() => MaNhanVien;
    public LoaiNghi getLoaiNghi() => LoaiNghi;
    public DateOnly getNgayBatDau() => NgayBatDau;
    public DateOnly getNgayKetThuc() => NgayKetThuc;
    public string getLyDo() => LyDo;
    public TrangThaiDuyet getTrangThaiDuyet() => TrangThaiDuyet;
    public int? getNguoiDuyet() => NguoiDuyet;
}
