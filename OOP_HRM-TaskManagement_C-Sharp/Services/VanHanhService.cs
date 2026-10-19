using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.services;

public class VanHanhService
{
    // =========================================================================
    // CÂU 1: Bản in hợp đồng chi tiết
    // =========================================================================
    public void inHopDongChiTiet(int maHopDong)
    {
        foreach (var hd in DatabaseMock.dsHopDong)
        {
            if (hd.getMaHopDong() == maHopDong)
            {
                string tenNV = "Mã " + hd.getMaNhanVien();
                foreach (var nv in DatabaseMock.dsNhanVien)
                {
                    if (nv.getMaNhanVien() == hd.getMaNhanVien())
                    {
                        tenNV = nv.HoTenNV() + " (Mã " + nv.getMaNhanVien() + ")";
                        break;
                    }
                }

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

                bool coDieuKhoan = false;
                foreach (var dk in DatabaseMock.dsDieuKhoanHopDong)
                {
                    if (dk.getMaHopDong() == maHopDong)
                    {
                        coDieuKhoan = true;
                        Console.WriteLine(string.Format("  [Điều {0}] {1,-25} ({2})", dk.getSoThuTuDieu(), dk.getTieuDeDieuKhoan(), dk.getLoaiDieuKhoan()));
                        Console.WriteLine($"          -> {dk.getNoiDungDieuKhoan()}");
                    }
                }
                if (!coDieuKhoan)
                {
                    Console.WriteLine("  (Hợp đồng không có điều khoản phụ kèm theo)");
                }
                return;
            }
        }

        Console.WriteLine("Không tìm thấy hợp đồng có mã: " + maHopDong);
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
        bool timThay = false;

        foreach (var hd in DatabaseMock.dsHopDong)
        {
            if (hd.getNgayHetHan().HasValue)
            {
                DateOnly ngayHetHan = hd.getNgayHetHan().Value;
                if (ngayHetHan >= mocThoiGian && ngayHetHan <= moc30NgaySau)
                {
                    timThay = true;
                    string hoTen = "";
                    foreach (var nv in DatabaseMock.dsNhanVien)
                    {
                        if (nv.getMaNhanVien() == hd.getMaNhanVien())
                        {
                            hoTen = nv.HoTenNV();
                            break;
                        }
                    }
                    Console.WriteLine(string.Format("{0,-14} {1,-8} {2,-22} {3,-18} {4}",
                        hd.getSoHopDong(), hd.getMaNhanVien(), hoTen, hd.getLoaiHopDong(), ngayHetHan));
                }
            }
        }

        if (!timThay)
        {
            Console.WriteLine("-> Không có hợp đồng nào sắp hết hạn trong 30 ngày tới.");
        }
    }

