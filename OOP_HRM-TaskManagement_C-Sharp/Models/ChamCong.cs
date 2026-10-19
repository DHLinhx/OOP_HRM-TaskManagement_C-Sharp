using com.hrmis.enums;

namespace com.hrmis.models;

public class ChamCong
{
    public int MaChamCong { get; set; }
    public int MaNhanVien { get; set; }
    public int MaCa { get; set; }
    public DateOnly NgayLamViec { get; set; }
    public TimeOnly? GioVaoThucTe { get; set; }
    public TimeOnly? GioRaThucTe { get; set; }
    public double SoGioLamThem { get; set; }
    public TrangThaiCong TrangThaiCong { get; set; }

    public ChamCong() { }

    public ChamCong(int maChamCong, int maNhanVien, int maCa, DateOnly ngayLamViec,
                    TimeOnly? gioVaoThucTe, TimeOnly? gioRaThucTe, double soGioLamThem, TrangThaiCong trangThaiCong)
    {
        MaChamCong = maChamCong;
        MaNhanVien = maNhanVien;
        MaCa = maCa;
        NgayLamViec = ngayLamViec;
        GioVaoThucTe = gioVaoThucTe;
        GioRaThucTe = gioRaThucTe;
        SoGioLamThem = soGioLamThem;
        TrangThaiCong = trangThaiCong;
    }

    public int getMaChamCong() => MaChamCong;
    public int getMaNhanVien() => MaNhanVien;
    public int getMaCa() => MaCa;
    public DateOnly getNgayLamViec() => NgayLamViec;
    public TimeOnly? getGioVaoThucTe() => GioVaoThucTe;
    public TimeOnly? getGioRaThucTe() => GioRaThucTe;
    public double getSoGioLamThem() => SoGioLamThem;
    public TrangThaiCong getTrangThaiCong() => TrangThaiCong;
}
