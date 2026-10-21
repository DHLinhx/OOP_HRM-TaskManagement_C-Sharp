using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.services;

public class VanHanhService
{
    // Helper: Tìm tên nhân viên theo mã (LINQ + Lambda)
    private string getTenNhanVien(int maNV)
    {
        var nv = DatabaseMock.dsNhanVien.FirstOrDefault(n => n.getMaNhanVien() == maNV);
        return nv != null ? nv.HoTenNV() : "Mã " + maNV;
    }

    // Helper: Tìm tên ca làm việc theo mã (LINQ + Lambda)
    private string getTenCa(int maCa)
    {
        var ca = DatabaseMock.dsCaLamViec.FirstOrDefault(c => c.getMaCa() == maCa);
        return ca != null ? ca.getTenCa() : "Ca " + maCa;
    }

    // Helper: Tìm số hợp đồng theo mã hợp đồng (LINQ + Lambda)
    private string getSoHopDong(int maHopDong)
    {
        var hd = DatabaseMock.dsHopDong.FirstOrDefault(h => h.getMaHopDong() == maHopDong);
        return hd != null ? hd.getSoHopDong() : "Mã HĐ: " + maHopDong;
    }

    // =========================================================================
    // CÂU 1: Bản in hợp đồng chi tiết
    // =========================================================================
    public void inHopDongChiTiet(int maHopDong)
    {
        var hd = DatabaseMock.dsHopDong.FirstOrDefault(h => h.getMaHopDong() == maHopDong);
        if (hd == null)
        {
            Console.WriteLine("Không tìm thấy hợp đồng có mã: " + maHopDong);
            return;
        }

        string tenNV = $"{getTenNhanVien(hd.getMaNhanVien())} (Mã {hd.getMaNhanVien()})";

        Console.WriteLine("==============================================================================");
        Console.WriteLine("CHI TIẾT HỢP ĐỒNG LAO ĐỘNG: " + hd.getSoHopDong());
        Console.WriteLine("==============================================================================");
        Console.WriteLine($"Nhân viên ký kết : {tenNV}");
        Console.WriteLine($"Loại hợp đồng    : {hd.getLoaiHopDong()}");
        Console.WriteLine($"Thời hạn         : {hd.getNgayHieuLuc()} -> {(hd.getNgayHetHan().HasValue ? hd.getNgayHetHan().Value.ToString() : "Vô thời hạn")}");
        Console.WriteLine(string.Format("Lương thỏa thuận : {0:N0} VNĐ", hd.getLuongThoaThuan()));
        Console.WriteLine($"Trạng thái       : {hd.getTrangThai()}");
        Console.WriteLine("------------------------------------------------------------------------------");
        Console.WriteLine("CÁC ĐIỀU KHOẢN ĐIỀU KIỆN KÈM THEO:");

        var dsDK = DatabaseMock.dsDieuKhoanHopDong.Where(dk => dk.getMaHopDong() == maHopDong).ToList();
        if (dsDK.Count == 0)
        {
            Console.WriteLine("  (Hợp đồng không có điều khoản phụ kèm theo)");
        }
        else
        {
            dsDK.ForEach(dk =>
            {
                Console.WriteLine(string.Format("  [Điều {0}] {1,-25} ({2})", dk.getSoThuTuDieu(), dk.getTieuDeDieuKhoan(), dk.getLoaiDieuKhoan()));
                Console.WriteLine($"          -> {dk.getNoiDungDieuKhoan()}");
            });
        }
    }

