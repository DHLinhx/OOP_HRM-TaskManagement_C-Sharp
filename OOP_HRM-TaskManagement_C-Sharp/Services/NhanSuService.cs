using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.services;

public class NhanSuService
{
    private string getTenPhongBan(int? maPhongBan)
    {
        var pb = DatabaseMock.dsPhongBan.FirstOrDefault(p => p.getmaPhongBan() != null && p.getmaPhongBan() == maPhongBan);
        return pb?.gettenPhongBan() ?? "Chua phan bo";
    }

    private string getTenChucVu(int? maChucVu)
    {
        var cv = DatabaseMock.dsChucVu.FirstOrDefault(c => c.getmaChucVu() != null && c.getmaChucVu() == maChucVu);
        return cv?.gettenChucVu() ?? "Chua co";
    }

    private string getTenNhanVien(int? maNhanVien)
    {
        var nv = DatabaseMock.dsNhanVien.FirstOrDefault(n => n.getMaNhanVien() == maNhanVien);
        return nv?.HoTenNV() ?? "Khong xac dinh";
    }

    private string getTenNhom(int? maNhom)
    {
        var nh = DatabaseMock.dsNhomNhanVien.FirstOrDefault(n => n.getmaNhom() != null && n.getmaNhom() == maNhom);
        return nh?.gettenNhom() ?? "Khong xac dinh";
    }

    // =============================================
    // Câu 11 (Câu 1 Linh): Danh sách nhân viên
    // =============================================
    public void DanhSachNhanVien()
    {
        string formatHeader = "{0,-8} {1,-22} {2,-12} {3,-22} {4,-28} {5,-18}";
        Console.WriteLine(string.Format(formatHeader, "Ma NV", "Ho va Ten", "Gioi Tinh", "Phong Ban", "Chuc Vu", "Nguoi Quan Ly"));
        Console.WriteLine(new string('-', 110));

        foreach (var nv in DatabaseMock.dsNhanVien)
        {
            string tenPhongBan = getTenPhongBan(nv.getMaPhongBan());
            string tenChucVu = getTenChucVu(nv.getMaChucVu());
            string tenQuanLy = "Cap cao nhat";
            if (nv.getMaQuanLy().HasValue)
            {
                tenQuanLy = getTenNhanVien(nv.getMaQuanLy().Value);
            }

            Console.WriteLine(string.Format(formatHeader,
                nv.getMaNhanVien(),
                nv.HoTenNV(),
                nv.getGioiTinh(),
                tenPhongBan,
                tenChucVu,
                tenQuanLy));
        }
    }

    // =============================================
    // Câu 12 (Câu 2 Linh): Cơ cấu tổ chức phòng ban
    // =============================================
    public void danhSachPhongBan()
    {
        Console.WriteLine("\n=== Cơ cấu tổ chức phòng ban ===");
        Console.WriteLine(string.Format("{0,-20} {1,-25} {2,-10}", "Phong ban", "Truong phong", "So NV"));
        Console.WriteLine(new string('-', 60));

        foreach (var pb in DatabaseMock.dsPhongBan)
        {
            string truongPhong = "Chua co";
            if (pb.getmaTruongPhong().HasValue)
            {
                var tp = DatabaseMock.dsNhanVien.FirstOrDefault(nv => nv.getMaNhanVien() == pb.getmaTruongPhong().Value);
                if (tp != null) truongPhong = tp.HoTenNV();
            }

            long soNhanVien = DatabaseMock.dsNhanVien.Count(nv => nv.getMaPhongBan() == pb.getmaPhongBan());

            Console.WriteLine(string.Format("{0,-20} {1,-25} {2,-10}",
                pb.gettenPhongBan(),
                truongPhong,
                soNhanVien));
        }
    }

