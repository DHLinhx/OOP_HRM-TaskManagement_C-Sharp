using System.Text;
using com.hrmis.services;

namespace com.hrmis;

public class Program
{
    private static readonly VanHanhService VAN_HANH_SERVICE = new();
    private static readonly NhanSuService NHAN_SU_SERVICE = new();
    private static readonly TaiChinhService TAI_CHINH_SERVICE = new();

    private static void printMenu() {
        Console.WriteLine("\n" + new string('=', 110));
        Console.WriteLine("                 HỆ THỐNG THÔNG TIN QUẢN TRỊ NHÂN SỰ & CÔNG VIỆC (HRMIS)");
        Console.WriteLine(new string('=', 110));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}",
                "[ PHÂN HỆ 1: LĨNH ]", "[ PHÂN HỆ 2: DƯƠNG ]", "[ PHÂN HỆ 3: HẠO ]"));
        Console.WriteLine(new string('-', 110));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 1. Danh sách toàn bộ nhân sự", "11. In chi tiết hợp đồng", "21. In phiếu lương (Payslip)"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 2. Cơ cấu tổ chức phòng ban", "12. Cảnh báo HĐ sắp hết hạn", "22. Bảng tổng hợp lương tháng"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 3. Tra cứu theo phòng ban (*)", "13. Bảng chấm công cá nhân", "23. Quỹ lương theo phòng ban"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 4. Top 3 nhân viên thâm niên", "14. Báo cáo xếp hạng đi trễ", "24. Top 3 thực lãnh cao nhất"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 5. Cây phân cấp quản lý", "15. Báo cáo làm thêm giờ (OT)", "25. Thống kê bảo hiểm bắt buộc"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 6. Chi tiết nhóm làm việc", "16. Theo dõi đơn nghỉ phép", "26. Báo cáo thưởng & phụ cấp"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 7. Thống kê nhân sự đa nhiệm", "17. Lịch sử xin nghỉ phép", "27. In phiếu đánh giá KPI"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 8. Phân bổ theo chức vụ", "18. Thống kê chuyên cần 100%", "28. Xếp hạng hiệu suất"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", " 9. NV chưa tham gia nhóm", "19. Thống kê loại hợp đồng", "29. Tìm nhân sự xuất sắc"));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}", "10. Tìm kiếm nhân sự (*)", "20. Lọc điều khoản pháp lý", "30. Lương TB theo chức vụ"));
        Console.WriteLine(new string('-', 110));
        Console.WriteLine(string.Format(" {0,-36} {1,-36} {2,-36}",
                "[91] Demo 10 câu của LĨNH", "[92] Demo 10 câu của DƯƠNG", "[93] Demo 10 câu của HẠO"));
        Console.WriteLine("  [0] Thoát chương trình         | (*) Có nhập từ bàn phím");
        Console.WriteLine(new string('=', 110));
    }

    private static void pause() {
        Console.Write("\n>>> Nhấn phím [ENTER] để quay lại menu...");
        Console.ReadLine();
    }

    private static void executeChoice(int choice) {
        Console.WriteLine("\n=== ĐANG THỰC THI: CÂU " + choice + " ===");

        switch (choice) {
            // LĨNH (Câu 1 -> 10: Quản lý nhân sự, tổ chức phòng ban)
            case 1:
                NHAN_SU_SERVICE.DanhSachNhanVien();
                break;
            case 2:
                NHAN_SU_SERVICE.danhSachPhongBan();
                break;
            case 3:
                Console.Write("Nhập mã hoặc tên phòng ban cần tra cứu (VD: Phong CNTT / 1): ");
                string? phongBanInput = Console.ReadLine()?.Trim();
                NHAN_SU_SERVICE.traCuuNhanVienTheoPhongBan(phongBanInput ?? "");
                break;
            case 4:
                NHAN_SU_SERVICE.top3NhanVienThamNien();
                break;
            case 5:
                NHAN_SU_SERVICE.danhSachCapDuoiCuaQuanLy(1);
                break;
            case 6:
                NHAN_SU_SERVICE.chiTietNhomNhanVien(1);
                break;
            case 7:
                NHAN_SU_SERVICE.thongKeNhanVienDaNhiemNhieuNhom();
                break;
            case 8:
                NHAN_SU_SERVICE.thongKeNhanSuTheoChucVu();
                break;
            case 9:
                NHAN_SU_SERVICE.danhSachNhanVienChuaVaoNhom();
                break;
            case 10:
                Console.Write("Nhập từ khóa tìm kiếm nhân sự (họ tên, email hoặc SĐT): ");
                string? tuKhoaInput = Console.ReadLine()?.Trim();
                NHAN_SU_SERVICE.timNhanSuTheoTuKhoa(tuKhoaInput ?? "");
                break;

            // DƯƠNG (Câu 11 -> 20: Vận hành, hợp đồng, chấm công, nghỉ phép)
            case 11:
                VAN_HANH_SERVICE.inHopDongChiTiet(3);
                break;
            case 12:
                VAN_HANH_SERVICE.canhBaoHopDongSapHetHan(DateOnly.FromDateTime(DateTime.Now));
                break;
            case 13:
                VAN_HANH_SERVICE.inBangChamCongCaNhan(2, 3, 2024);
                break;
            case 14:
                VAN_HANH_SERVICE.baocaoDiTre(3, 2024);
                break;
            case 15:
                VAN_HANH_SERVICE.baocaoOT(3, 2024);
                break;
            case 16:
                VAN_HANH_SERVICE.theoDoiDonNghiPhep();
                break;
            case 17:
                VAN_HANH_SERVICE.lsNghi(2);
                break;
            case 18:
                VAN_HANH_SERVICE.thongkeChuyenCan(3, 2024);
                break;
            case 19:
                VAN_HANH_SERVICE.thongkeHopDong();
                break;
            case 20:
                VAN_HANH_SERVICE.locDieuKhoanPhapLy();
                break;

            // HẠO (Câu 21 -> 30: Tài chính tiền lương, khấu trừ, KPI)
            case 21:
                TAI_CHINH_SERVICE.inPhieuLuong(2, 3, 2024);
                break;
            case 22:
                TAI_CHINH_SERVICE.bangTongHopLuongThang(3, 2024);
                break;
            case 23:
                TAI_CHINH_SERVICE.quyLuongTheoPhongBan(3, 2024);
                break;
            case 24:
                TAI_CHINH_SERVICE.topThuNhapCaoNhat(3, 2024);
                break;
            case 25:
                TAI_CHINH_SERVICE.thongKeBaoHiem(3, 2024);
                break;
            case 26:
                TAI_CHINH_SERVICE.baoCaoThuongPhuCap(3, 2024);
                break;
            case 27:
                TAI_CHINH_SERVICE.inPhieuKPI(1);
                break;
            case 28:
                TAI_CHINH_SERVICE.xepHangHieuSuat("Quy 1", 2024);
                break;
            case 29:
                TAI_CHINH_SERVICE.timNhanSuXuatSac(1, 2024);
                break;
            case 30:
                TAI_CHINH_SERVICE.thuNhapTrungBinhTheoChucVu();
                break;

            // PHÍM TẮT CHẠY DEMO TỰ ĐỘNG THEO PHÂN HỆ
            case 91:
                Console.WriteLine("\n>>> [DEMO TỰ ĐỘNG] CHẠY TOÀN BỘ 10 CÂU CỦA LĨNH (1 -> 10) <<<");
                NHAN_SU_SERVICE.DanhSachNhanVien();
                NHAN_SU_SERVICE.danhSachPhongBan();
                NHAN_SU_SERVICE.traCuuNhanVienTheoPhongBan("Phong CNTT");
                NHAN_SU_SERVICE.top3NhanVienThamNien();
                NHAN_SU_SERVICE.danhSachCapDuoiCuaQuanLy(1);
                NHAN_SU_SERVICE.chiTietNhomNhanVien(1);
                NHAN_SU_SERVICE.thongKeNhanVienDaNhiemNhieuNhom();
                NHAN_SU_SERVICE.thongKeNhanSuTheoChucVu();
                NHAN_SU_SERVICE.danhSachNhanVienChuaVaoNhom();
                NHAN_SU_SERVICE.timNhanSuTheoTuKhoa("linh");
                break;

            case 92:
                Console.WriteLine("\n>>> [DEMO TỰ ĐỘNG] CHẠY TOÀN BỘ 10 CÂU CỦA DƯƠNG (11 -> 20) <<<");
                VAN_HANH_SERVICE.inHopDongChiTiet(3);
                VAN_HANH_SERVICE.canhBaoHopDongSapHetHan(DateOnly.FromDateTime(DateTime.Now));
                VAN_HANH_SERVICE.inBangChamCongCaNhan(2, 3, 2024);
                VAN_HANH_SERVICE.baocaoDiTre(3, 2024);
                VAN_HANH_SERVICE.baocaoOT(3, 2024);
                VAN_HANH_SERVICE.theoDoiDonNghiPhep();
                VAN_HANH_SERVICE.lsNghi(2);
                VAN_HANH_SERVICE.thongkeChuyenCan(3, 2024);
                VAN_HANH_SERVICE.thongkeHopDong();
                VAN_HANH_SERVICE.locDieuKhoanPhapLy();
                break;

            case 93:
                Console.WriteLine("\n>>> [DEMO TỰ ĐỘNG] CHẠY TOÀN BỘ 10 CÂU CỦA HẠO (21 -> 30) <<<");
                TAI_CHINH_SERVICE.inPhieuLuong(2, 3, 2024);
                TAI_CHINH_SERVICE.bangTongHopLuongThang(3, 2024);
                TAI_CHINH_SERVICE.quyLuongTheoPhongBan(3, 2024);
                TAI_CHINH_SERVICE.topThuNhapCaoNhat(3, 2024);
                TAI_CHINH_SERVICE.thongKeBaoHiem(3, 2024);
                TAI_CHINH_SERVICE.baoCaoThuongPhuCap(3, 2024);
                TAI_CHINH_SERVICE.inPhieuKPI(1);
                TAI_CHINH_SERVICE.xepHangHieuSuat("Quy 1", 2024);
                TAI_CHINH_SERVICE.timNhanSuXuatSac(1, 2024);
                TAI_CHINH_SERVICE.thuNhapTrungBinhTheoChucVu();
                break;

            default:
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                break;
        }
    }

    public static void Main(string[] args) {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true) {
            printMenu();
            Console.Write("Nhập lựa chọn của bạn (0 để thoát): ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) {
                Console.WriteLine("Vui lòng nhập một lựa chọn hợp lệ.");
                continue;
            }

            if (!int.TryParse(input, out int choice)) {
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số hiển thị trên menu.");
                continue;
            }

            if (choice == 0) {
                Console.WriteLine("\nChương trình kết thúc. Cảm ơn bạn!");
                break;
            }

            executeChoice(choice);
            pause();
        }
    }
}
