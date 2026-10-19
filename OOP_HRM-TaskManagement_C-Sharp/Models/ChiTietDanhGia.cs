namespace com.hrmis.models;

public class ChiTietDanhGia
{
    public int MaChiTietDanhGia { get; set; }
    public int MaDanhGia { get; set; }
    public string TieuChiDanhGia { get; set; } = string.Empty;
    public double TrongSo { get; set; }
    public double DiemSo { get; set; }
    public string NhanXetChiTiet { get; set; } = string.Empty;

    public ChiTietDanhGia() { }

    public ChiTietDanhGia(int maChiTietDanhGia, int maDanhGia, string tieuChiDanhGia,
                          double trongSo, double diemSo, string nhanXetChiTiet)
    {
        MaChiTietDanhGia = maChiTietDanhGia;
        MaDanhGia = maDanhGia;
        TieuChiDanhGia = tieuChiDanhGia;
        TrongSo = trongSo;
        DiemSo = diemSo;
        NhanXetChiTiet = nhanXetChiTiet;
    }

    public int getMaChiTietDanhGia() => MaChiTietDanhGia;
    public int getMaDanhGia() => MaDanhGia;
    public string getTieuChiDanhGia() => TieuChiDanhGia;
    public double getTrongSo() => TrongSo;
    public double getDiemSo() => DiemSo;
    public string getNhanXetChiTiet() => NhanXetChiTiet;
}
