using System.Globalization;
using System.Text;
using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.services;

public class TaiChinhService
{
    private static readonly CultureInfo VI_CULTURE = CultureInfo.GetCultureInfo("vi-VN");
    private static readonly string LINE = new string('-', 78);

    private static string tien(double v)
    {
        return Math.Round(v).ToString("N0", VI_CULTURE);
    }

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

    private static NhanVien? timNhanVien(int maNV)
    {
        return DatabaseMock.dsNhanVien.FirstOrDefault(nv => nv.getMaNhanVien() == maNV);
    }

    private static string tenNV(int maNV)
    {
        var nv = timNhanVien(maNV);
        return nv == null ? "(không rõ #" + maNV + ")" : hoTen(nv);
    }

    private static string tenPhongBan(int? maPB)
    {
        var pb = DatabaseMock.dsPhongBan.FirstOrDefault(p => p.getmaPhongBan().HasValue && p.getmaPhongBan() == maPB);
        return pb?.gettenPhongBan() ?? "(không rõ)";
    }

    private static string tenChucVu(int? maCV)
    {
        var cv = DatabaseMock.dsChucVu.FirstOrDefault(c => c.getmaChucVu().HasValue && c.getmaChucVu() == maCV);
        return cv?.gettenChucVu() ?? "(không rõ)";
    }

    private static string tenChucVuCuaNV(int maNV)
    {
        var nv = timNhanVien(maNV);
        return nv == null ? "" : tenChucVu(nv.getMaChucVu());
    }

    private static List<ChiTietBangLuong> chiTietCua(BangLuong bl)
    {
        return DatabaseMock.dsChiTietBangLuong.Where(ct => ct.getMaBangLuong() == bl.getMaBangLuong()).ToList();
    }

    private static List<BangLuong> bangLuongTheoKy(int thang, int nam)
    {
        return DatabaseMock.dsBangLuong.Where(bl => bl.getKyThang() == thang && bl.getKyNam() == nam).ToList();
    }