    // =========================================================================
    // CÂU 2: Cảnh báo hợp đồng sắp hết hạn trong vòng 30 ngày
    // =========================================================================
    public void canhBaoHopDongSapHetHan(DateOnly mocThoiGian)
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("CẢNH BÁO HỢP ĐỒNG SẮP HẾT HẠN (30 NGÀY TỪ " + mocThoiGian + ")");
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-14} {1,-8} {2,-22} {3,-18} {4}", "Số HĐ", "Mã NV", "Họ và tên", "Loại hợp đồng", "Ngày hết hạn"));
        Console.WriteLine("------------------------------------------------------------------------------");

        DateOnly moc30NgaySau = mocThoiGian.AddDays(30);

        var dsSapHetHan = DatabaseMock.dsHopDong
            .Where(hd => hd.getNgayHetHan().HasValue
                         && hd.getNgayHetHan().Value >= mocThoiGian
                         && hd.getNgayHetHan().Value <= moc30NgaySau)
            .ToList();

        if (dsSapHetHan.Count == 0)
        {
            Console.WriteLine("-> Không có hợp đồng nào sắp hết hạn trong 30 ngày tới.");
            return;
        }

        dsSapHetHan.ForEach(hd =>
        {
            string hoTen = getTenNhanVien(hd.getMaNhanVien());
            Console.WriteLine(string.Format("{0,-14} {1,-8} {2,-22} {3,-18} {4}",
                hd.getSoHopDong(), hd.getMaNhanVien(), hoTen, hd.getLoaiHopDong(), hd.getNgayHetHan()));
        });
    }

    // =========================================================================
    // CÂU 3: Bảng chấm công cá nhân theo tháng/năm
    // =========================================================================
    public void inBangChamCongCaNhan(int maNhanVien, int thang, int nam)
    {
        string hoTen = getTenNhanVien(maNhanVien);

        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("BẢNG CHẤM CÔNG NHÂN VIÊN: {0} (Mã {1}) - THÁNG {2:D2}/{3}", hoTen, maNhanVien, thang, nam));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-12} {1,-18} {2,-10} {3,-10} {4,-12} {5}", "Ngày", "Ca làm việc", "Giờ vào", "Giờ ra", "Số giờ làm", "Trạng thái"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var dsCong = DatabaseMock.dsChamCong
            .Where(cc => cc.getMaNhanVien() == maNhanVien
                         && cc.getNgayLamViec().Month == thang
                         && cc.getNgayLamViec().Year == nam)
            .ToList();

        if (dsCong.Count == 0)
        {
            Console.WriteLine("-> Không tìm thấy dữ liệu chấm công trong tháng này.");
            return;
        }

        dsCong.ForEach(cc =>
        {
            string tenCa = getTenCa(cc.getMaCa());
            double soGioLam = 0.0;
            if (cc.getGioVaoThucTe().HasValue && cc.getGioRaThucTe().HasValue)
            {
                var timeIn = cc.getGioVaoThucTe().Value.ToTimeSpan();
                var timeOut = cc.getGioRaThucTe().Value.ToTimeSpan();
                soGioLam = (timeOut - timeIn).TotalHours;
            }

            Console.WriteLine(string.Format("{0,-12} {1,-18} {2,-10} {3,-10} {4,-12} {5}",
                cc.getNgayLamViec(),
                tenCa,
                cc.getGioVaoThucTe(),
                cc.getGioRaThucTe(),
                string.Format("{0:F1} giờ", soGioLam),
                cc.getTrangThaiCong()));
        });
    }

    // =========================================================================
    // CÂU 4: Báo cáo đi trễ trong tháng (sắp xếp giảm dần theo số lần vi phạm)
    // =========================================================================
    public class ThongKeDiTre
    {
        public int maNV;
        public string hoTen;
        public int soLan;

        public ThongKeDiTre(int maNV, string hoTen, int soLan)
        {
            this.maNV = maNV;
            this.hoTen = hoTen;
            this.soLan = soLan;
        }
    }

    public void baocaoDiTre(int thang, int nam)
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("BÁO CÁO NHÂN VIÊN ĐI TRỄ - THÁNG {0:D2}/{1}", thang, nam));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3}", "Hạng", "Mã NV", "Họ và tên", "Số lần đi trễ"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var danhSach = DatabaseMock.dsNhanVien
            .Select(nv =>
            {
                int soLanTre = DatabaseMock.dsChamCong.Count(cc =>
                    cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam
                    && cc.getTrangThaiCong() == TrangThaiCong.DI_TRE);
                return new ThongKeDiTre(nv.getMaNhanVien(), nv.HoTenNV(), soLanTre);
            })
            .Where(tk => tk.soLan > 0)
            .OrderByDescending(tk => tk.soLan)
            .ToList();

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Tuyệt vời! Không có nhân viên nào đi trễ trong tháng này.");
            return;
        }

        int rank = 1;
        danhSach.ForEach(tk =>
            Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3} lần", rank++, tk.maNV, tk.hoTen, tk.soLan)));
    }

    // =========================================================================
    // CÂU 5: Báo cáo nhân viên làm thêm giờ (OT) trong tháng
    // =========================================================================
    public class ThongKeOT
    {
        public int maNV;
        public string hoTen;
        public double soGio;

        public ThongKeOT(int maNV, string hoTen, double soGio)
        {
            this.maNV = maNV;
            this.hoTen = hoTen;
            this.soGio = soGio;
        }
    }

    public void baocaoOT(int thang, int nam)
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("BÁO CÁO TOP NHÂN VIÊN LÀM THÊM GIỜ (OT) - THÁNG {0:D2}/{1}", thang, nam));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3}", "Hạng", "Mã NV", "Họ và tên", "Tổng số giờ OT"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var danhSach = DatabaseMock.dsNhanVien
            .Select(nv =>
            {
                double soGio = DatabaseMock.dsChamCong
                    .Where(cc => cc.getMaNhanVien() == nv.getMaNhanVien()
                                 && cc.getNgayLamViec().Month == thang
                                 && cc.getNgayLamViec().Year == nam)
                    .Sum(cc => cc.getSoGioLamThem());
                return new ThongKeOT(nv.getMaNhanVien(), nv.HoTenNV(), soGio);
            })
            .Where(tk => tk.soGio > 0)
            .OrderByDescending(tk => tk.soGio)
            .ToList();

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Không có nhân viên nào làm thêm giờ trong tháng này.");
            return;
        }

        int rank = 1;
        danhSach.ForEach(tk =>
            Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3:F1} giờ", rank++, tk.maNV, tk.hoTen, tk.soGio)));
    }

    // =========================================================================
    // CÂU 6: Bảng theo dõi đơn nghỉ phép đang CHỜ DUYỆT
    // =========================================================================
    public void theoDoiDonNghiPhep()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("DANH SÁCH ĐƠN NGHỈ PHÉP ĐANG CHỜ DUYỆT");
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-8} {1,-22} {2,-16} {3,-10} {4,-24} {5}", "Mã đơn", "Người nộp", "Loại nghỉ", "Số ngày", "Thời gian", "Người duyệt"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var dsChoDuyet = DatabaseMock.dsDonXinNghi
            .Where(don => don.getTrangThaiDuyet() == TrangThaiDuyet.CHO_DUYET)
            .ToList();

        if (dsChoDuyet.Count == 0)
        {
            Console.WriteLine("-> Hiện không có đơn xin nghỉ phép nào đang chờ duyệt.");
            return;
        }

        dsChoDuyet.ForEach(don =>
        {
            string tenNguoiNop = getTenNhanVien(don.getMaNhanVien());
            string tenNguoiDuyet = don.getNguoiDuyet().HasValue ? getTenNhanVien(don.getNguoiDuyet().Value) : "Chưa chỉ định";
            long soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
            string thoiGian = $"{don.getNgayBatDau()} -> {don.getNgayKetThuc()}";

            Console.WriteLine(string.Format("{0,-8} {1,-22} {2,-16} {3,-10} {4,-24} {5}",
                don.getMaDonNghi(), tenNguoiNop, don.getLoaiNghi(), soNgayNghi + " ngày", thoiGian, tenNguoiDuyet));
        });
    }

    // =========================================================================
    // CÂU 7: Lịch sử nghỉ phép của một nhân viên
    // =========================================================================
    public void lsNghi(int maNV)
    {
        bool timThayNV = DatabaseMock.dsNhanVien.Any(nv => nv.getMaNhanVien() == maNV);
        if (!timThayNV)
        {
            Console.WriteLine("Không tìm thấy nhân viên có mã: " + maNV);
            return;
        }

        string hoTen = getTenNhanVien(maNV);

        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("LỊCH SỬ NGHỈ PHÉP: {0} (Mã {1})", hoTen, maNV));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-8} {1,-16} {2,-10} {3,-25} {4,-14} {5}", "Mã đơn", "Loại nghỉ", "Số ngày", "Thời gian", "Trạng thái", "Lý do"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var dsNghi = DatabaseMock.dsDonXinNghi.Where(don => don.getMaNhanVien() == maNV).ToList();

        if (dsNghi.Count == 0)
        {
            Console.WriteLine("-> Nhân viên chưa có đơn xin nghỉ phép nào.");
            return;
        }

        dsNghi.ForEach(don =>
        {
            long soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
            string thoiGian = $"{don.getNgayBatDau()} -> {don.getNgayKetThuc()}";

            Console.WriteLine(string.Format("{0,-8} {1,-16} {2,-10} {3,-25} {4,-14} {5}",
                don.getMaDonNghi(), don.getLoaiNghi(), soNgayNghi + " ngày", thoiGian, don.getTrangThaiDuyet(), don.getLyDo()));
        });
    }

    // =========================================================================
    // CÂU 8: Thống kê nhân viên đạt chuyên cần 100% trong tháng
    // =========================================================================
    public void thongkeChuyenCan(int thang, int nam)
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("DANH SÁCH NHÂN VIÊN ĐẠT CHUYÊN CẦN 100% - THÁNG {0:D2}/{1}", thang, nam));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-8} {1,-25} {2,-18} {3}", "Mã NV", "Họ và tên", "Số ngày công", "Tỷ lệ đúng giờ"));
        Console.WriteLine("------------------------------------------------------------------------------");

        var dsChuyenCan = DatabaseMock.dsNhanVien
            .Where(nv =>
            {
                int tongCong = DatabaseMock.dsChamCong.Count(cc =>
                    cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam);
                int dungGio = DatabaseMock.dsChamCong.Count(cc =>
                    cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam
                    && cc.getTrangThaiCong() == TrangThaiCong.DUNG_GIO);
                return tongCong > 0 && tongCong == dungGio;
            })
            .ToList();

        if (dsChuyenCan.Count == 0)
        {
            Console.WriteLine("-> Không có nhân viên nào đạt chuyên cần 100% trong tháng.");
            return;
        }

        dsChuyenCan.ForEach(nv =>
        {
            int tongCong = DatabaseMock.dsChamCong.Count(cc =>
                cc.getMaNhanVien() == nv.getMaNhanVien()
                && cc.getNgayLamViec().Month == thang
                && cc.getNgayLamViec().Year == nam);
            Console.WriteLine(string.Format("{0,-8} {1,-25} {2,-18} {3}",
                nv.getMaNhanVien(),
                nv.HoTenNV(),
                tongCong + "/" + tongCong + " ngày",
                "100%"));
        });
    }

    // =========================================================================
    // CÂU 9: Thống kê số lượng nhân viên theo từng loại hợp đồng lao động
    // =========================================================================
    public void thongkeHopDong()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("THỐNG KÊ NHÂN SỰ THEO TỪNG LOẠI HỢP ĐỒNG LAO ĐỘNG");
        Console.WriteLine("==============================================================================");

        Enum.GetValues<LoaiHopDong>().ToList().ForEach(lhd =>
        {
            var dsHDTheoLoai = DatabaseMock.dsHopDong
                .Where(hd => hd.getLoaiHopDong() == lhd && hd.getTrangThai() != TrangThaiHopDong.DA_HET_HAN)
                .ToList();

            Console.WriteLine(string.Format("\n>> LOẠI HỢP ĐỒNG: {0} ({1} nhân viên)", lhd, dsHDTheoLoai.Count));
            if (dsHDTheoLoai.Count > 0)
            {
                Console.WriteLine(string.Format("   {0,-8} {1,-22} {2,-15} {3}", "Mã NV", "Họ và tên", "Số hợp đồng", "Trạng thái HĐ"));
                Console.WriteLine("   --------------------------------------------------------------------------");
                dsHDTheoLoai.ForEach(hd =>
                {
                    string hoTen = getTenNhanVien(hd.getMaNhanVien());
                    Console.WriteLine(string.Format("   {0,-8} {1,-22} {2,-15} {3}",
                        hd.getMaNhanVien(),
                        hoTen,
                        hd.getSoHopDong(),
                        hd.getTrangThai()));
                });
            }
        });
    }

    // =========================================================================
    // CÂU 10: Lọc các điều khoản pháp lý (Bảo mật hoặc Kỷ luật) trên hệ thống
    // =========================================================================
    public void locDieuKhoanPhapLy()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("DANH SÁCH ĐIỀU KHOẢN PHÁP LÝ (BẢO MẬT & KỶ LUẬT)");
        Console.WriteLine("==============================================================================");

        var dsDKPhapLy = DatabaseMock.dsDieuKhoanHopDong
            .Where(dk => !string.IsNullOrEmpty(dk.getLoaiDieuKhoan())
                         && (dk.getLoaiDieuKhoan().Equals("Bao mat", StringComparison.OrdinalIgnoreCase)
                             || dk.getLoaiDieuKhoan().Equals("Ky luat", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (dsDKPhapLy.Count == 0)
        {
            Console.WriteLine("-> Không tìm thấy điều khoản bảo mật hoặc kỷ luật nào.");
            return;
        }

        dsDKPhapLy.ForEach(dk =>
        {
            string soHD = getSoHopDong(dk.getMaHopDong());
            Console.WriteLine(string.Format("[{0}] - Điều {1} ({2}): {3}",
                dk.getLoaiDieuKhoan().ToUpper(),
                dk.getSoThuTuDieu(),
                soHD,
                dk.getTieuDeDieuKhoan()));
            Console.WriteLine(string.Format("   -> Nội dung: {0}\n", dk.getNoiDungDieuKhoan()));
        });
    }
}