    // =========================================================================
    // CÂU 3: Bảng chấm công cá nhân theo tháng/năm
    // =========================================================================
    public void inBangChamCongCaNhan(int maNhanVien, int thang, int nam)
    {
        string hoTen = "Mã " + maNhanVien;
        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            if (nv.getMaNhanVien() == maNhanVien)
            {
                hoTen = nv.HoTenNV();
                break;
            }
        }

        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("BẢNG CHẤM CÔNG NHÂN VIÊN: {0} (Mã {1}) - THÁNG {2:D2}/{3}", hoTen, maNhanVien, thang, nam));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-12} {1,-18} {2,-10} {3,-10} {4,-12} {5}", "Ngày", "Ca làm việc", "Giờ vào", "Giờ ra", "Số giờ làm", "Trạng thái"));
        Console.WriteLine("------------------------------------------------------------------------------");

        bool timThay = false;
        foreach (var cc in DatabaseMock.dsChamCong)
        {
            if (cc.getMaNhanVien() == maNhanVien
                && cc.getNgayLamViec().Month == thang
                && cc.getNgayLamViec().Year == nam)
            {
                timThay = true;
                string tenCa = "Ca " + cc.getMaCa();
                foreach (var ca in DatabaseMock.dsCaLamViec)
                {
                    if (ca.getMaCa() == cc.getMaCa())
                    {
                        tenCa = ca.getTenCa();
                        break;
                    }
                }

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
            }
        }

        if (!timThay)
        {
            Console.WriteLine("-> Không tìm thấy dữ liệu chấm công trong tháng này.");
        }
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

        List<ThongKeDiTre> danhSach = new();
        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            int soLanTre = 0;
            foreach (var cc in DatabaseMock.dsChamCong)
            {
                if (cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam
                    && cc.getTrangThaiCong() == TrangThaiCong.DI_TRE)
                {
                    soLanTre++;
                }
            }
            if (soLanTre > 0)
            {
                danhSach.Add(new ThongKeDiTre(nv.getMaNhanVien(), nv.HoTenNV(), soLanTre));
            }
        }

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Tuyệt vời! Không có nhân viên nào đi trễ trong tháng này.");
            return;
        }

        danhSach.Sort((a, b) => b.soLan.CompareTo(a.soLan));
        int rank = 1;
        foreach (var tk in danhSach)
        {
            Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3} lần", rank++, tk.maNV, tk.hoTen, tk.soLan));
        }
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

        List<ThongKeOT> danhSach = new();
        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            double soGio = 0;
            foreach (var cc in DatabaseMock.dsChamCong)
            {
                if (cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam)
                {
                    soGio += cc.getSoGioLamThem();
                }
            }
            if (soGio > 0)
            {
                danhSach.Add(new ThongKeOT(nv.getMaNhanVien(), nv.HoTenNV(), soGio));
            }
        }

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Không có nhân viên nào làm thêm giờ trong tháng này.");
            return;
        }

        danhSach.Sort((a, b) => b.soGio.CompareTo(a.soGio));
        int rank = 1;
        foreach (var tk in danhSach)
        {
            Console.WriteLine(string.Format("{0,-6} {1,-8} {2,-25} {3:F1} giờ", rank++, tk.maNV, tk.hoTen, tk.soGio));
        }
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

        bool timThay = false;
        foreach (var don in DatabaseMock.dsDonXinNghi)
        {
            if (don.getTrangThaiDuyet() == TrangThaiDuyet.CHO_DUYET)
            {
                timThay = true;
                string tenNguoiNop = "Mã " + don.getMaNhanVien();
                foreach (var nv in DatabaseMock.dsNhanVien)
                {
                    if (nv.getMaNhanVien() == don.getMaNhanVien())
                    {
                        tenNguoiNop = nv.HoTenNV();
                        break;
                    }
                }

                string tenNguoiDuyet = "Chưa chỉ định";
                if (don.getNguoiDuyet().HasValue)
                {
                    foreach (var nv in DatabaseMock.dsNhanVien)
                    {
                        if (nv.getMaNhanVien() == don.getNguoiDuyet().Value)
                        {
                            tenNguoiDuyet = nv.HoTenNV();
                            break;
                        }
                    }
                }

                long soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
                string thoiGian = don.getNgayBatDau() + " -> " + don.getNgayKetThuc();

                Console.WriteLine(string.Format("{0,-8} {1,-22} {2,-16} {3,-10} {4,-24} {5}",
                    don.getMaDonNghi(), tenNguoiNop, don.getLoaiNghi(), soNgayNghi + " ngày", thoiGian, tenNguoiDuyet));
            }
        }

        if (!timThay)
        {
            Console.WriteLine("-> Hiện không có đơn xin nghỉ phép nào đang chờ duyệt.");
        }
    }

    // =========================================================================
    // CÂU 7: Lịch sử nghỉ phép của một nhân viên
    // =========================================================================
    public void lsNghi(int maNV)
    {
        string hoTen = "Không xác định";
        bool timThayNV = false;
        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            if (nv.getMaNhanVien() == maNV)
            {
                hoTen = nv.HoTenNV();
                timThayNV = true;
                break;
            }
        }
        if (!timThayNV)
        {
            Console.WriteLine("Không tìm thấy nhân viên có mã: " + maNV);
            return;
        }

        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("LỊCH SỬ NGHỈ PHÉP: {0} (Mã {1})", hoTen, maNV));
        Console.WriteLine("==============================================================================");
        Console.WriteLine(string.Format("{0,-8} {1,-16} {2,-10} {3,-25} {4,-14} {5}", "Mã đơn", "Loại nghỉ", "Số ngày", "Thời gian", "Trạng thái", "Lý do"));
        Console.WriteLine("------------------------------------------------------------------------------");

        bool coDon = false;
        foreach (var don in DatabaseMock.dsDonXinNghi)
        {
            if (don.getMaNhanVien() == maNV)
            {
                coDon = true;
                long soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
                string thoiGian = don.getNgayBatDau() + " -> " + don.getNgayKetThuc();

                Console.WriteLine(string.Format("{0,-8} {1,-16} {2,-10} {3,-25} {4,-14} {5}",
                    don.getMaDonNghi(), don.getLoaiNghi(), soNgayNghi + " ngày", thoiGian, don.getTrangThaiDuyet(), don.getLyDo()));
            }
        }
        if (!coDon)
        {
            Console.WriteLine("-> Nhân viên chưa có đơn xin nghỉ phép nào.");
        }
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

        bool timThay = false;
        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            int tongcong = 0;
            int dungGio = 0;
            foreach (var cc in DatabaseMock.dsChamCong)
            {
                if (cc.getMaNhanVien() == nv.getMaNhanVien()
                    && cc.getNgayLamViec().Month == thang
                    && cc.getNgayLamViec().Year == nam)
                {
                    tongcong++;
                    if (cc.getTrangThaiCong() == TrangThaiCong.DUNG_GIO)
                    {
                        dungGio++;
                    }
                }
            }
            if (tongcong == dungGio && dungGio > 0)
            {
                timThay = true;
                Console.WriteLine(string.Format("{0,-8} {1,-25} {2,-18} {3}",
                    nv.getMaNhanVien(),
                    nv.HoTenNV(),
                    dungGio + "/" + tongcong + " ngày",
                    "100%"));
            }
        }
        if (!timThay)
        {
            Console.WriteLine("-> Không có nhân viên nào đạt chuyên cần 100% trong tháng.");
        }
    }

    // =========================================================================
    // CÂU 9: Thống kê số lượng nhân viên theo từng loại hợp đồng lao động
    // =========================================================================
    public void thongkeHopDong()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("THỐNG KÊ NHÂN SỰ THEO TỪNG LOẠI HỢP ĐỒNG LAO ĐỘNG");
        Console.WriteLine("==============================================================================");

        foreach (LoaiHopDong lhd in Enum.GetValues(typeof(LoaiHopDong)))
        {
            int count = 0;
            foreach (var hd in DatabaseMock.dsHopDong)
            {
                if (hd.getLoaiHopDong() == lhd && hd.getTrangThai() != TrangThaiHopDong.DA_HET_HAN)
                {
                    count++;
                }
            }

            Console.WriteLine(string.Format("\n>> LOẠI HỢP ĐỒNG: {0} ({1} nhân viên)", lhd, count));
            if (count > 0)
            {
                Console.WriteLine(string.Format("   {0,-8} {1,-22} {2,-15} {3}", "Mã NV", "Họ và tên", "Số hợp đồng", "Trạng thái HĐ"));
                Console.WriteLine("   --------------------------------------------------------------------------");
                foreach (var hd in DatabaseMock.dsHopDong)
                {
                    if (hd.getLoaiHopDong() == lhd && hd.getTrangThai() != TrangThaiHopDong.DA_HET_HAN)
                    {
                        foreach (var nv in DatabaseMock.dsNhanVien)
                        {
                            if (nv.getMaNhanVien() == hd.getMaNhanVien())
                            {
                                Console.WriteLine(string.Format("   {0,-8} {1,-22} {2,-15} {3}",
                                    nv.getMaNhanVien(),
                                    nv.HoTenNV(),
                                    hd.getSoHopDong(),
                                    hd.getTrangThai()));
                            }
                        }
                    }
                }
            }
        }
    }

    // =========================================================================
    // CÂU 10: Lọc các điều khoản pháp lý (Bảo mật hoặc Kỷ luật) trên hệ thống
    // =========================================================================
    public void locDieuKhoanPhapLy()
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine("DANH SÁCH ĐIỀU KHOẢN PHÁP LÝ (BẢO MẬT & KỶ LUẬT)");
        Console.WriteLine("==============================================================================");

        bool timThay = false;
        foreach (var dk in DatabaseMock.dsDieuKhoanHopDong)
        {
            string loai = dk.getLoaiDieuKhoan();
            if (loai != null && (loai.Equals("Bao mat", StringComparison.OrdinalIgnoreCase) || loai.Equals("Ky luat", StringComparison.OrdinalIgnoreCase)))
            {
                timThay = true;

                string soHD = "Mã HĐ: " + dk.getMaHopDong();
                foreach (var hd in DatabaseMock.dsHopDong)
                {
                    if (hd.getMaHopDong() == dk.getMaHopDong())
                    {
                        soHD = hd.getSoHopDong();
                        break;
                    }
                }

                Console.WriteLine(string.Format("[{0}] - Điều {1} ({2}): {3}",
                    dk.getLoaiDieuKhoan().ToUpper(),
                    dk.getSoThuTuDieu(),
                    soHD,
                    dk.getTieuDeDieuKhoan()));
                Console.WriteLine(string.Format("   -> Nội dung: {0}\n", dk.getNoiDungDieuKhoan()));
            }
        }

        if (!timThay)
        {
            Console.WriteLine("-> Không tìm thấy điều khoản bảo mật hoặc kỷ luật nào.");
        }
    }
}
