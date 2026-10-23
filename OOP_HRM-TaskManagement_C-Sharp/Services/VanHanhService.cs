using com.hrmis.dao;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.services;

public class VanHanhService
{
    private readonly HopDongDAO hopDongDAO = new();
    private readonly DieuKhoanHopDongDAO dieuKhoanDAO = new();
    private readonly ChamCongDAO chamCongDAO = new();
    private readonly DonXinNghiDAO donXinNghiDAO = new();
    private readonly NhanVienDAO nhanVienDAO = new();
    private readonly CaLamViecDAO caLamViecDAO = new();

    private string getTenNhanVien(int maNV)
    {
        var nv = nhanVienDAO.GetById(maNV);
        return nv != null ? nv.HoTenNV() : "Mã " + maNV;
    }

    private string getTenCa(int maCa) => caLamViecDAO.GetTenCa(maCa);

    private string getSoHopDong(int maHopDong)
    {
        var hd = hopDongDAO.GetById(maHopDong);
        return hd != null ? hd.getSoHopDong() : "Mã HĐ: " + maHopDong;
    }

    // =========================================================================
    // CÂU 1: Bản in hợp đồng chi tiết
    // =========================================================================
    public void inHopDongChiTiet(int maHopDong)
    {
        var hd = hopDongDAO.GetById(maHopDong);
        if (hd == null)
        {
            Console.WriteLine("Không tìm thấy hợp đồng có mã: " + maHopDong);
            return;
        }

        string tenNV = getTenNhanVien(hd.getMaNhanVien()) + " (Mã " + hd.getMaNhanVien() + ")";
        Console.WriteLine(new string('=', 78));
        Console.WriteLine("CHI TIẾT HỢP ĐỒNG LAO ĐỘNG: " + hd.getSoHopDong());
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"Nhân viên ký kết : {tenNV}");
        Console.WriteLine($"Loại hợp đồng    : {hd.getLoaiHopDong()}");
        Console.WriteLine($"Thời hạn         : {hd.getNgayHieuLuc()} -> {(hd.getNgayHetHan().HasValue ? hd.getNgayHetHan().Value.ToString() : "Vô thời hạn")}");
        Console.WriteLine($"Lương thỏa thuận : {hd.getLuongThoaThuan():N0} VNĐ");
        Console.WriteLine($"Trạng thái       : {hd.getTrangThai()}");
        Console.WriteLine(new string('-', 78));
        Console.WriteLine("CÁC ĐIỀU KHOẢN ĐIỀU KIỆN KÈM THEO:");

