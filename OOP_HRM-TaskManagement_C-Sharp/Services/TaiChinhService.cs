using System.Globalization;
using System.Text;
using com.hrmis.dao;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.services;

public class TaiChinhService
{
    private readonly NhanVienDAO nhanVienDAO = new();
    private readonly PhongBanDAO phongBanDAO = new();
    private readonly ChucVuDAO chucVuDAO = new();
    private readonly BangLuongDAO bangLuongDAO = new();
    private readonly ChiTietBangLuongDAO chiTietBangLuongDAO = new();
    private readonly DanhGiaHieuSuatDAO danhGiaHieuSuatDAO = new();
    private readonly ChiTietDanhGiaDAO chiTietDanhGiaDAO = new();
    private readonly DonXinNghiDAO donXinNghiDAO = new();
    private readonly ChamCongDAO chamCongDAO = new();

    private static readonly CultureInfo VI_CULTURE = CultureInfo.GetCultureInfo("vi-VN");
    private static readonly string LINE = new string('-', 78);

    private static string tien(double v) => Math.Round(v).ToString("N0", VI_CULTURE);

    private static string chuanHoa(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        string normalized = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (char c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC)
            .Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
    }

    private static string hoTen(NhanVien nv) => nv.HoTenNV();

    private NhanVien? timNhanVien(int maNV) => nhanVienDAO.GetById(maNV);

    private string tenNV(int maNV)
    {
        var nv = timNhanVien(maNV);
        return nv == null ? "(không rõ #" + maNV + ")" : hoTen(nv);
    }

    private string tenPhongBan(int maPB) => phongBanDAO.GetTenPhongBan(maPB);

    private string tenChucVu(int maCV) => chucVuDAO.GetTenChucVu(maCV);

    private List<ChiTietBangLuong> chiTietCua(BangLuong bl) =>
        chiTietBangLuongDAO.GetByBangLuong(bl.getMaBangLuong());

    private List<BangLuong> bangLuongTheoKy(int thang, int nam) =>
        bangLuongDAO.GetByKy(thang, nam);

