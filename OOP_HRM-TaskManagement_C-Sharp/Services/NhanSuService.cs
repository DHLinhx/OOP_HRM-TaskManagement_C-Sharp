using com.hrmis.dao;
using com.hrmis.models;

namespace com.hrmis.services;

public class NhanSuService
{
    private readonly NhanVienDAO nhanVienDAO = new();
    private readonly PhongBanDAO phongBanDAO = new();
    private readonly ChucVuDAO chucVuDAO = new();
    private readonly NhomNhanVienDAO nhomNhanVienDAO = new();
    private readonly ThanhVienNhomDAO thanhVienNhomDAO = new();

    private string getTenPhongBan(int? maPhongBan) =>
        maPhongBan.HasValue ? phongBanDAO.GetTenPhongBan(maPhongBan.Value) : "Chưa phân bổ";

    private string getTenChucVu(int? maChucVu) =>
        maChucVu.HasValue ? chucVuDAO.GetTenChucVu(maChucVu.Value) : "Chưa có";

    private string getTenNhanVien(int? maNhanVien) =>
        maNhanVien.HasValue ? (nhanVienDAO.GetById(maNhanVien.Value)?.HoTenNV() ?? "Không xác định") : "Không xác định";

    private string getTenNhom(int? maNhom) =>
        maNhom.HasValue ? nhomNhanVienDAO.GetTenNhom(maNhom.Value) : "Không xác định";

    // =============================================
    // Câu 11: Danh sách nhân viên
    // =============================================
    public void DanhSachNhanVien()
    {
        string formatHeader = "{0,-8} {1,-22} {2,-12} {3,-22} {4,-28} {5,-18}";
        Console.WriteLine(string.Format(formatHeader, "Ma NV", "Ho va Ten", "Gioi Tinh", "Phong Ban", "Chuc Vu", "Nguoi Quan Ly"));
        Console.WriteLine(new string('-', 110));

        nhanVienDAO.GetAll().ForEach(nv =>
        {
            string tenPhongBan = getTenPhongBan(nv.getMaPhongBan());
            string tenChucVu = getTenChucVu(nv.getMaChucVu());
            string tenQuanLy = nv.getMaQuanLy().HasValue ? getTenNhanVien(nv.getMaQuanLy().Value) : "Cap cao nhat";

            Console.WriteLine(string.Format(formatHeader,
                nv.getMaNhanVien(),
                nv.HoTenNV(),
                nv.getGioiTinh(),
                tenPhongBan,
                tenChucVu,
                tenQuanLy));
        });
    }

    // =============================================
    // Câu 12: Cơ cấu tổ chức phòng ban
    // =============================================
    public void danhSachPhongBan()
    {
        Console.WriteLine("\n=== Cơ cấu tổ chức phòng ban ===");
        Console.WriteLine(string.Format("{0,-20} {1,-25} {2,-10}", "Phong ban", "Truong phong", "So NV"));
        Console.WriteLine(new string('-', 60));

        phongBanDAO.GetAll().ForEach(pb =>
        {
            string truongPhong = pb.getmaTruongPhong().HasValue
                ? getTenNhanVien(pb.getmaTruongPhong().Value)
                : "Chua co";

            int soNhanVien = nhanVienDAO.GetByPhongBan(pb.getmaPhongBan() ?? 0).Count;

            Console.WriteLine(string.Format("{0,-20} {1,-25} {2,-10}",
                pb.gettenPhongBan(),
                truongPhong,
                soNhanVien));
        });
    }