        var dsDK = dieuKhoanDAO.GetByHopDong(maHopDong);
        if (dsDK.Count == 0)
        {
            Console.WriteLine("  (Hợp đồng không có điều khoản phụ kèm theo)");
        }
        else
        {
            dsDK.ForEach(dk =>
            {
                Console.WriteLine($"  [Điều {dk.getSoThuTuDieu()}] {dk.getTieuDeDieuKhoan(),-25} ({dk.getLoaiDieuKhoan()})");
                Console.WriteLine($"          -> {dk.getNoiDungDieuKhoan()}");
            });
        }
    }

    // =========================================================================
    // CÂU 2: Cảnh báo hợp đồng sắp hết hạn trong 30 ngày
    // =========================================================================
    public void canhBaoHopDongSapHetHan(DateOnly mocThoiGian)
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"CẢNH BÁO HỢP ĐỒNG SẮP HẾT HẠN (30 NGÀY TỪ {mocThoiGian})");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Số HĐ",-14} {"Mã NV",-8} {"Họ và tên",-22} {"Loại hợp đồng",-18} {"Ngày hết hạn"}");
        Console.WriteLine(new string('-', 78));

        var dsSapHetHan = hopDongDAO.GetSapHetHan(mocThoiGian, 30);
        if (dsSapHetHan.Count == 0)
        {
            Console.WriteLine("-> Không có hợp đồng nào sắp hết hạn trong 30 ngày tới.");
            return;
        }

        dsSapHetHan.ForEach(hd =>
        {
            string hoTen = getTenNhanVien(hd.getMaNhanVien());
            Console.WriteLine($"{hd.getSoHopDong(),-14} {hd.getMaNhanVien(),-8} {hoTen,-22} {hd.getLoaiHopDong(),-18} {hd.getNgayHetHan()}");
        });
    }

    // =========================================================================
    // CÂU 3: Bảng chấm công cá nhân
    // =========================================================================
    public void inBangChamCongCaNhan(int maNhanVien, int thang, int nam)
    {
        string hoTen = getTenNhanVien(maNhanVien);
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"BẢNG CHẤM CÔNG NHÂN VIÊN: {hoTen} (Mã {maNhanVien}) - THÁNG {thang:D2}/{nam}");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Ngày",-12} {"Ca làm việc",-18} {"Giờ vào",-10} {"Giờ ra",-10} {"Số giờ làm",-12} {"Trạng thái"}");
        Console.WriteLine(new string('-', 78));

        var dsCong = chamCongDAO.GetByNhanVienVaThang(maNhanVien, thang, nam);
        if (dsCong.Count == 0)
        {
            Console.WriteLine("-> Không tìm thấy dữ liệu chấm công trong tháng này.");
            return;
        }

        dsCong.ForEach(cc =>
        {
            string tenCa = getTenCa(cc.getMaCa());
            double soGioLam = (cc.getGioVaoThucTe().HasValue && cc.getGioRaThucTe().HasValue)
                ? (cc.getGioRaThucTe().Value.ToTimeSpan() - cc.getGioVaoThucTe().Value.ToTimeSpan()).TotalHours
                : 0.0;

            Console.WriteLine($"{cc.getNgayLamViec(),-12} {tenCa,-18} {cc.getGioVaoThucTe(),-10} {cc.getGioRaThucTe(),-10} {$"{soGioLam:F1} giờ",-12} {cc.getTrangThaiCong()}");
        });
    }

    // =========================================================================
    // CÂU 4: Báo cáo đi trễ trong tháng
    // =========================================================================
    public record ThongKeDiTre(int maNV, string hoTen, int soLan);

    public void baocaoDiTre(int thang, int nam)
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"BÁO CÁO NHÂN VIÊN ĐI TRỄ - THÁNG {thang:D2}/{nam}");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Hạng",-6} {"Mã NV",-8} {"Họ và tên",-25} {"Số lần đi trễ"}");
        Console.WriteLine(new string('-', 78));

        var danhSach = nhanVienDAO.GetAll()
            .Select(nv => new ThongKeDiTre(nv.getMaNhanVien(), nv.HoTenNV(), chamCongDAO.CountDiTre(nv.getMaNhanVien(), thang, nam)))
            .Where(tk => tk.soLan > 0)
            .OrderByDescending(tk => tk.soLan)
            .ToList();

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Tuyệt vời! Không có nhân viên nào đi trễ trong tháng này.");
            return;
        }

        int rank = 1;
        danhSach.ForEach(tk => Console.WriteLine($"{rank++,-6} {tk.maNV,-8} {tk.hoTen,-25} {tk.soLan} lần"));
    }

    // =========================================================================
    // CÂU 5: Báo cáo làm thêm giờ OT
    // =========================================================================
    public record ThongKeOT(int maNV, string hoTen, double soGio);

    public void baocaoOT(int thang, int nam)
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"BÁO CÁO TOP NHÂN VIÊN LÀM THÊM GIỜ (OT) - THÁNG {thang:D2}/{nam}");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Hạng",-6} {"Mã NV",-8} {"Họ và tên",-25} {"Tổng số giờ OT"}");
        Console.WriteLine(new string('-', 78));

        var danhSach = nhanVienDAO.GetAll()
            .Select(nv => new ThongKeOT(nv.getMaNhanVien(), nv.HoTenNV(), chamCongDAO.SumGioOT(nv.getMaNhanVien(), thang, nam)))
            .Where(tk => tk.soGio > 0)
            .OrderByDescending(tk => tk.soGio)
            .ToList();

        if (danhSach.Count == 0)
        {
            Console.WriteLine("-> Không có nhân viên nào làm thêm giờ trong tháng này.");
            return;
        }

        int rank = 1;
        danhSach.ForEach(tk => Console.WriteLine($"{rank++,-6} {tk.maNV,-8} {tk.hoTen,-25} {tk.soGio:F1} giờ"));
    }

    // =========================================================================
    // CÂU 6: Bảng theo dõi đơn nghỉ phép đang CHỜ DUYỆT
    // =========================================================================
    public void theoDoiDonNghiPhep()
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine("DANH SÁCH ĐƠN NGHỈ PHÉP ĐANG CHỜ DUYỆT");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Mã đơn",-8} {"Người nộp",-22} {"Loại nghỉ",-16} {"Số ngày",-10} {"Thời gian",-24} {"Người duyệt"}");
        Console.WriteLine(new string('-', 78));

        var dsChoDuyet = donXinNghiDAO.GetChoDuyet();
        if (dsChoDuyet.Count == 0)
        {
            Console.WriteLine("-> Hiện không có đơn xin nghỉ phép nào đang chờ duyệt.");
            return;
        }

        dsChoDuyet.ForEach(don =>
        {
            string tenNguoiNop = getTenNhanVien(don.getMaNhanVien());
            string tenNguoiDuyet = don.getNguoiDuyet().HasValue ? getTenNhanVien(don.getNguoiDuyet().Value) : "Chưa chỉ định";
            int soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
            string thoiGian = $"{don.getNgayBatDau()} -> {don.getNgayKetThuc()}";
            Console.WriteLine($"{don.getMaDonNghi(),-8} {tenNguoiNop,-22} {don.getLoaiNghi(),-16} {$"{soNgayNghi} ngày",-10} {thoiGian,-24} {tenNguoiDuyet}");
        });
    }

    // =========================================================================
    // CÂU 7: Lịch sử nghỉ phép của một nhân viên
    // =========================================================================
    public void lsNghi(int maNV)
    {
        var nv = nhanVienDAO.GetById(maNV);
        if (nv == null)
        {
            Console.WriteLine("Không tìm thấy nhân viên có mã: " + maNV);
            return;
        }

        string hoTen = nv.HoTenNV();
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"LỊCH SỬ NGHỈ PHÉP: {hoTen} (Mã {maNV})");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Mã đơn",-8} {"Loại nghỉ",-16} {"Số ngày",-10} {"Thời gian",-25} {"Trạng thái",-14} {"Lý do"}");
        Console.WriteLine(new string('-', 78));

        var dsNghi = donXinNghiDAO.GetByNhanVien(maNV);
        if (dsNghi.Count == 0)
        {
            Console.WriteLine("-> Nhân viên chưa có đơn xin nghỉ phép nào.");
            return;
        }

        dsNghi.ForEach(don =>
        {
            int soNgayNghi = don.getNgayKetThuc().DayNumber - don.getNgayBatDau().DayNumber + 1;
            string thoiGian = $"{don.getNgayBatDau()} -> {don.getNgayKetThuc()}";
            Console.WriteLine($"{don.getMaDonNghi(),-8} {don.getLoaiNghi(),-16} {$"{soNgayNghi} ngày",-10} {thoiGian,-25} {don.getTrangThaiDuyet(),-14} {don.getLyDo()}");
        });
    }

    // =========================================================================
    // CÂU 8: Thống kê nhân viên đạt chuyên cần 100%
    // =========================================================================
    public void thongkeChuyenCan(int thang, int nam)
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"DANH SÁCH NHÂN VIÊN ĐẠT CHUYÊN CẦN 100% - THÁNG {thang:D2}/{nam}");
        Console.WriteLine(new string('=', 78));
        Console.WriteLine($"{"Mã NV",-8} {"Họ và tên",-25} {"Số ngày công",-18} {"Tỷ lệ đúng giờ"}");
        Console.WriteLine(new string('-', 78));

        var dsChuyenCan = nhanVienDAO.GetAll()
            .Where(nv => chamCongDAO.IsChuyenCan100PhanTram(nv.getMaNhanVien(), thang, nam))
            .ToList();

        if (dsChuyenCan.Count == 0)
        {
            Console.WriteLine("-> Không có nhân viên nào đạt chuyên cần 100% trong tháng.");
            return;
        }

        dsChuyenCan.ForEach(nv =>
        {
            int tongCong = chamCongDAO.GetByNhanVienVaThang(nv.getMaNhanVien(), thang, nam).Count;
            Console.WriteLine($"{nv.getMaNhanVien(),-8} {nv.HoTenNV(),-25} {$"{tongCong}/{tongCong} ngày",-18} {"100%"}");
        });
    }

    // =========================================================================
    // CÂU 9: Thống kê số lượng nhân sự theo từng loại hợp đồng
    // =========================================================================
    public void thongkeHopDong()
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine("THỐNG KÊ NHÂN SỰ THEO TỪNG LOẠI HỢP ĐỒNG LAO ĐỘNG");
        Console.WriteLine(new string('=', 78));

        foreach (LoaiHopDong lhd in Enum.GetValues<LoaiHopDong>())
        {
            var dsHDTheoLoai = hopDongDAO.GetByLoai(lhd)
                .Where(hd => hd.getTrangThai() != TrangThaiHopDong.DA_HET_HAN)
                .ToList();

            Console.WriteLine($"\n>> LOẠI HỢP ĐỒNG: {lhd} ({dsHDTheoLoai.Count} nhân viên)");
            if (dsHDTheoLoai.Count > 0)
            {
                Console.WriteLine($"   {"Mã NV",-8} {"Họ và tên",-22} {"Số hợp đồng",-15} {"Trạng thái HĐ"}");
                Console.WriteLine("   " + new string('-', 74));
                dsHDTheoLoai.ForEach(hd =>
                {
                    string hoTen = getTenNhanVien(hd.getMaNhanVien());
                    Console.WriteLine($"   {hd.getMaNhanVien(),-8} {hoTen,-22} {hd.getSoHopDong(),-15} {hd.getTrangThai()}");
                });
            }
        }
    }

    // =========================================================================
    // CÂU 10: Lọc các điều khoản pháp lý
    // =========================================================================
    public void locDieuKhoanPhapLy()
    {
        Console.WriteLine(new string('=', 78));
        Console.WriteLine("DANH SÁCH ĐIỀU KHOẢN PHÁP LÝ (BẢO MẬT & KỶ LUẬT)");
        Console.WriteLine(new string('=', 78));

        var dsDKPhapLy = dieuKhoanDAO.GetByLoaiDieuKhoan("Bao mat", "Ky luat");
        if (dsDKPhapLy.Count == 0)
        {
            Console.WriteLine("-> Không tìm thấy điều khoản bảo mật hoặc kỷ luật nào.");
            return;
        }

        dsDKPhapLy.ForEach(dk =>
        {
            string soHD = getSoHopDong(dk.getMaHopDong());
            Console.WriteLine($"[{dk.getLoaiDieuKhoan().ToUpper()}] - Điều {dk.getSoThuTuDieu()} ({soHD}): {dk.getTieuDeDieuKhoan()}");
            Console.WriteLine($"   -> Nội dung: {dk.getNoiDungDieuKhoan()}\n");
        });
    }
}
