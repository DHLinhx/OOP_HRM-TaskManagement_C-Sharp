using System.Text;
using com.hrmis.services;

namespace com.hrmis;

public class Program
{
    private static readonly VanHanhService VAN_HANH_SERVICE = new();
    private static readonly NhanSuService NHAN_SU_SERVICE = new();
    private static readonly TaiChinhService TAI_CHINH_SERVICE = new();

    private static void printMenu() {
        Console.WriteLine("\n===================== MENU TRUY VẤN HRM =====================");
        Console.WriteLine("--- PHÂN HỆ 1: NHÂN SỰ & TỔ CHỨC (LĨNH: CÂU 1 -> 10) ---");
        Console.WriteLine("1. Danh sách nhân viên");
        Console.WriteLine("2. Danh sách phòng ban");
        Console.WriteLine("3. Tra cứu nhân viên theo phòng ban");
        Console.WriteLine("4. Top 3 nhân viên thâm niên");
        Console.WriteLine("5. Danh sách cấp dưới của quản lý");
        Console.WriteLine("6. Chi tiết nhóm nhân viên");
        Console.WriteLine("7. Thống kê nhân viên đa nhiệm");
        Console.WriteLine("8. Thống kê nhân sự theo chức vụ");
        Console.WriteLine("9. Danh sách nhân viên chưa vào nhóm");
        Console.WriteLine("10. Tìm nhân sự theo từ khóa");

        Console.WriteLine("--- PHÂN HỆ 2: VẬN HÀNH & CHẤM CÔNG (DƯƠNG: CÂU 11 -> 20) ---");
        Console.WriteLine("11. In chi tiết hợp đồng");
        Console.WriteLine("12. Cảnh báo hợp đồng sắp hết hạn");
        Console.WriteLine("13. Bảng chấm công cá nhân");
        Console.WriteLine("14. Báo cáo đi trễ");
        Console.WriteLine("15. Báo cáo làm thêm giờ");
        Console.WriteLine("16. Theo dõi đơn nghỉ phép");
        Console.WriteLine("17. Lịch sử nghỉ của nhân viên");
        Console.WriteLine("18. Thống kê chuyên cần");
        Console.WriteLine("19. Thống kê hợp đồng");
        Console.WriteLine("20. Lọc điều khoản pháp lý");

        Console.WriteLine("--- PHÂN HỆ 3: TÀI CHÍNH & HIỆU SUẤT (HẠO: CÂU 21 -> 30) ---");
        Console.WriteLine("21. In phiếu lương cá nhân");
        Console.WriteLine("22. Bảng tổng hợp lương tháng");
        Console.WriteLine("23. Quỹ lương theo phòng ban");
        Console.WriteLine("24. Top thu nhập thực lãnh cao nhất");
        Console.WriteLine("25. Thống kê bảo hiểm");
        Console.WriteLine("26. Báo cáo thưởng phụ cấp");
        Console.WriteLine("27. In phiếu KPI");
        Console.WriteLine("28. Xếp hạng hiệu suất");
        Console.WriteLine("29. Tìm nhân sự xuất sắc");
        Console.WriteLine("30. Thu nhập trung bình theo chức vụ");
        Console.WriteLine("0. Thoát chương trình");
        Console.WriteLine("=============================================================");
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
                NHAN_SU_SERVICE.traCuuNhanVienTheoPhongBan("Phong CNTT");
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
                NHAN_SU_SERVICE.timNhanSuTheoTuKhoa("linh");
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
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số từ 0 đến 30.");
                continue;
            }

            if (choice == 0) {
                Console.WriteLine("Chương trình kết thúc. Cảm ơn bạn!");
                break;
            }

            executeChoice(choice);
        }
    }
}