    // =============================================
    // Câu 13: Tra cứu nhân viên theo phòng ban
    // =============================================
    public void traCuuNhanVienTheoPhongBan(string maHoacTenPhongBan)
    {
        if (string.IsNullOrWhiteSpace(maHoacTenPhongBan))
        {
            Console.WriteLine("Nhap ma hoac ten phong ban");
            return;
        }

        string keyword = maHoacTenPhongBan.Trim().ToLower();
        var dsKetQua = nhanVienDAO.GetAll()
            .Where(nv => nv.getMaPhongBan().ToString().Equals(keyword, StringComparison.OrdinalIgnoreCase)
                      || getTenPhongBan(nv.getMaPhongBan()).ToLower().Contains(keyword))
            .ToList();

        Console.WriteLine("\n=== Danh sach nhan vien thuoc phong ban ===");
        if (dsKetQua.Count == 0)
        {
            Console.WriteLine("Khong tim thay phong ban nao phu hop.");
            return;
        }

        dsKetQua.ForEach(nv =>
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())} | {getTenPhongBan(nv.getMaPhongBan())}");
        });
    }

    // =============================================
    // Câu 14: Top 3 nhân viên thâm niên
    // =============================================
    public void top3NhanVienThamNien()
    {
        Console.WriteLine("\n=== Top 3 nhan vien co ngay vao lam som nhat ===");

        nhanVienDAO.GetTopThamNien(3).ForEach(nv =>
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getNgayVaoLam()}");
        });
    }

    // =============================================
    // Câu 15: Cây phân cấp quản lý
    // =============================================
    public void danhSachCapDuoiCuaQuanLy(int maNhanVienQuanLy)
    {
        Console.WriteLine($"\n=== Danh sach cap duoi cua quan ly {maNhanVienQuanLy} ===");

        var dsCapDuoi = nhanVienDAO.GetCapDuoi(maNhanVienQuanLy);
        if (dsCapDuoi.Count == 0)
        {
            Console.WriteLine("Khong co cap duoi nao.");
            return;
        }

        dsCapDuoi.ForEach(nv =>
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())}");
        });
    }

    // =============================================
    // Câu 16: Chi tiết nhóm nhân viên
    // =============================================
    public void chiTietNhomNhanVien(int maNhom)
    {
        Console.WriteLine("\n=== Chi tiet nhom lam viec ===");

        var nhom = nhomNhanVienDAO.GetById(maNhom);
        if (nhom == null)
        {
            Console.WriteLine("Khong tim thay nhom.");
            return;
        }

        string truongNhom = nhom.getmaTruongNhom().HasValue ? getTenNhanVien(nhom.getmaTruongNhom().Value) : "Chưa có";
        Console.WriteLine("Ma nhom: " + nhom.getmaNhom());
        Console.WriteLine("Ten nhom: " + nhom.gettenNhom());
        Console.WriteLine("Truong nhom: " + truongNhom);
        Console.WriteLine("Ngay thanh lap: " + nhom.getngayThanhLap());
        Console.WriteLine("Mo ta: " + nhom.ToString());

        Console.WriteLine("\nDanh sach thanh vien:");
        thanhVienNhomDAO.GetByNhom(maNhom).ForEach(tv =>
        {
            var nv = nhanVienDAO.GetById(tv.getnhomNhanVien());
            if (nv != null)
            {
                Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {tv.getVaiTro()}");
            }
        });
    }

    // =============================================
    // Câu 17: Thống kê nhân sự đa nhiệm
    // =============================================
    public void thongKeNhanVienDaNhiemNhieuNhom()
    {
        Console.WriteLine("\n=== Danh sach nhan vien tham gia tu 2 nhom tro len ===");

        var dsTheoNhanVien = thanhVienNhomDAO.GetAll()
            .GroupBy(tv => tv.getnhomNhanVien())
            .Where(g => g.Count() >= 2)
            .ToList();

        dsTheoNhanVien.ForEach(entry =>
        {
            var nv = nhanVienDAO.GetById(entry.Key);
            if (nv != null)
            {
                string dsNhom = string.Join(", ", entry.Select(tv => getTenNhom(tv.getmaNhom())));
                Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {dsNhom}");
            }
        });
    }

    // =============================================
    // Câu 18: Phân bổ nhân sự theo chức danh
    // =============================================
    public void thongKeNhanSuTheoChucVu()
    {
        Console.WriteLine("\n=== Thong ke nhan su theo chuc vu ===");

        chucVuDAO.GetAll().ForEach(cv =>
        {
            int soLuong = nhanVienDAO.GetByChucVu(cv.getmaChucVu() ?? 0).Count;
            if (soLuong > 0)
            {
                Console.WriteLine($"- {cv.gettenChucVu()}: {soLuong}");
            }
        });
    }

    // =============================================
    // Câu 19: Nhân viên chưa vào nhóm
    // =============================================
    public void danhSachNhanVienChuaVaoNhom()
    {
        Console.WriteLine("\n=== Danh sach nhan vien chua tham gia nhom nao ===");

        var dsChuaVaoNhom = nhanVienDAO.GetAll()
            .Where(nv => thanhVienNhomDAO.GetByNhanVien(nv.getMaNhanVien()).Count == 0)
            .ToList();

        if (dsChuaVaoNhom.Count == 0)
        {
            Console.WriteLine("Tat ca nhan vien da tham gia nhom.");
            return;
        }

        dsChuaVaoNhom.ForEach(nv =>
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenPhongBan(nv.getMaPhongBan())}");
        });
    }

    // =============================================
    // Câu 20: Tìm kiếm nhân sự đa tiêu chí
    // =============================================
    public void timNhanSuTheoTuKhoa(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.WriteLine("Nhap tu khoa tim kiem.");
            return;
        }

        var dsKetQua = nhanVienDAO.Search(keyword);
        Console.WriteLine("\n=== Ket qua tim kiem nhan su ===");
        if (dsKetQua.Count == 0)
        {
            Console.WriteLine("Khong tim thay ket qua nao.");
            return;
        }

        dsKetQua.ForEach(nv =>
        {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getSoDienThoai()} | {nv.getEmail()} | {getTenPhongBan(nv.getMaPhongBan())}");
        });
    }
}
