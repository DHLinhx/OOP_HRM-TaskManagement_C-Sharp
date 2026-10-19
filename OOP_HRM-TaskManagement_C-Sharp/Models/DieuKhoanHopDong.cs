namespace com.hrmis.models;

public class DieuKhoanHopDong
{
    public int MaDieuKhoan { get; set; }
    public int MaHopDong { get; set; }
    public int SoThuTuDieu { get; set; }
    public string TieuDeDieuKhoan { get; set; } = string.Empty;
    public string NoiDungDieuKhoan { get; set; } = string.Empty;
    public string LoaiDieuKhoan { get; set; } = string.Empty;

    public DieuKhoanHopDong() { }

    public DieuKhoanHopDong(int maDieuKhoan, int maHopDong, int soThuTuDieu,
                            string tieuDeDieuKhoan, string noiDungDieuKhoan, string loaiDieuKhoan)
    {
        MaDieuKhoan = maDieuKhoan;
        MaHopDong = maHopDong;
        SoThuTuDieu = soThuTuDieu;
        TieuDeDieuKhoan = tieuDeDieuKhoan;
        NoiDungDieuKhoan = noiDungDieuKhoan;
        LoaiDieuKhoan = loaiDieuKhoan;
    }

    public int getMaDieuKhoan() => MaDieuKhoan;
    public int getMaHopDong() => MaHopDong;
    public int getSoThuTuDieu() => SoThuTuDieu;
    public string getTieuDeDieuKhoan() => TieuDeDieuKhoan;
    public string getNoiDungDieuKhoan() => NoiDungDieuKhoan;
    public string getLoaiDieuKhoan() => LoaiDieuKhoan;
}