    // =============================================
    // Câu 13 (Câu 3 Linh): Tra cứu nhân viên theo phòng ban
    // =============================================
    public void traCuuNhanVienTheoPhongBan(string maHoacTenPhongBan)
    {
        if (string.IsNullOrWhiteSpace(maHoacTenPhongBan))
        {
            Console.WriteLine("Nhap ma hoac ten phong ban");
            return;
        }

        string keyword = maHoacTenPhongBan.Trim().ToLower();
        var dsKetQua = DatabaseMock.dsNhanVien.Where(nv =>
            nv.getMaPhongBan().ToString().ToLower().Equals(keyword) ||
            getTenPhongBan(nv.getMaPhongBan()).ToLower().Contains(keyword)
        ).ToList();

        Console.WriteLine("\n=== Danh sach nhan vien thuoc phong ban ===");
        if (dsKetQua.Count == 0)
        {
            Console.WriteLine("Khong tim thay phong ban nao phu hop.");
            return;
        }

        foreach (var nv in dsKetQua)
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())} | {getTenPhongBan(nv.getMaPhongBan())}");
        }
    }

    // =============================================
    // Câu 14 (Câu 4 Linh): Top 3 nhân viên thâm niên
    // =============================================
    public void top3NhanVienThamNien()
    {
        Console.WriteLine("\n=== Top 3 nhan vien co ngay vao lam som nhat ===");

        var top3 = DatabaseMock.dsNhanVien.OrderBy(nv => nv.getNgayVaoLam()).Take(3);
        foreach (var nv in top3)
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getNgayVaoLam()}");
        }
    }

    // =============================================
    // Câu 15 (Câu 5 Linh): Cây phân cấp quản lý
    // =============================================
    public void danhSachCapDuoiCuaQuanLy(int maNhanVienQuanLy)
    {
        Console.WriteLine($"\n=== Danh sach cap duoi cua quan ly {maNhanVienQuanLy} ===");

        var dsCapDuoi = DatabaseMock.dsNhanVien.Where(nv => nv.getMaQuanLy().HasValue && nv.getMaQuanLy().Value == maNhanVienQuanLy).ToList();

        if (dsCapDuoi.Count == 0)
        {
            Console.WriteLine("Khong co cap duoi nao.");
            return;
        }

        foreach (var nv in dsCapDuoi)
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())}");
        }
    }

    // =============================================
    // Câu 16 (Câu 6 Linh): Chi tiết một nhóm làm việc
    // =============================================
    public void chiTietNhomNhanVien(int maNhom)
    {
        Console.WriteLine("\n=== Chi tiet nhom lam viec ===");

        var nhom = DatabaseMock.dsNhomNhanVien.FirstOrDefault(nh => nh.getmaNhom().HasValue && nh.getmaNhom().Value == maNhom);

        if (nhom == null)
        {
            Console.WriteLine("Khong tim thay nhom.");
            return;
        }

        string truongNhom = getTenNhanVien(nhom.getmaTruongNhom());
        Console.WriteLine("Ma nhom: " + nhom.getmaNhom());
        Console.WriteLine("Ten nhom: " + nhom.gettenNhom());
        Console.WriteLine("Truong nhom: " + truongNhom);
        Console.WriteLine("Ngay thanh lap: " + nhom.getngayThanhLap());
        Console.WriteLine("Mo ta: " + nhom.ToString());

        Console.WriteLine("\nDanh sach thanh vien:");
        var thanhVienList = DatabaseMock.dsThanhVienNhom.Where(tv => tv.getmaNhom().HasValue && tv.getmaNhom().Value == maNhom).ToList();
        foreach (var tv in thanhVienList)
        {
            var nv = DatabaseMock.dsNhanVien.FirstOrDefault(n => n.getMaNhanVien() == tv.getnhomNhanVien());
            if (nv != null)
            {
                Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {tv.getVaiTro()}");
            }
        }
    }

    // =============================================
    // Câu 17 (Câu 7 Linh): Thống kê nhân sự đa nhiệm
    // =============================================
    public void thongKeNhanVienDaNhiemNhieuNhom()
    {
        Console.WriteLine("\n=== Danh sach nhan vien tham gia tu 2 nhom tro len ===");

        var dsTheoNhanVien = DatabaseMock.dsThanhVienNhom.GroupBy(tv => tv.getnhomNhanVien());

        foreach (var entry in dsTheoNhanVien.Where(g => g.Count() >= 2))
        {
            var nv = DatabaseMock.dsNhanVien.FirstOrDefault(n => n.getMaNhanVien() == entry.Key);
            if (nv != null)
            {
                string dsNhom = string.Join(", ", entry.Select(tv => tv.getmaNhom().HasValue ? getTenNhom(tv.getmaNhom().Value) : ""));
                Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {dsNhom}");
            }
        }
    }

    // =============================================
    // Câu 18 (Câu 8 Linh): Phân bổ nhân sự theo chức danh
    // =============================================
    public void thongKeNhanSuTheoChucVu()
    {
        Console.WriteLine("\n=== Thong ke nhan su theo chuc vu ===");

        foreach (var cv in DatabaseMock.dsChucVu)
        {
            long soLuong = DatabaseMock.dsNhanVien.Count(nv => nv.getMaChucVu() == cv.getmaChucVu());
            if (soLuong > 0)
            {
                Console.WriteLine($"- {cv.gettenChucVu()}: {soLuong}");
            }
        }
    }

    // =============================================
    // Câu 19 (Câu 9 Linh): Nhân viên chưa vào nhóm
    // =============================================
    public void danhSachNhanVienChuaVaoNhom()
    {
        Console.WriteLine("\n=== Danh sach nhan vien chua tham gia nhom nao ===");

        var dsChuaVaoNhom = DatabaseMock.dsNhanVien
            .Where(nv => !DatabaseMock.dsThanhVienNhom.Any(tv => tv.getnhomNhanVien() == nv.getMaNhanVien()))
            .ToList();

        if (dsChuaVaoNhom.Count == 0)
        {
            Console.WriteLine("Tat ca nhan vien da tham gia nhom.");
            return;
        }

        foreach (var nv in dsChuaVaoNhom)
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenPhongBan(nv.getMaPhongBan())}");
        }
    }

    // =============================================
    // Câu 20 (Câu 10 Linh): Tìm kiếm nhân sự đa tiêu chí
    // =============================================
    public void timNhanSuTheoTuKhoa(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.WriteLine("Nhap tu khoa tim kiem.");
            return;
        }

        string key = keyword.Trim().ToLower();
        var dsKetQua = DatabaseMock.dsNhanVien.Where(nv =>
            nv.HoTenNV().ToLower().Contains(key) ||
            nv.getSoDienThoai().ToLower().Contains(key) ||
            nv.getEmail().ToLower().Contains(key)
        ).ToList();

        Console.WriteLine("\n=== Ket qua tim kiem nhan su ===");
        if (dsKetQua.Count == 0)
        {
            Console.WriteLine("Khong tim thay ket qua nao.");
            return;
        }

        foreach (var nv in dsKetQua)
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getSoDienThoai()} | {nv.getEmail()} | {getTenPhongBan(nv.getMaPhongBan())}");
        }
    }
}
