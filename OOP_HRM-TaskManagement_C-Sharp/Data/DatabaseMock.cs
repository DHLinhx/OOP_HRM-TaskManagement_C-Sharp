using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.data;

public class DatabaseMock
{
    public static List<PhongBan> dsPhongBan = new();
    public static List<ChucVu> dsChucVu = new();
    public static List<NhanVien> dsNhanVien = new();
    public static List<NhomNhanVien> dsNhomNhanVien = new();
    public static List<ThanhVienNhom> dsThanhVienNhom = new();
    public static List<HopDong> dsHopDong = new();
    public static List<DieuKhoanHopDong> dsDieuKhoanHopDong = new();
    public static List<CaLamViec> dsCaLamViec = new();
    public static List<ChamCong> dsChamCong = new();
    public static List<DonXinNghi> dsDonXinNghi = new();
    public static List<BangLuong> dsBangLuong = new();
    public static List<ChiTietBangLuong> dsChiTietBangLuong = new();
    public static List<DanhGiaHieuSuat> dsDanhGiaHieuSuat = new();
    public static List<ChiTietDanhGia> dsChiTietDanhGia = new();

    public static void initData()
    {
        dsPhongBan.Clear();
        dsChucVu.Clear();
        dsNhanVien.Clear();
        dsNhomNhanVien.Clear();
        dsThanhVienNhom.Clear();
        dsHopDong.Clear();
        dsDieuKhoanHopDong.Clear();
        dsCaLamViec.Clear();
        dsChamCong.Clear();
        dsDonXinNghi.Clear();
        dsBangLuong.Clear();
        dsChiTietBangLuong.Clear();
        dsDanhGiaHieuSuat.Clear();
        dsChiTietDanhGia.Clear();

        // ==========================================
        // 1. CHỨC VỤ & PHÒNG BAN
        // ==========================================
        dsChucVu.Add(new ChucVu(1, "Giam doc Ky thuat"));
        dsChucVu.Add(new ChucVu(2, "Truong phong Kinh doanh"));
        dsChucVu.Add(new ChucVu(3, "Lap trinh vien Senior"));
        dsChucVu.Add(new ChucVu(4, "Lap trinh vien Junior"));
        dsChucVu.Add(new ChucVu(5, "Chuyen vien Nhan su"));
        dsChucVu.Add(new ChucVu(6, "Thuc tap sinh"));
        dsChucVu.Add(new ChucVu(7, "Truong phong Nhan su"));
        dsChucVu.Add(new ChucVu(8, "Chuyen vien Sales"));

        dsPhongBan.Add(new PhongBan(1, "Phong CNTT", 1, "Tang 4 - Toa A"));
        dsPhongBan.Add(new PhongBan(2, "Phong Kinh doanh", 3, "Tang 3 - Toa A"));
        dsPhongBan.Add(new PhongBan(3, "Phong Nhan su", 5, "Tang 2 - Toa B"));

        // ==========================================
        // 2. NHÂN VIÊN (10 người)
        // ==========================================
        dsNhanVien.Add(new NhanVien(1, "Nguyen Van", "An", "an.nguyen@company.com", "0901234567",
            new DateOnly(1988, 3, 15), GioiTinh.NAM, new DateOnly(2018, 1, 10),
            TrangThaiLamViec.DANG_LAM_VIEC, 1, 1, null));
        dsNhanVien.Add(new NhanVien(3, "Le Hoang", "Cuong", "cuong.le@company.com", "0987654321",
            new DateOnly(1985, 11, 5), GioiTinh.NAM, new DateOnly(2017, 3, 1),
            TrangThaiLamViec.DANG_LAM_VIEC, 2, 2, 1));
        dsNhanVien.Add(new NhanVien(5, "Vu Thi", "Hoa", "hoa.vu@company.com", "0977889900",
            new DateOnly(1990, 12, 30), GioiTinh.NU, new DateOnly(2019, 10, 1),
            TrangThaiLamViec.DANG_LAM_VIEC, 3, 7, 1));

        dsNhanVien.Add(new NhanVien(2, "Tran Thi", "Binh", "binh.tran@company.com", "0912345678",
            new DateOnly(1992, 7, 22), GioiTinh.NU, new DateOnly(2019, 5, 20),
            TrangThaiLamViec.DANG_LAM_VIEC, 1, 3, 1));
        dsNhanVien.Add(new NhanVien(4, "Pham Minh", "Duc", "duc.pham@company.com", "0933445566",
            new DateOnly(1996, 9, 12), GioiTinh.NAM, new DateOnly(2021, 8, 15),
            TrangThaiLamViec.DANG_LAM_VIEC, 1, 4, 1));
        dsNhanVien.Add(new NhanVien(6, "Dang Quoc", "Khoa", "khoa.dang@company.com", "0944556677",
            new DateOnly(2001, 2, 18), GioiTinh.NAM, new DateOnly(2023, 6, 1),
            TrangThaiLamViec.DANG_LAM_VIEC, 1, 6, 2));
        dsNhanVien.Add(new NhanVien(7, "Ngo Thanh", "Giang", "giang.ngo@company.com", "0955667788",
            new DateOnly(1995, 4, 10), GioiTinh.NU, new DateOnly(2020, 2, 15),
            TrangThaiLamViec.DANG_LAM_VIEC, 1, 3, 2));
        dsNhanVien.Add(new NhanVien(8, "Bui Van", "Hieu", "hieu.bui@company.com", "0966778899",
            new DateOnly(1997, 8, 25), GioiTinh.NAM, new DateOnly(2022, 1, 10),
            TrangThaiLamViec.DANG_LAM_VIEC, 2, 8, 3));
        dsNhanVien.Add(new NhanVien(9, "Dinh Thi", "Linh", "linh.dinh@company.com", "0999888777",
            new DateOnly(1998, 1, 5), GioiTinh.NU, new DateOnly(2021, 11, 20),
            TrangThaiLamViec.DANG_LAM_VIEC, 3, 5, 5));
        dsNhanVien.Add(new NhanVien(10, "Hoang Anh", "Tuan", "tuan.hoang@company.com", "0911223344",
            new DateOnly(1999, 5, 20), GioiTinh.NAM, new DateOnly(2023, 1, 5),
            TrangThaiLamViec.DANG_LAM_VIEC, 2, 8, 3));

        // ==========================================
        // 3. NHÓM & THÀNH VIÊN NHÓM
        // ==========================================
        dsNhomNhanVien.Add(new NhomNhanVien(1, "Doi Du an Mobile App", 2, new DateOnly(2024, 1, 15),
            "Lam ung dung Mobile"));
        dsNhomNhanVien.Add(new NhomNhanVien(2, "Doi Nghien cuu AI", 1, new DateOnly(2023, 11, 1),
            "Nghien cuu mo hinh AI"));

        dsThanhVienNhom.Add(new ThanhVienNhom(1, 2, new DateOnly(2024, 1, 15), "Tech Lead"));
        dsThanhVienNhom.Add(new ThanhVienNhom(1, 4, new DateOnly(2024, 2, 1), "Mobile Dev"));
        dsThanhVienNhom.Add(new ThanhVienNhom(1, 7, new DateOnly(2024, 3, 1), "Tester"));
        dsThanhVienNhom.Add(new ThanhVienNhom(2, 1, new DateOnly(2023, 11, 1), "Truong nhom"));
        dsThanhVienNhom.Add(new ThanhVienNhom(2, 2, new DateOnly(2023, 11, 10), "Chuyen gia AI"));

        // ==========================================
        // 4. HỢP ĐỒNG & ĐIỀU KHOẢN
        // ==========================================
        dsHopDong.Add(new HopDong(1, 2, "HD-2023-001", LoaiHopDong.THU_VIEC,
            new DateOnly(2023, 5, 20), new DateOnly(2023, 7, 20), 8000000,
            TrangThaiHopDong.DA_HET_HAN));
        dsHopDong.Add(new HopDong(2, 1, "HD-2022-005", LoaiHopDong.VO_THOI_HAN,
            new DateOnly(2022, 1, 10), null, 45000000, TrangThaiHopDong.DANG_HIEU_LUC));
        dsHopDong.Add(new HopDong(3, 2, "HD-2025-045", LoaiHopDong.XAC_DINH_THOI_HAN,
            new DateOnly(2025, 5, 20), new DateOnly(2027, 5, 20), 30000000,
            TrangThaiHopDong.DANG_HIEU_LUC));
        dsHopDong.Add(new HopDong(4, 4, "HD-2025-088", LoaiHopDong.XAC_DINH_THOI_HAN,
            new DateOnly(2025, 3, 1), DateOnly.FromDateTime(DateTime.Now).AddDays(20), 16000000,
            TrangThaiHopDong.SAP_HET_HAN));
        dsHopDong.Add(new HopDong(5, 6, "HD-2026-012", LoaiHopDong.THU_VIEC,
            new DateOnly(2026, 1, 5), DateOnly.FromDateTime(DateTime.Now).AddDays(10), 5000000,
            TrangThaiHopDong.SAP_HET_HAN));

        dsDieuKhoanHopDong.Add(new DieuKhoanHopDong(1, 3, 1, "Thoi gian lam viec", "T2 - T6, 8h00 - 17h30",
            "Quy dinh"));
        dsDieuKhoanHopDong.Add(new DieuKhoanHopDong(2, 3, 2, "Bao mat thong tin",
            "Khong tiet lo ma nguon ra ngoai", "Bao mat"));
        dsDieuKhoanHopDong.Add(new DieuKhoanHopDong(3, 3, 3, "Ky luat cong ty",
            "Tuan thu dung noi quy lao dong", "Ky luat"));

        // ==========================================
        // 5. CA LÀM VIỆC & CHẤM CÔNG
        // ==========================================
        dsCaLamViec.Add(new CaLamViec(1, "Ca hanh chinh", new TimeOnly(8, 0), new TimeOnly(17, 30)));
        dsCaLamViec.Add(new CaLamViec(2, "Ca kip sang", new TimeOnly(6, 0), new TimeOnly(14, 0)));
        dsCaLamViec.Add(new CaLamViec(3, "Ca kip chieu", new TimeOnly(14, 0), new TimeOnly(22, 0)));
        dsCaLamViec.Add(new CaLamViec(4, "Ca linh hoat", new TimeOnly(9, 0), new TimeOnly(18, 30)));
        dsCaLamViec.Add(new CaLamViec(5, "Ca ban thoi gian", new TimeOnly(8, 0), new TimeOnly(12, 0)));

        dsChamCong.Add(new ChamCong(1, 1, 1, new DateOnly(2024, 3, 1), new TimeOnly(7, 55),
            new TimeOnly(17, 35), 0.0, TrangThaiCong.DUNG_GIO));
        dsChamCong.Add(new ChamCong(2, 1, 1, new DateOnly(2024, 3, 2), new TimeOnly(7, 50),
            new TimeOnly(19, 30), 2.0, TrangThaiCong.DUNG_GIO));
        dsChamCong.Add(new ChamCong(3, 2, 2, new DateOnly(2024, 3, 1), new TimeOnly(6, 20),
            new TimeOnly(14, 0), 0.0, TrangThaiCong.DI_TRE));
        dsChamCong.Add(new ChamCong(4, 2, 3, new DateOnly(2024, 3, 2), new TimeOnly(14, 20),
            new TimeOnly(23, 30), 1.5, TrangThaiCong.DI_TRE));
        dsChamCong.Add(new ChamCong(5, 4, 4, new DateOnly(2024, 3, 2), new TimeOnly(8, 55),
            new TimeOnly(21, 30), 3.0, TrangThaiCong.DUNG_GIO));
        dsChamCong.Add(new ChamCong(6, 7, 5, new DateOnly(2024, 3, 2), new TimeOnly(7, 55),
            new TimeOnly(13, 30), 1.5, TrangThaiCong.DUNG_GIO));
        dsChamCong.Add(new ChamCong(7, 8, 1, new DateOnly(2024, 3, 2), new TimeOnly(8, 0),
            new TimeOnly(18, 30), 1.0, TrangThaiCong.DUNG_GIO));

        // ==========================================
        // 6. ĐƠN XIN NGHỈ
        // ==========================================
        dsDonXinNghi.Add(new DonXinNghi(1, 2, LoaiNghi.NGHI_PHEP_NAM, new DateOnly(2024, 3, 10),
            new DateOnly(2024, 3, 11), "Viec gia dinh", TrangThaiDuyet.CHO_DUYET, 1));
        dsDonXinNghi.Add(new DonXinNghi(2, 4, LoaiNghi.NGHI_OM, new DateOnly(2024, 3, 5),
            new DateOnly(2024, 3, 5), "Kham benh", TrangThaiDuyet.DA_DUYET, 2));

        // ==========================================
        // 7. BẢNG LƯƠNG & CHI TIẾT
        // ==========================================
        dsBangLuong.Add(new BangLuong(1, 1, 3, 2024, 49000000.0, 4650000.0, 44350000.0,
            TrangThaiChiTra.DA_THANH_TOAN, new DateOnly(2024, 4, 5)));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(1, 1, "Luong co ban", LoaiKhoanMuc.CONG, 45000000.0, "Luong co ban"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(2, 1, "Phu cap trach nhiem", LoaiKhoanMuc.CONG, 4000000.0, "Phu cap GD"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(3, 1, "Bao hiem xa hoi", LoaiKhoanMuc.TRU, 4650000.0, "BHXH, BHYT"));

        dsBangLuong.Add(new BangLuong(2, 2, 3, 2024, 33500000.0, 3200000.0, 30300000.0,
            TrangThaiChiTra.DA_THANH_TOAN, new DateOnly(2024, 4, 5)));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(4, 2, "Luong co ban", LoaiKhoanMuc.CONG, 30000000.0, "22 ngay cong"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(5, 2, "Tien OT lam them", LoaiKhoanMuc.CONG, 2000000.0, "3 gio OT"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(6, 2, "Phu cap an trua", LoaiKhoanMuc.CONG, 1500000.0, "Thang 3"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(7, 2, "Bao hiem xa hoi", LoaiKhoanMuc.TRU, 3150000.0, "Trich nop BH"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(8, 2, "Phat di tre", LoaiKhoanMuc.TRU, 50000.0, "Di tre 2 lan"));

        dsBangLuong.Add(new BangLuong(3, 4, 3, 2024, 18200000.0, 1700000.0, 16500000.0,
            TrangThaiChiTra.CHO_DUYET, null));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(9, 3, "Luong co ban", LoaiKhoanMuc.CONG, 16000000.0, "Luong thoa thuan"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(10, 3, "Tien OT lam them", LoaiKhoanMuc.CONG, 2200000.0, "4 gio OT"));
        dsChiTietBangLuong.Add(new ChiTietBangLuong(11, 3, "Bao hiem xa hoi", LoaiKhoanMuc.TRU, 1700000.0, "Trich nop BH"));

        // ==========================================
        // 8. ĐÁNH GIÁ HIỆU SUẤT
        // ==========================================
        dsDanhGiaHieuSuat.Add(new DanhGiaHieuSuat(1, 1, 1, "Quy 1", 2024, 4.8, XepLoai.XUAT_SAC));
        dsDanhGiaHieuSuat.Add(new DanhGiaHieuSuat(2, 2, 1, "Quy 1", 2024, 4.2, XepLoai.TOT));
        dsDanhGiaHieuSuat.Add(new DanhGiaHieuSuat(3, 4, 2, "Quy 1", 2024, 3.1, XepLoai.TRUNG_BINH));
        dsDanhGiaHieuSuat.Add(new DanhGiaHieuSuat(4, 6, 2, "Quy 1", 2024, 1.5, XepLoai.YEU));

        dsChiTietDanhGia.Add(new ChiTietDanhGia(1, 1, "Ky nang quan ly du an", 0.5, 5.0, "Dung tien do vuot chi tieu"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(2, 1, "Chuyen can dung gio", 0.5, 4.6, "Guong mau dung gio 100%"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(3, 2, "Tien do cong viec", 0.6, 4.5, "Hoan thanh tot task"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(4, 2, "Chuyen can dung gio", 0.4, 3.75, "Co vai lan di tre"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(5, 3, "Chuyen can dung gio", 0.5, 2.0, "Hay di tre, can cai thien"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(6, 3, "Chat luong code", 0.5, 4.2, "Code tot"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(7, 4, "Tien do hoc tap", 0.5, 1.5, "Chua tap trung"));
        dsChiTietDanhGia.Add(new ChiTietDanhGia(8, 4, "Ky luat", 0.5, 1.5, "Can co gang hon"));
    }

    static DatabaseMock()
    {
        initData();
    }
}