    private static void tieuDe(string s)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 78));
        Console.WriteLine(s);
        Console.WriteLine(new string('=', 78));
    }

    // =========================================================================
    // CÂU 21: In phiếu lương cá nhân (Payslip)
    // =========================================================================
    public void inPhieuLuong(int maNV, int thang, int nam)
    {
        var nv = timNhanVien(maNV);
        if (nv == null)
        {
            Console.WriteLine("Không tìm thấy nhân viên có mã " + maNV);
            return;
        }

        var bl = bangLuongDAO.GetByNhanVienVaKy(maNV, thang, nam);
        if (bl == null)
        {
            Console.WriteLine($"Nhân viên {hoTen(nv)} chưa có bảng lương kỳ {thang:D2}/{nam}.");
            return;
        }

        var ct = chiTietCua(bl);
        tieuDe($"PHIẾU LƯƠNG THÁNG {thang:D2}/{nam}");
        Console.WriteLine($"Nhân viên : {hoTen(nv)} (Mã {nv.getMaNhanVien()})");
        Console.WriteLine($"Phòng ban : {tenPhongBan(nv.getMaPhongBan())}");
        Console.WriteLine($"Chức vụ   : {tenChucVu(nv.getMaChucVu())}");
        Console.WriteLine(LINE);

        if (ct.Count == 0)
        {
            Console.WriteLine("(Bảng lương này chưa có dòng chi tiết hạch toán)");
        }
        else
        {
            double luongCoBan = 0;
            var khoanCong = new List<ChiTietBangLuong>();
            var khoanTru = new List<ChiTietBangLuong>();

            foreach (var c in ct)
            {
                if (c.getLoaiKhoanMuc() == LoaiKhoanMuc.TRU)
                {
                    khoanTru.Add(c);
                }
                else if (chuanHoa(c.getTenKhoanMuc()).Contains("luong co ban"))
                {
                    luongCoBan += c.getSoTien();
                }
                else
                {
                    khoanCong.Add(c);
                }
            }

            Console.WriteLine($"{"Lương cơ bản",-40} {tien(luongCoBan),20}");
            Console.WriteLine("\nCác khoản cộng (phụ cấp, thưởng, OT):");
            if (khoanCong.Count == 0) Console.WriteLine("  (không có)");
            foreach (var c in khoanCong)
            {
                Console.WriteLine($"  + {c.getTenKhoanMuc(),-37} {tien(c.getSoTien()),20}   {(string.IsNullOrEmpty(c.getGhiChu()) ? "" : "(" + c.getGhiChu() + ")")}");
            }

            Console.WriteLine("\nCác khoản trừ (BHXH, thuế, phạt trễ):");
            if (khoanTru.Count == 0) Console.WriteLine("  (không có)");
            foreach (var c in khoanTru)
            {
                Console.WriteLine($"  - {c.getTenKhoanMuc(),-37} {tien(c.getSoTien()),20}   {(string.IsNullOrEmpty(c.getGhiChu()) ? "" : "(" + c.getGhiChu() + ")")}");
            }
        }

        Console.WriteLine(LINE);
        Console.WriteLine($"{"Tổng thu nhập",-40} {tien(bl.getTongThuNhap()),20}");
        Console.WriteLine($"{"Tổng khấu trừ",-40} {tien(bl.getTongKhauTru()),20}");
        Console.WriteLine($"{"THỰC LÃNH",-40} {tien(bl.getThucLanh()),20} VND");
        Console.WriteLine($"Trạng thái chi trả: {bl.getTrangThaiChiTra()}{(bl.getNgayChiTra().HasValue ? $" (ngày {bl.getNgayChiTra()})" : "")}");

        if (ct.Count > 0)
        {
            double cong = ct.Where(c => c.getLoaiKhoanMuc() == LoaiKhoanMuc.CONG).Sum(c => c.getSoTien());
            double tru = ct.Where(c => c.getLoaiKhoanMuc() == LoaiKhoanMuc.TRU).Sum(c => c.getSoTien());
            if (Math.Abs(cong - bl.getTongThuNhap()) > 1 || Math.Abs(tru - bl.getTongKhauTru()) > 1)
            {
                Console.WriteLine("[Cảnh báo] Tổng các dòng chi tiết không khớp với tổng thu nhập/khấu trừ đã lưu.");
            }
        }
    }

    // =========================================================================
    // CÂU 22: Bảng tổng hợp chi phí lương tháng
    // =========================================================================
    public void bangTongHopLuongThang(int thang, int nam)
    {
        tieuDe($"BẢNG TỔNG HỢP LƯƠNG THÁNG {thang:D2}/{nam}");
        var ds = bangLuongTheoKy(thang, nam);
        if (ds.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào trong kỳ này.");
            return;
        }

        Console.WriteLine($"{"Mã NV",-6} {"Tên NV",-22} {"Tổng thu nhập",15} {"Tổng khấu trừ",15} {"Thực lãnh",15}");
        Console.WriteLine(LINE);
        double tThuNhap = 0, tKhauTru = 0, tThucLanh = 0;
        foreach (var bl in ds)
        {
            Console.WriteLine($"{bl.getMaNhanVien(),-6} {tenNV(bl.getMaNhanVien()),-22} {tien(bl.getTongThuNhap()),15} {tien(bl.getTongKhauTru()),15} {tien(bl.getThucLanh()),15}");
            tThuNhap += bl.getTongThuNhap();
            tKhauTru += bl.getTongKhauTru();
            tThucLanh += bl.getThucLanh();
        }
        Console.WriteLine(LINE);
        Console.WriteLine($"{$"TỔNG CỘNG ({ds.Count} nhân viên)",-29} {tien(tThuNhap),15} {tien(tKhauTru),15} {tien(tThucLanh),15}");
    }

    // =========================================================================
    // CÂU 23: Thống kê quỹ lương theo phòng ban
    // =========================================================================
    public void quyLuongTheoPhongBan(int thang, int nam)
    {
        tieuDe($"QUỸ LƯƠNG THỰC LÃNH THEO PHÒNG BAN - THÁNG {thang:D2}/{nam}");
        var ds = bangLuongTheoKy(thang, nam);
        if (ds.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào trong kỳ này.");
            return;
        }

        var quy = new Dictionary<int, double>();
        var soNV = new Dictionary<int, int>();
        foreach (var pb in phongBanDAO.GetAll())
        {
            int maPB = pb.getmaPhongBan() ?? 0; quy[maPB] = 0.0;
            soNV[maPB] = 0;
        }

        foreach (var bl in ds)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            if (nv == null) continue;
            int pb = nv.getMaPhongBan();
            quy[pb] = quy.GetValueOrDefault(pb, 0.0) + bl.getThucLanh();
            soNV[pb] = soNV.GetValueOrDefault(pb, 0) + 1;
        }

        Console.WriteLine($"{"Mã PB",-6} {"Phòng ban",-24} {"Số NV",10} {"Quỹ lương thực lãnh",22}");
        Console.WriteLine(LINE);
        double tong = 0;
        foreach (var (k, v) in quy)
        {
            Console.WriteLine($"{k,-6} {tenPhongBan(k),-24} {soNV.GetValueOrDefault(k, 0),10} {tien(v),22}");
            tong += v;
        }
        Console.WriteLine(LINE);
        Console.WriteLine($"{"TỔNG CÔNG TY",-31} {"",10} {tien(tong),22}");
    }

    // =========================================================================
    // CÂU 24: Top 3 thu nhập thực lãnh cao nhất
    // =========================================================================
    public void topThuNhapCaoNhat(int thang, int nam)
    {
        tieuDe($"TOP 3 THU NHẬP THỰC LÃNH CAO NHẤT - THÁNG {thang:D2}/{nam}");
        var top = bangLuongDAO.GetTopThuNhap(thang, nam, 3);
        if (top.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào trong kỳ này.");
            return;
        }

        Console.WriteLine($"{"Hạng",-5} {"Mã NV",-6} {"Tên NV",-22} {"Chức vụ",-24} {"Thực lãnh",15}");
        Console.WriteLine(LINE);
        int hang = 1;
        foreach (var bl in top)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            Console.WriteLine($"{hang++,-5} {bl.getMaNhanVien(),-6} {tenNV(bl.getMaNhanVien()),-22} {(nv == null ? "" : tenChucVu(nv.getMaChucVu())),-24} {tien(bl.getThucLanh()),15}");
        }
    }

    // =========================================================================
    // CÂU 25: Chi tiết khấu trừ bảo hiểm
    // =========================================================================
    public void thongKeBaoHiem(int thang, int nam)
    {
        tieuDe($"THỐNG KÊ BẢO HIỂM BẮT BUỘC ĐÃ TRÍCH - THÁNG {thang:D2}/{nam}");
        var ds = bangLuongTheoKy(thang, nam);
        if (ds.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào trong kỳ này.");
            return;
        }

        Console.WriteLine($"{"Mã NV",-6} {"Tên NV",-22} {"BHXH",12} {"BHYT",12} {"BHTN",12} {"Tổng",13}");
        Console.WriteLine(LINE);
        double sXH = 0, sYT = 0, sTN = 0;
        foreach (var bl in ds)
        {
            double xh = 0, yt = 0, tn = 0;
            foreach (var c in chiTietCua(bl))
            {
                if (c.getLoaiKhoanMuc() != LoaiKhoanMuc.TRU) continue;
                string ten = chuanHoa(c.getTenKhoanMuc());
                if (!ten.Contains("bao hiem")) continue;
                if (ten.Contains("that nghiep")) tn += c.getSoTien();
                else if (ten.Contains("y te")) yt += c.getSoTien();
                else xh += c.getSoTien();
            }
            Console.WriteLine($"{bl.getMaNhanVien(),-6} {tenNV(bl.getMaNhanVien()),-22} {tien(xh),12} {tien(yt),12} {tien(tn),12} {tien(xh + yt + tn),13}");
            sXH += xh; sYT += yt; sTN += tn;
        }
        Console.WriteLine(LINE);
        Console.WriteLine($"{"TỔNG CỘNG",-29} {tien(sXH),12} {tien(sYT),12} {tien(sTN),12} {tien(sXH + sYT + sTN),13}");
    }

    // =========================================================================
    // CÂU 26: Báo cáo thưởng và phụ cấp
    // =========================================================================
    public void baoCaoThuongPhuCap(int thang, int nam)
    {
        tieuDe($"BÁO CÁO THƯỞNG & PHỤ CẤP - THÁNG {thang:D2}/{nam}");
        var ds = bangLuongTheoKy(thang, nam);
        if (ds.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào trong kỳ này.");
            return;
        }

        Console.WriteLine($"{"Mã NV",-6} {"Tên NV",-20} {"Khoản mục",-26} {"Số tiền",13}  {"Lý do chi"}");
        Console.WriteLine(LINE);
        double tong = 0;
        int dem = 0;
        foreach (var bl in ds)
        {
            foreach (var c in chiTietCua(bl))
            {
                if (c.getLoaiKhoanMuc() != LoaiKhoanMuc.CONG) continue;
                string ten = chuanHoa(c.getTenKhoanMuc());
                if (!ten.Contains("phu cap") && !ten.Contains("thuong")) continue;
                Console.WriteLine($"{bl.getMaNhanVien(),-6} {tenNV(bl.getMaNhanVien()),-20} {c.getTenKhoanMuc(),-26} {tien(c.getSoTien()),13}  {(string.IsNullOrEmpty(c.getGhiChu()) ? "" : c.getGhiChu())}");
                tong += c.getSoTien();
                dem++;
            }
        }
        if (dem == 0)
        {
            Console.WriteLine("Không có khoản thưởng/phụ cấp nào trong kỳ này.");
            return;
        }
        Console.WriteLine(LINE);
        Console.WriteLine($"TỔNG {dem} khoản: {tien(tong)} VND");
    }

    // =========================================================================
    // CÂU 27: Bản in phiếu KPI chi tiết
    // =========================================================================
    public void inPhieuKPI(int maDanhGia)
    {
        var dg = danhGiaHieuSuatDAO.GetById(maDanhGia);
        if (dg == null)
        {
            Console.WriteLine("Không tìm thấy phiếu đánh giá có mã " + maDanhGia);
            return;
        }
        var nv = timNhanVien(dg.getMaNhanVien());

        tieuDe("PHIẾU ĐÁNH GIÁ HIỆU SUẤT (KPI) - Mã " + dg.getMaDanhGia());
        Console.WriteLine($"Nhân viên được đánh giá: {tenNV(dg.getMaNhanVien())} (Mã {dg.getMaNhanVien()})");
        if (nv != null)
        {
            Console.WriteLine($"Phòng ban / Chức vụ    : {tenPhongBan(nv.getMaPhongBan())} / {tenChucVu(nv.getMaChucVu())}");
        }
        Console.WriteLine($"Người đánh giá         : {tenNV(dg.getNguoiDanhGia())} (Mã {dg.getNguoiDanhGia()})");
        Console.WriteLine($"Kỳ đánh giá            : {dg.getKyDanhGia()} / {dg.getNam()}");
        Console.WriteLine($"Điểm tổng              : {dg.getDiemTrungBinh():F2}");
        Console.WriteLine($"Xếp loại               : {dg.getXepLoai()}");
        Console.WriteLine(LINE);

        var ct = chiTietDanhGiaDAO.GetByDanhGia(maDanhGia);
        if (ct.Count == 0)
        {
            Console.WriteLine("(Phiếu này chưa có chi tiết từng tiêu chí)");
            return;
        }

        Console.WriteLine($"{"Tiêu chí",-30} {"Trọng số",9} {"Điểm",8}  {"Nhận xét"}");
        Console.WriteLine(LINE);
        foreach (var c in ct)
        {
            Console.WriteLine($"{c.getTieuChiDanhGia(),-30} {c.getTrongSo(),9:F2} {c.getDiemSo(),8:F2}  {(string.IsNullOrEmpty(c.getNhanXetChiTiet()) ? "" : c.getNhanXetChiTiet())}");
        }
    }

    // =========================================================================
    // CÂU 28: Xếp hạng hiệu suất theo xếp loại
    // =========================================================================
    public void xepHangHieuSuat(string kyDanhGia, int nam)
    {
        tieuDe($"XẾP HẠNG HIỆU SUẤT - {kyDanhGia} / {nam}");
        string kyChuan = chuanHoa(kyDanhGia).Trim();
        var ds = danhGiaHieuSuatDAO.GetByNam(nam)
            .Where(d => chuanHoa(d.getKyDanhGia()).Trim().Equals(kyChuan))
            .ToList();

        if (ds.Count == 0)
        {
            Console.WriteLine("Chưa có đánh giá nào trong kỳ này.");
            return;
        }

        foreach (XepLoai xl in Enum.GetValues<XepLoai>())
        {
            var nhom = ds.Where(d => d.getXepLoai() == xl)
                .OrderByDescending(d => d.getDiemTrungBinh())
                .ToList();

            Console.WriteLine($"\n>> {xl} ({nhom.Count} nhân viên)");
            if (nhom.Count == 0)
            {
                Console.WriteLine("   (không có)");
                continue;
            }
            foreach (var d in nhom)
            {
                Console.WriteLine($"   {d.getMaNhanVien(),-6} {tenNV(d.getMaNhanVien()),-22} {tenChucVuCuaNV(d.getMaNhanVien()),-24} {d.getDiemTrungBinh(),6:F2}");
            }
        }
    }

    private string tenChucVuCuaNV(int maNV)
    {
        var nv = timNhanVien(maNV);
        return nv == null ? "" : tenChucVu(nv.getMaChucVu());
    }

    // =========================================================================
    // CÂU 29: Nhân sự xuất sắc & không nghỉ ngày nào
    // =========================================================================
    public void timNhanSuXuatSac(int quy, int nam)
    {
        tieuDe($"NHÂN SỰ XUẤT SẮC & KHÔNG NGHỈ NGÀY NÀO - QUÝ {quy}/{nam}");
        if (quy < 1 || quy > 4)
        {
            Console.WriteLine("Quý phải từ 1 đến 4.");
            return;
        }
        var dau = new DateOnly(nam, (quy - 1) * 3 + 1, 1);
        var cuoi = dau.AddMonths(3).AddDays(-1);
        string kyChuan = chuanHoa("Quy " + quy);

        var xuatSac = danhGiaHieuSuatDAO.GetByNam(nam)
            .Where(d => chuanHoa(d.getKyDanhGia()).Trim().Equals(kyChuan) && d.getXepLoai() == XepLoai.XUAT_SAC)
            .ToList();

        int dem = 0;
        foreach (var d in xuatSac)
        {
            int maNV = d.getMaNhanVien();
            bool coNghiPhep = donXinNghiDAO.GetByNhanVien(maNV).Any(n =>
                n.getTrangThaiDuyet() == TrangThaiDuyet.DA_DUYET
                && n.getNgayBatDau() <= cuoi && n.getNgayKetThuc() >= dau);

            bool coVang = chamCongDAO.GetAll().Any(c =>
                c.getMaNhanVien() == maNV
                && c.getNgayLamViec() >= dau && c.getNgayLamViec() <= cuoi
                && (c.getTrangThaiCong() == TrangThaiCong.VANG_MAT_CO_PHEP || c.getTrangThaiCong() == TrangThaiCong.VANG_MAT_KHONG_PHEP));

            if (coNghiPhep || coVang) continue;
            if (dem == 0)
            {
                Console.WriteLine($"{"Mã NV",-6} {"Tên NV",-22} {"Chức vụ",-24} {"Điểm",8}");
                Console.WriteLine(LINE);
            }
            Console.WriteLine($"{maNV,-6} {tenNV(maNV),-22} {tenChucVuCuaNV(maNV),-24} {d.getDiemTrungBinh(),8:F2}");
            dem++;
        }
        if (dem == 0) Console.WriteLine("Không có nhân viên nào thỏa cả hai điều kiện.");
        else Console.WriteLine($"\nTìm thấy {dem} nhân viên.");
    }

    // =========================================================================
    // CÂU 30: Thu nhập trung bình theo từng chức vụ
    // =========================================================================
    public void thuNhapTrungBinhTheoChucVu()
    {
        tieuDe("THU NHẬP TRUNG BÌNH THEO CHỨC VỤ (toàn bộ bảng lương)");
        var tatCaBangLuong = bangLuongDAO.GetAll();
        if (tatCaBangLuong.Count == 0)
        {
            Console.WriteLine("Chưa có bảng lương nào.");
            return;
        }

        var theoChucVu = new Dictionary<int, List<BangLuong>>();
        foreach (var bl in tatCaBangLuong)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            if (nv == null) continue;
            if (!theoChucVu.ContainsKey(nv.getMaChucVu()))
            {
                theoChucVu[nv.getMaChucVu()] = new List<BangLuong>();
            }
            theoChucVu[nv.getMaChucVu()].Add(bl);
        }

        var ds = theoChucVu.ToList();
        ds.Sort((a, b) => tb(b.Value).CompareTo(tb(a.Value)));

        Console.WriteLine($"{"Chức vụ",-26} {"Số bảng lương",10} {"TB thu nhập",16}");
        Console.WriteLine(LINE);
        foreach (var e in ds)
        {
            Console.WriteLine($"{tenChucVu(e.Key),-26} {e.Value.Count,10} {tien(tb(e.Value)),16}");
        }
    }

    private static double tb(List<BangLuong> ds) =>
        ds.Count == 0 ? 0 : ds.Average(bl => bl.getTongThuNhap());
}