    private static void tieuDe(string s)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 78));
        Console.WriteLine(s);
        Console.WriteLine(new string('=', 78));
    }

    private static bool khongCoDuLieu<T>(ICollection<T> c, string thongBao)
    {
        if (c == null || c.Count == 0)
        {
            Console.WriteLine(thongBao);
            return true;
        }
        return false;
    }

    // =============================================
    // CÂU 21 (Câu 1 Hảo): In phiếu lương cá nhân (Payslip)
    // =============================================
    public void inPhieuLuong(int maNV, int thang, int nam)
    {
        var nv = timNhanVien(maNV);
        if (nv == null)
        {
            Console.WriteLine("Không tìm thấy nhân viên có mã " + maNV);
            return;
        }

        var bl = DatabaseMock.dsBangLuong.FirstOrDefault(b => b.getMaNhanVien() == maNV && b.getKyThang() == thang && b.getKyNam() == nam);
        if (bl == null)
        {
            Console.WriteLine(string.Format("Nhân viên {0} chưa có bảng lương kỳ {1:D2}/{2}.", hoTen(nv), thang, nam));
            return;
        }

        var ct = chiTietCua(bl);
        tieuDe(string.Format("PHIẾU LƯƠNG THÁNG {0:D2}/{1}", thang, nam));
        Console.WriteLine(string.Format("Nhân viên : {0} (Mã {1})", hoTen(nv), nv.getMaNhanVien()));
        Console.WriteLine(string.Format("Phòng ban : {0}", tenPhongBan(nv.getMaPhongBan())));
        Console.WriteLine(string.Format("Chức vụ   : {0}", tenChucVu(nv.getMaChucVu())));
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

            Console.WriteLine(string.Format("{0,-40} {1,20}", "Lương cơ bản", tien(luongCoBan)));
            Console.WriteLine("\nCác khoản cộng (phụ cấp, thưởng, OT):");
            if (khoanCong.Count == 0) Console.WriteLine("  (không có)");
            foreach (var c in khoanCong)
            {
                string ghiChuStr = string.IsNullOrEmpty(c.getGhiChu()) ? "" : "(" + c.getGhiChu() + ")";
                Console.WriteLine(string.Format("  + {0,-37} {1,20}   {2}", c.getTenKhoanMuc(), tien(c.getSoTien()), ghiChuStr));
            }

            Console.WriteLine("\nCác khoản trừ (BHXH, thuế, phạt trễ):");
            if (khoanTru.Count == 0) Console.WriteLine("  (không có)");
            foreach (var c in khoanTru)
            {
                string ghiChuStr = string.IsNullOrEmpty(c.getGhiChu()) ? "" : "(" + c.getGhiChu() + ")";
                Console.WriteLine(string.Format("  - {0,-37} {1,20}   {2}", c.getTenKhoanMuc(), tien(c.getSoTien()), ghiChuStr));
            }
        }

        Console.WriteLine(LINE);
        Console.WriteLine(string.Format("{0,-40} {1,20}", "Tổng thu nhập", tien(bl.getTongThuNhap())));
        Console.WriteLine(string.Format("{0,-40} {1,20}", "Tổng khấu trừ", tien(bl.getTongKhauTru())));
        Console.WriteLine(string.Format("{0,-40} {1,20} VND", "THỰC LÃNH", tien(bl.getThucLanh())));
        string ngayChiTraStr = bl.getNgayChiTra().HasValue ? $" (ngày {bl.getNgayChiTra()})" : "";
        Console.WriteLine(string.Format("Trạng thái chi trả: {0}{1}", bl.getTrangThaiChiTra(), ngayChiTraStr));

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

    // =============================================
    // CÂU 22 (Câu 2 Hảo): Bảng tổng hợp chi phí lương tháng
    // =============================================
    public void bangTongHopLuongThang(int thang, int nam)
    {
        tieuDe(string.Format("BẢNG TỔNG HỢP LƯƠNG THÁNG {0:D2}/{1}", thang, nam));
        var ds = bangLuongTheoKy(thang, nam);
        if (khongCoDuLieu(ds, "Chưa có bảng lương nào trong kỳ này.")) return;

        Console.WriteLine(string.Format("{0,-6} {1,-22} {2,15} {3,15} {4,15}", "Mã NV", "Tên NV", "Tổng thu nhập", "Tổng khấu trừ", "Thực lãnh"));
        Console.WriteLine(LINE);
        double tThuNhap = 0, tKhauTru = 0, tThucLanh = 0;
        foreach (var bl in ds)
        {
            Console.WriteLine(string.Format("{0,-6} {1,-22} {2,15} {3,15} {4,15}",
                bl.getMaNhanVien(), tenNV(bl.getMaNhanVien()),
                tien(bl.getTongThuNhap()), tien(bl.getTongKhauTru()), tien(bl.getThucLanh())));
            tThuNhap += bl.getTongThuNhap();
            tKhauTru += bl.getTongKhauTru();
            tThucLanh += bl.getThucLanh();
        }
        Console.WriteLine(LINE);
        Console.WriteLine(string.Format("{0,-29} {1,15} {2,15} {3,15}",
            "TỔNG CỘNG (" + ds.Count + " nhân viên)", tien(tThuNhap), tien(tKhauTru), tien(tThucLanh)));
    }

    // =============================================
    // CÂU 23 (Câu 3 Hảo): Thống kê quỹ lương theo phòng ban
    // =============================================
    public void quyLuongTheoPhongBan(int thang, int nam)
    {
        tieuDe(string.Format("QUỸ LƯƠNG THỰC LÃNH THEO PHÒNG BAN - THÁNG {0:D2}/{1}", thang, nam));
        var ds = bangLuongTheoKy(thang, nam);
        if (khongCoDuLieu(ds, "Chưa có bảng lương nào trong kỳ này.")) return;

        var quy = new Dictionary<int, double>();
        var soNV = new Dictionary<int, int>();
        foreach (var pb in DatabaseMock.dsPhongBan)
        {
            if (pb.getmaPhongBan().HasValue) quy[pb.getmaPhongBan().Value] = 0.0;
        }

        foreach (var bl in ds)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            if (nv == null) continue;
            int pbId = nv.getMaPhongBan();
            quy[pbId] = quy.GetValueOrDefault(pbId, 0.0) + bl.getThucLanh();
            soNV[pbId] = soNV.GetValueOrDefault(pbId, 0) + 1;
        }

        Console.WriteLine(string.Format("{0,-6} {1,-24} {2,10} {3,22}", "Mã PB", "Phòng ban", "Số NV", "Quỹ lương thực lãnh"));
        Console.WriteLine(LINE);
        double tong = 0;
        foreach (var kv in quy)
        {
            Console.WriteLine(string.Format("{0,-6} {1,-24} {2,10} {3,22}",
                kv.Key, tenPhongBan(kv.Key), soNV.GetValueOrDefault(kv.Key, 0), tien(kv.Value)));
            tong += kv.Value;
        }
        Console.WriteLine(LINE);
        Console.WriteLine(string.Format("{0,-31} {1,10} {2,22}", "TỔNG CÔNG TY", "", tien(tong)));
    }

    // =============================================
    // CÂU 24 (Câu 4 Hảo): Top 3 thu nhập thực lãnh cao nhất
    // =============================================
    public void topThuNhapCaoNhat(int thang, int nam)
    {
        tieuDe(string.Format("TOP 3 THU NHẬP THỰC LÃNH CAO NHẤT - THÁNG {0:D2}/{1}", thang, nam));
        var ds = bangLuongTheoKy(thang, nam);
        if (khongCoDuLieu(ds, "Chưa có bảng lương nào trong kỳ này.")) return;

        var top = ds.OrderByDescending(b => b.getThucLanh()).Take(3).ToList();

        Console.WriteLine(string.Format("{0,-5} {1,-6} {2,-22} {3,-24} {4,15}", "Hạng", "Mã NV", "Tên NV", "Chức vụ", "Thực lãnh"));
        Console.WriteLine(LINE);
        int hang = 1;
        foreach (var bl in top)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            Console.WriteLine(string.Format("{0,-5} {1,-6} {2,-22} {3,-24} {4,15}",
                hang++, bl.getMaNhanVien(), tenNV(bl.getMaNhanVien()),
                nv == null ? "" : tenChucVu(nv.getMaChucVu()), tien(bl.getThucLanh())));
        }
    }

    // =============================================
    // CÂU 25 (Câu 5 Hảo): Chi tiết khấu trừ bảo hiểm (BHXH, BHYT, BHTN)
    // =============================================
    public void thongKeBaoHiem(int thang, int nam)
    {
        tieuDe(string.Format("THỐNG KÊ BẢO HIỂM BẮT BUỘC ĐÃ TRÍCH - THÁNG {0:D2}/{1}", thang, nam));
        var ds = bangLuongTheoKy(thang, nam);
        if (khongCoDuLieu(ds, "Chưa có bảng lương nào trong kỳ này.")) return;

        Console.WriteLine(string.Format("{0,-6} {1,-22} {2,12} {3,12} {4,12} {5,13}", "Mã NV", "Tên NV", "BHXH", "BHYT", "BHTN", "Tổng"));
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
            Console.WriteLine(string.Format("{0,-6} {1,-22} {2,12} {3,12} {4,12} {5,13}",
                bl.getMaNhanVien(), tenNV(bl.getMaNhanVien()), tien(xh), tien(yt), tien(tn), tien(xh + yt + tn)));
            sXH += xh; sYT += yt; sTN += tn;
        }
        Console.WriteLine(LINE);
        Console.WriteLine(string.Format("{0,-29} {1,12} {2,12} {3,12} {4,13}", "TỔNG CỘNG", tien(sXH), tien(sYT), tien(sTN), tien(sXH + sYT + sTN)));
    }

    // =============================================
    // CÂU 26 (Câu 6 Hảo): Báo cáo thưởng và phụ cấp
    // =============================================
    public void baoCaoThuongPhuCap(int thang, int nam)
    {
        tieuDe(string.Format("BÁO CÁO THƯỞNG & PHỤ CẤP - THÁNG {0:D2}/{1}", thang, nam));
        var ds = bangLuongTheoKy(thang, nam);
        if (khongCoDuLieu(ds, "Chưa có bảng lương nào trong kỳ này.")) return;

        Console.WriteLine(string.Format("{0,-6} {1,-20} {2,-26} {3,13}  {4}", "Mã NV", "Tên NV", "Khoản mục", "Số tiền", "Lý do chi"));
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
                Console.WriteLine(string.Format("{0,-6} {1,-20} {2,-26} {3,13}  {4}",
                    bl.getMaNhanVien(), tenNV(bl.getMaNhanVien()), c.getTenKhoanMuc(), tien(c.getSoTien()), c.getGhiChu() ?? ""));
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
        Console.WriteLine(string.Format("TỔNG {0} khoản: {1} VND", dem, tien(tong)));
    }

    // =============================================
    // CÂU 27 (Câu 7 Hảo): Bản in phiếu đánh giá KPI chi tiết
    // =============================================
    public void inPhieuKPI(int maDanhGia)
    {
        var dg = DatabaseMock.dsDanhGiaHieuSuat.FirstOrDefault(d => d.getMaDanhGia() == maDanhGia);
        if (dg == null)
        {
            Console.WriteLine("Không tìm thấy phiếu đánh giá có mã " + maDanhGia);
            return;
        }
        var nv = timNhanVien(dg.getMaNhanVien());

        tieuDe("PHIẾU ĐÁNH GIÁ HIỆU SUẤT (KPI) - Mã " + dg.getMaDanhGia());
        Console.WriteLine(string.Format("Nhân viên được đánh giá: {0} (Mã {1})", tenNV(dg.getMaNhanVien()), dg.getMaNhanVien()));
        if (nv != null)
        {
            Console.WriteLine(string.Format("Phòng ban / Chức vụ    : {0} / {1}", tenPhongBan(nv.getMaPhongBan()), tenChucVu(nv.getMaChucVu())));
        }
        Console.WriteLine(string.Format("Người đánh giá         : {0} (Mã {1})", tenNV(dg.getNguoiDanhGia()), dg.getNguoiDanhGia()));
        Console.WriteLine(string.Format("Kỳ đánh giá            : {0} / {1}", dg.getKyDanhGia(), dg.getNam()));
        Console.WriteLine(string.Format("Điểm tổng              : {0:F2}", dg.getDiemTrungBinh()));
        Console.WriteLine(string.Format("Xếp loại               : {0}", dg.getXepLoai()));
        Console.WriteLine(LINE);

        var ct = DatabaseMock.dsChiTietDanhGia.Where(c => c.getMaDanhGia() == maDanhGia).ToList();
        if (khongCoDuLieu(ct, "(Phiếu này chưa có chi tiết từng tiêu chí)")) return;

        Console.WriteLine(string.Format("{0,-30} {1,9} {2,8}  {3}", "Tiêu chí", "Trọng số", "Điểm", "Nhận xét"));
        Console.WriteLine(LINE);
        foreach (var c in ct)
        {
            Console.WriteLine(string.Format("{0,-30} {1,9:F2} {2,8:F2}  {3}",
                c.getTieuChiDanhGia(), c.getTrongSo(), c.getDiemSo(), c.getNhanXetChiTiet() ?? ""));
        }
    }

    // =============================================
    // CÂU 28 (Câu 8 Hảo): Xếp hạng hiệu suất theo phân loại
    // =============================================
    public void xepHangHieuSuat(string kyDanhGia, int nam)
    {
        tieuDe(string.Format("XẾP HẠNG HIỆU SUẤT - {0} / {1}", kyDanhGia, nam));
        string kyChuan = chuanHoa(kyDanhGia).Trim();
        var ds = DatabaseMock.dsDanhGiaHieuSuat.Where(d => d.getNam() == nam && chuanHoa(d.getKyDanhGia()).Trim().Equals(kyChuan)).ToList();
        if (khongCoDuLieu(ds, "Chưa có đánh giá nào trong kỳ này.")) return;

        foreach (XepLoai xl in Enum.GetValues(typeof(XepLoai)))
        {
            var nhom = ds.Where(d => d.getXepLoai() == xl).OrderByDescending(d => d.getDiemTrungBinh()).ToList();
            Console.WriteLine(string.Format("\n>> {0} ({1} nhân viên)", xl, nhom.Count));
            if (nhom.Count == 0)
            {
                Console.WriteLine("   (không có)");
                continue;
            }
            foreach (var d in nhom)
            {
                Console.WriteLine(string.Format("   {0,-6} {1,-22} {2,-24} {3,6:F2}",
                    d.getMaNhanVien(), tenNV(d.getMaNhanVien()), tenChucVuCuaNV(d.getMaNhanVien()), d.getDiemTrungBinh()));
            }
        }
    }

    // =============================================
    // CÂU 29 (Câu 9 Hảo): Nhân sự xuất sắc & không nghỉ ngày nào
    // =============================================
    public void timNhanSuXuatSac(int quy, int nam)
    {
        tieuDe(string.Format("NHÂN SỰ XUẤT SẮC & KHÔNG NGHỈ NGÀY NÀO - QUÝ {0}/{1}", quy, nam));
        if (quy < 1 || quy > 4)
        {
            Console.WriteLine("Quý phải từ 1 đến 4.");
            return;
        }

        var dau = new DateOnly(nam, (quy - 1) * 3 + 1, 1);
        var cuoi = dau.AddMonths(3).AddDays(-1);
        string kyChuan = chuanHoa("Quy " + quy);

        var xuatSac = DatabaseMock.dsDanhGiaHieuSuat.Where(d => d.getNam() == nam && chuanHoa(d.getKyDanhGia()).Trim().Equals(kyChuan)
            && d.getXepLoai() == XepLoai.XUAT_SAC).ToList();

        int dem = 0;
        foreach (var d in xuatSac)
        {
            int maNV = d.getMaNhanVien();
            bool coNghiPhep = DatabaseMock.dsDonXinNghi.Any(n =>
                n.getMaNhanVien() == maNV && n.getTrangThaiDuyet() == TrangThaiDuyet.DA_DUYET
                && n.getNgayBatDau() <= cuoi && n.getNgayKetThuc() >= dau);

            bool coVang = DatabaseMock.dsChamCong.Any(c =>
                c.getMaNhanVien() == maNV && c.getNgayLamViec() >= dau && c.getNgayLamViec() <= cuoi
                && (c.getTrangThaiCong() == TrangThaiCong.VANG_MAT_CO_PHEP || c.getTrangThaiCong() == TrangThaiCong.VANG_MAT_KHONG_PHEP));

            if (coNghiPhep || coVang) continue;
            if (dem == 0)
            {
                Console.WriteLine(string.Format("{0,-6} {1,-22} {2,-24} {3,8}", "Mã NV", "Tên NV", "Chức vụ", "Điểm"));
                Console.WriteLine(LINE);
            }
            Console.WriteLine(string.Format("{0,-6} {1,-22} {2,-24} {3,8:F2}",
                maNV, tenNV(maNV), tenChucVuCuaNV(maNV), d.getDiemTrungBinh()));
            dem++;
        }
        if (dem == 0) Console.WriteLine("Không có nhân viên nào thỏa cả hai điều kiện.");
        else Console.WriteLine("\nTìm thấy " + dem + " nhân viên.");
    }

    // =============================================
    // CÂU 30 (Câu 10 Hảo): Thu nhập trung bình theo chức vụ
    // =============================================
    public void thuNhapTrungBinhTheoChucVu()
    {
        tieuDe("THU NHẬP TRUNG BÌNH THEO CHỨC VỤ (toàn bộ bảng lương)");
        if (khongCoDuLieu(DatabaseMock.dsBangLuong, "Chưa có bảng lương nào.")) return;

        var theoChucVu = new Dictionary<int, List<BangLuong>>();
        foreach (var bl in DatabaseMock.dsBangLuong)
        {
            var nv = timNhanVien(bl.getMaNhanVien());
            if (nv == null) continue;
            int cvId = nv.getMaChucVu();
            if (!theoChucVu.ContainsKey(cvId))
            {
                theoChucVu[cvId] = new List<BangLuong>();
            }
            theoChucVu[cvId].Add(bl);
        }

        var ds = theoChucVu.OrderByDescending(kv => tb(kv.Value)).ToList();

        Console.WriteLine(string.Format("{0,-26} {1,10} {2,16}", "Chức vụ", "Số bảng lương", "TB thu nhập"));
        Console.WriteLine(LINE);
        foreach (var e in ds)
        {
            Console.WriteLine(string.Format("{0,-26} {1,10} {2,16}", tenChucVu(e.Key), e.Value.Count, tien(tb(e.Value))));
        }
    }

    private static double tb(List<BangLuong> ds)
    {
        return ds.Count == 0 ? 0 : ds.Average(bl => bl.getTongThuNhap());
    }
}
