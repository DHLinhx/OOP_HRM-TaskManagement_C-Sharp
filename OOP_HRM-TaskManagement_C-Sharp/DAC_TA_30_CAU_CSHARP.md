# BẢNG ĐẶC TẢ 30 CÂU TRUY VẤN - PHIÊN BẢN C# (.NET 10 / LINQ)
## HỆ THỐNG QUẢN TRỊ NHÂN SỰ VÀ CÔNG VIỆC (HRM & TASK MANAGEMENT)
### Ngôn ngữ: C# 10 / .NET 10 | Công nghệ: LINQ & Lambdas | Kiến trúc: 3-Tier Layered

---

## MỤC LỤC TỔNG QUAN

| Nhóm câu | Phân hệ nghiệp vụ | Thành viên phụ trách | Lớp Service & DAO chính | Số lượng câu |
|---|---|---|---|:---:|
| **Câu 1 -> 10** | **Phân hệ 1:** Quản trị Nhân sự & Tổ chức | **Đặng Hoàng Lĩnh** (MSSV: 25162073) | `NhanSuService`, `NhanVienDAO`, `PhongBanDAO`, `ChucVuDAO`, `NhomNhanVienDAO`, `ThanhVienNhomDAO` | 10 câu |
| **Câu 11 -> 20** | **Phân hệ 2:** Vận hành & Quản lý Chấm công | **Nguyễn Sinh Dương** (MSSV: 25162019) | `VanHanhService`, `HopDongDAO`, `DieuKhoanHopDongDAO`, `CaLamViecDAO`, `ChamCongDAO`, `DonXinNghiDAO` | 10 câu |
| **Câu 21 -> 30** | **Phân hệ 3:** Quản lý Tài chính, Tiền lương & KPI | **Nguyễn Gia Hạo** (MSSV: 25162030) | `TaiChinhService`, `BangLuongDAO`, `ChiTietBangLuongDAO`, `DanhGiaHieuSuatDAO`, `ChiTietDanhGiaDAO` | 10 câu |

---

# PHẦN 1: ĐẶC TẢ 10 CÂU TRUY VẤN - PHÂN HỆ NHÂN SỰ & TỔ CHỨC
### Sinh viên phụ trách: ĐẶNG HOÀNG LĨNH (MSSV: 25162073)

---

### CÂU 1: IN DANH SÁCH TOÀN BỘ NHÂN VIÊN KÈM CHỨC VỤ, PHÒNG BAN VÀ CẤP TRÊN TRỰC TIẾP

* **Tên phương thức:** `DanhSachNhanVien()` (thuộc `NhanSuService`)
* **Mục tiêu nghiệp vụ:** Cung cấp báo cáo tổng quan toàn bộ nhân sự đang làm việc tại doanh nghiệp. Giải mã các mã định danh khóa ngoại (`maPhongBan`, `maChucVu`) thành tên hiển thị có ý nghĩa, đồng thời giải mã quan hệ tự tham chiếu (`maQuanLy`) để xác định người quản lý trực tiếp.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `gioiTinh`, `maPhongBan`, `maChucVu`, `maQuanLy`.
  * `PhongBan`: `tenPhongBan`.
  * `ChucVu`: `tenChucVu`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. *Nguồn:* Gọi `nhanVienDAO.getAll()` để lấy toàn bộ danh sách `List<NhanVien>`.
  2. *Duyệt:* Sử dụng `.forEach()` duyệt từng đối tượng `nv`.
  3. *Biến đổi:* Gọi các hàm helper: `getTenPhongBan(nv.getMaPhongBan())`, `getTenChucVu(nv.getMaChucVu())`.
  4. *Kiểm tra Null:* Kiểm tra `nv.getMaQuanLy() != null`: nếu có thì gọi `getTenNhanVien(maQL)`, ngược lại gán chuỗi `"Cấp cao nhất"`.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    nhanVienDAO.getAll().forEach(nv -> {
        String pb = getTenPhongBan(nv.getMaPhongBan());
        String cv = getTenChucVu(nv.getMaChucVu());
        String ql = nv.getMaQuanLy() != null ? getTenNhanVien(nv.getMaQuanLy()) : "Cấp cao nhất";
        System.out.printf("%-8d %-22s %-12s %-22s %-28s %-18s%n", 
                nv.getMaNhanVien(), nv.HoTenNV(), nv.getGioiTinh(), pb, cv, ql);
    });
    ```
    ```csharp
    nhanVienDAO.GetAll().ForEach(nv => {
        string pb = getTenPhongBan(nv.getMaPhongBan());
        string cv = getTenChucVu(nv.getMaChucVu());
        string ql = nv.getMaQuanLy().HasValue ? getTenNhanVien(nv.getMaQuanLy().Value) : "Cấp cao nhất";
        Console.WriteLine($"{nv.getMaNhanVien(),-8} {nv.HoTenNV(),-22} {nv.getGioiTinh(),-12} {pb,-22} {cv,-28} {ql,-18}");
    });
    ```

---

### CÂU 2: THỐNG KÊ CƠ CẤU TỔ CHỨC PHÒNG BAN VÀ SỐ LƯỢNG NHÂN SỰ TRỰC THUỘC

* **Tên phương thức:** `danhSachPhongBan()` (thuộc `NhanSuService`)
* **Mục tiêu nghiệp vụ:** Giúp ban lãnh đạo nắm bắt cơ cấu bộ máy tổ chức, nhận diện vị trí trưởng phòng của từng phòng ban và đo lường quy mô nhân sự hiện tại của từng đơn vị.
* **Thực thể & Thuộc tính tham gia:**
  * `PhongBan`: `maPhongBan`, `tenPhongBan`, `maTruongPhong`.
  * `NhanVien`: `maNhanVien`, `maPhongBan`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. *Nguồn:* Gọi `phongBanDAO.getAll()` để lấy danh sách phòng ban.
  2. *Xử lý trưởng phòng:* Nếu `pb.getmaTruongPhong() != null` thì tra cứu tên trưởng phòng qua `getTenNhanVien()`, ngược lại hiển thị `"Chưa có"`.
  3. *Đếm số lượng nhân viên:* Gọi `nhanVienDAO.getByPhongBan(pb.getmaPhongBan())` rồi lấy kích thước danh sách (`.size()` trong Java, `.Count` trong C#).
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    phongBanDAO.getAll().forEach(pb -> {
        String tp = pb.getmaTruongPhong() != null ? getTenNhanVien(pb.getmaTruongPhong()) : "Chưa có";
        long count = nhanVienDAO.getByPhongBan(pb.getmaPhongBan()).size();
        System.out.printf("%-20s %-25s %-10d%n", pb.gettenPhongBan(), tp, count);
    });
    ```
    ```csharp
    phongBanDAO.GetAll().ForEach(pb => {
        string tp = pb.getmaTruongPhong().HasValue ? getTenNhanVien(pb.getmaTruongPhong().Value) : "Chưa có";
        int count = nhanVienDAO.GetByPhongBan(pb.getmaPhongBan()).Count;
        Console.WriteLine($"{pb.gettenPhongBan(),-20} {tp,-25} {count,-10}");
    });
    ```

---

### CÂU 3: TRA CỨU DANH SÁCH NHÂN VIÊN THEO PHÒNG BAN (MÃ HOẶC TÊN)

* **Tên phương thức:** `traCuuNhanVienTheoPhongBan(String maHoacTenPhongBan)`
* **Mục tiêu nghiệp vụ:** Cho phép người dùng nhập từ bàn phím mã phòng ban (ví dụ: `"1"`) hoặc tên phòng ban (ví dụ: `"Cong nghe"`) để lọc danh sách các nhân viên đang công tác tại phòng ban đó (tìm kiếm không phân biệt hoa thường).
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `maPhongBan`, `maChucVu`.
  * `PhongBan`: `maPhongBan`, `tenPhongBan`.
* **Dữ liệu đầu vào:** Chuỗi `maHoacTenPhongBan` (kiểm tra `null` hoặc rỗng).
* **Luồng xử lý (Pipeline Logic):**
  1. Kiểm tra đầu vào: Nếu rỗng thì thông báo yêu cầu nhập.
  2. Mở Stream trên toàn bộ nhân viên: `nhanVienDAO.getAll().stream()`.
  3. Lọc bằng `.filter()`: So sánh mã phòng ban (chuyển sang chuỗi bằng `equalsIgnoreCase`) HOẶC tên phòng ban chứa từ khóa (`.toLowerCase().contains()`).
  4. Gom thành danh sách bằng `.collect(Collectors.toList())`.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    List<NhanVien> ds = nhanVienDAO.getAll().stream()
        .filter(nv -> String.valueOf(nv.getMaPhongBan()).equalsIgnoreCase(keyword)
                || getTenPhongBan(nv.getMaPhongBan()).toLowerCase().contains(keyword))
        .collect(Collectors.toList());
    ds.forEach(nv -> System.out.printf("- %s | %s | %s | %s%n",
        nv.getMaNhanVien(), nv.HoTenNV(), getTenChucVu(nv.getMaChucVu()), getTenPhongBan(nv.getMaPhongBan())));
    ```
    ```csharp
    var ds = nhanVienDAO.GetAll()
        .Where(nv => nv.getMaPhongBan().ToString().Equals(keyword, StringComparison.OrdinalIgnoreCase)
                || getTenPhongBan(nv.getMaPhongBan()).ToLower().Contains(keyword)).ToList();
    ds.ForEach(nv => Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())} | {getTenPhongBan(nv.getMaPhongBan())}"));
    ```

---

### CÂU 4: TOP 3 NHÂN VIÊN CÓ THÂM NIÊN CÔNG TÁC CAO NHẤT TOÀN CÔNG TY

* **Tên phương thức:** `top3NhanVienThamNien()` (gọi `nhanVienDAO.getTopThamNien(3)`)
* **Mục tiêu nghiệp vụ:** Vinh danh và tri ân các nhân viên gắn bó lâu năm nhất với doanh nghiệp, sắp xếp theo thuộc tính ngày vào làm việc (`ngayVaoLam`) sớm nhất.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `ngayVaoLam`.
* **Dữ liệu đầu vào:** `limit = 3`.
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: `DatabaseMock.dsNhanVien.stream()`.
  2. Sắp xếp tăng dần theo ngày vào làm: `.sorted(Comparator.comparing(NhanVien::getNgayVaoLam))`.
  3. Cắt lấy đúng 3 bản ghi đầu tiên: `.limit(3)`.
  4. Thu hoạch kết quả: `.collect(Collectors.toList())`.
  5. Tầng Service: Nhận danh sách và in ra console.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    // Tầng DAO:
    public List<NhanVien> getTopThamNien(int limit) {
        return DatabaseMock.dsNhanVien.stream()
                .sorted(Comparator.comparing(NhanVien::getNgayVaoLam))
                .limit(limit)
                .collect(Collectors.toList());
    }
    // Tầng Service:
    nhanVienDAO.getTopThamNien(3).forEach(nv -> 
        System.out.printf("- %s | %s | %s%n", nv.getMaNhanVien(), nv.HoTenNV(), nv.getNgayVaoLam()));
    ```
    ```csharp
    // Tầng DAO:
    public List<NhanVien> GetTopThamNien(int limit) =>
        DatabaseMock.dsNhanVien.OrderBy(nv => nv.getNgayVaoLam()).Take(limit).ToList();
    // Tầng Service:
    nhanVienDAO.GetTopThamNien(3).ForEach(nv => 
        Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getNgayVaoLam()}"));
    ```

---

### CÂU 5: CÂY PHÂN CẤP QUẢN LÝ - DANH SÁCH CẤP DƯỚI CỦA MỘT LÃNH ĐẠO

* **Tên phương thức:** `danhSachCapDuoiCuaQuanLy(int maNhanVienQuanLy)`
* **Mục tiêu nghiệp vụ:** Báo cáo cơ cấu nhân sự cấp dưới trực thuộc một người quản lý cụ thể. Phục vụ công tác ủy quyền, giao việc và đánh giá quản lý.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `maChucVu`, `maQuanLy` (Self-reference FK).
* **Dữ liệu đầu vào:** `maNhanVienQuanLy` (số nguyên hợp lệ).
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: Mở luồng danh sách nhân viên.
  2. Lọc bằng `.filter()`: Điều kiện `nv.getMaQuanLy() != null && nv.getMaQuanLy() == maQL`.
  3. Thu hoạch: `.collect(Collectors.toList())`.
  4. Tầng Service: Kiểm tra nếu danh sách rỗng thì thông báo `"Khong co cap duoi nao."`; nếu có thì duyệt in Mã NV, Họ tên và Chức vụ.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    List<NhanVien> dsCapDuoi = nhanVienDAO.getCapDuoi(maNhanVienQuanLy);
    dsCapDuoi.forEach(nv -> System.out.printf("- %s | %s | %s%n",
            nv.getMaNhanVien(), nv.HoTenNV(), getTenChucVu(nv.getMaChucVu())));
    ```
    ```csharp
    var dsCapDuoi = nhanVienDAO.GetCapDuoi(maNhanVienQuanLy);
    dsCapDuoi.ForEach(nv => Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenChucVu(nv.getMaChucVu())}"));
    ```

---

### CÂU 6: CHI TIẾT MỘT NHÓM LÀM VIỆC VÀ DANH SÁCH THÀNH VIÊN DỰ ÁN

* **Tên phương thức:** `chiTietNhomNhanVien(int maNhom)`
* **Mục tiêu nghiệp vụ:** Hiển thị hồ sơ của một nhóm/dự án công tác (tên nhóm, trưởng nhóm, ngày thành lập) và kết nối bảng nhiều-nhiều `ThanhVienNhom` để in đầy đủ danh sách thành viên cùng vai trò đảm nhiệm.
* **Thực thể & Thuộc tính tham gia:**
  * `NhomNhanVien`: `maNhom`, `tenNhom`, `maTruongNhom`, `ngayThanhLap`.
  * `ThanhVienNhom`: `maNhom`, `nhomNhanVien` (FK trỏ tới NhanVien), `vaiTro`, `ngayThamGia`.
  * `NhanVien`: `maNhanVien`, `ho`, `ten`.
* **Dữ liệu đầu vào:** `maNhom` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Tra cứu nhóm: `nhomNhanVienDAO.getById(maNhom)`. Nếu không thấy thì kết thúc.
  2. Lấy danh sách thành viên: `thanhVienNhomDAO.getByNhom(maNhom)`.
  3. Duyệt danh sách thành viên bằng `.forEach()`, với mỗi thành viên gọi `nhanVienDAO.getById()` để trích xuất Họ tên và in cùng vai trò trong dự án.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    thanhVienNhomDAO.getByNhom(maNhom).forEach(tv -> {
        NhanVien nv = nhanVienDAO.getById(tv.getnhomNhanVien()).orElse(null);
        if (nv != null) {
            System.out.printf("- %s | %s | Vai trò: %s | Ngày tham gia: %s%n",
                    nv.getMaNhanVien(), nv.HoTenNV(), tv.getVaiTro(), tv.getngayThamGia());
        }
    });
    ```
    ```csharp
    thanhVienNhomDAO.GetByNhom(maNhom).ForEach(tv => {
        var nv = nhanVienDAO.GetById(tv.getnhomNhanVien());
        if (nv != null) {
            Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | Vai trò: {tv.getVaiTro()} | Ngày tham gia: {tv.getngayThamGia()}");
        }
    });
    ```

---

### CÂU 7: THỐNG KÊ NHÂN SỰ ĐA NHIỆM (THAM GIA TỪ 2 NHÓM DỰ ÁN TRỞ LÊN)

* **Tên phương thức:** `thongKeNhanVienThamGiaNhieuNhom()`
* **Mục tiêu nghiệp vụ:** Phát hiện các nhân sự chủ chốt (Key Persons) đang phải kiêm nhiệm nhiều ban dự án cùng lúc ($ge 2$ nhóm) để có phương án cân bằng tải công việc và khen thưởng xứng đáng.
* **Thực thể & Thuộc tính tham gia:**
  * `ThanhVienNhom`: `nhomNhanVien`, `maNhom`.
  * `NhanVien`: `maNhanVien`, `ho`, `ten`.
  * `NhomNhanVien`: `tenNhom`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Mở Stream trên toàn bộ bảng `ThanhVienNhom`.
  2. Gom nhóm theo mã nhân viên: `.collect(Collectors.groupingBy(ThanhVienNhom::getnhomNhanVien))`.
  3. Mở Stream trên tập kết quả (`entrySet().stream()`).
  4. Lọc những người có số lượng nhóm tham gia: `.filter(entry -> entry.getValue().size() >= 2)`.
  5. Nối tên các nhóm bằng `Collectors.joining(", ")` và in ra.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    thanhVienNhomDAO.getAll().stream()
        .collect(Collectors.groupingBy(ThanhVienNhom::getnhomNhanVien))
        .entrySet().stream()
        .filter(entry -> entry.getValue().size() >= 2)
        .forEach(entry -> {
            String ten = getTenNhanVien(entry.getKey());
            String dsNhom = entry.getValue().stream().map(tv -> getTenNhom(tv.getmaNhom())).collect(Collectors.joining(", "));
            System.out.printf("- %s: %s (%d nhóm)%n", ten, dsNhom, entry.getValue().size());
        });
    ```
    ```csharp
    thanhVienNhomDAO.GetAll().GroupBy(tv => tv.getnhomNhanVien())
        .Where(g => g.Count() >= 2).ToList()
        .ForEach(entry => {
            string ten = getTenNhanVien(entry.Key);
            string dsNhom = string.Join(", ", entry.Select(tv => getTenNhom(tv.getmaNhom())));
            Console.WriteLine($"- {ten}: {dsNhom} ({entry.Count()} nhóm)");
        });
    ```

---

### CÂU 8: THỐNG KÊ PHÂN BỔ NHÂN SỰ THEO TỪNG VỊ TRÍ CHỨC VỤ

* **Tên phương thức:** `thongKeNhanVienTheoChucVu()`
* **Mục tiêu nghiệp vụ:** Đo lường cơ cấu nguồn nhân lực theo từng vị trí công việc (Developer, Tester, BA, Project Manager, Director...) phục vụ hoạch định tuyển dụng và đào tạo.
* **Thực thể & Thuộc tính tham gia:**
  * `ChucVu`: `maChucVu`, `tenChucVu`.
  * `NhanVien`: `maChucVu`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Duyệt qua từng chức vụ trong `chucVuDAO.getAll()`.
  2. Với mỗi chức vụ, đếm số lượng nhân viên bằng: `nhanVienDAO.getAll().stream().filter(nv -> nv.getMaChucVu() == cv.getmaChucVu()).count()`.
  3. In kết quả chức vụ và số lượng nhân sự tương ứng.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chucVuDAO.getAll().forEach(cv -> {
        long count = nhanVienDAO.getAll().stream().filter(nv -> nv.getMaChucVu() == cv.getmaChucVu()).count();
        System.out.printf("%-30s Số lượng NV: %d%n", cv.gettenChucVu(), count);
    });
    ```
    ```csharp
    chucVuDAO.GetAll().ForEach(cv => {
        int count = nhanVienDAO.GetAll().Count(nv => nv.getMaChucVu() == cv.getmaChucVu());
        Console.WriteLine($"{cv.gettenChucVu(),-30} Số lượng NV: {count}");
    });
    ```

---

### CÂU 9: DANH SÁCH NHÂN VIÊN CHƯA THAM GIA BẤT KỲ NHÓM DỰ ÁN NÀO

* **Tên phương thức:** `danhSachNhanVienChuaThamGiaNhom()`
* **Mục tiêu nghiệp vụ:** Nhận diện nguồn lực nhàn rỗi hoặc các nhân viên mới vào làm việc chưa được phân bổ vào các nhóm/dự án cụ thể để kịp thời giao nhiệm vụ.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `maPhongBan`.
  * `ThanhVienNhom`: `nhomNhanVien`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Mở Stream trên toàn bộ nhân viên: `nhanVienDAO.getAll().stream()`.
  2. Lọc bằng `.filter()`: Điều kiện `thanhVienNhomDAO.getByNhanVien(nv.getMaNhanVien()).isEmpty()` (danh sách nhóm rỗng).
  3. Thu hoạch: `.collect(Collectors.toList())`.
  4. Duyệt in danh sách nhân viên chưa có nhóm.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    List<NhanVien> dsChuaVaoNhom = nhanVienDAO.getAll().stream()
        .filter(nv -> thanhVienNhomDAO.getByNhanVien(nv.getMaNhanVien()).isEmpty())
        .collect(Collectors.toList());
    dsChuaVaoNhom.forEach(nv -> System.out.printf("- %s | %s | %s%n",
            nv.getMaNhanVien(), nv.HoTenNV(), getTenPhongBan(nv.getMaPhongBan())));
    ```
    ```csharp
    var dsChuaVaoNhom = nhanVienDAO.GetAll()
        .Where(nv => thanhVienNhomDAO.GetByNhanVien(nv.getMaNhanVien()).Count == 0).ToList();
    dsChuaVaoNhom.ForEach(nv => Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {getTenPhongBan(nv.getMaPhongBan())}"));
    ```

---

### CÂU 10: TÌM KIẾM NHÂN SỰ ĐA TIÊU CHÍ THEO TỪ KHÓA LINH HOẠT

* **Tên phương thức:** `timNhanSuTheoTuKhoa(String keyword)` (gọi `nhanVienDAO.search(keyword)`)
* **Mục tiêu nghiệp vụ:** Cung cấp thanh công cụ tìm kiếm thông minh (Global Search): Một từ khóa duy nhất do người dùng nhập vào có thể khớp đồng thời với Họ tên, Số điện thoại hoặc Email của nhân viên.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `soDienThoai`, `email`, `maPhongBan`.
* **Dữ liệu đầu vào:** Chuỗi `keyword` (nhập từ bàn phím).
* **Luồng xử lý (Pipeline Logic):**
  1. Kiểm tra `keyword` rỗng: Nếu rỗng thì yêu cầu nhập lại.
  2. Chuẩn hóa: `String k = keyword.toLowerCase().trim()`.
  3. Tầng DAO: `DatabaseMock.dsNhanVien.stream().filter()`.
  4. Vị từ khớp: `nv.HoTenNV().toLowerCase().contains(k) || nv.getSoDienThoai().contains(k) || nv.getEmail().toLowerCase().contains(k)`.
  5. Thu hoạch và in kết quả.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    // Tầng DAO:
    public List<NhanVien> search(String kw) {
        String k = kw.toLowerCase().trim();
        return DatabaseMock.dsNhanVien.stream()
            .filter(nv -> nv.HoTenNV().toLowerCase().contains(k) 
                    || nv.getSoDienThoai().contains(k) 
                    || nv.getEmail().toLowerCase().contains(k))
            .collect(Collectors.toList());
    }
    // Tầng Service:
    nhanVienDAO.search(keyword).forEach(nv -> System.out.printf("- %s | %s | %s | %s | %s%n",
            nv.getMaNhanVien(), nv.HoTenNV(), nv.getSoDienThoai(), nv.getEmail(), getTenPhongBan(nv.getMaPhongBan())));
    ```
    ```csharp
    // Tầng DAO:
    public List<NhanVien> Search(string kw) {
        string k = kw.ToLower().Trim();
        return DatabaseMock.dsNhanVien
            .Where(nv => nv.HoTenNV().ToLower().Contains(k) 
                    || nv.getSoDienThoai().Contains(k) 
                    || nv.getEmail().ToLower().Contains(k)).ToList();
    }
    // Tầng Service:
    nhanVienDAO.Search(keyword).ForEach(nv => 
        Console.WriteLine($"- {nv.getMaNhanVien()} | {nv.HoTenNV()} | {nv.getSoDienThoai()} | {nv.getEmail()} | {getTenPhongBan(nv.getMaPhongBan())}"));
    ```

---

# PHẦN 2: ĐẶC TẢ 10 CÂU TRUY VẤN - PHÂN HỆ VẬN HÀNH & CHẤM CÔNG
### Sinh viên phụ trách: NGUYỄN SINH DƯƠNG (MSSV: 25162019)

---

### CÂU 11: BẢN IN CHI TIẾT HỢP ĐỒNG LAO ĐỘNG VÀ ĐIỀU KHOẢN PHÁP LÝ ĐÍNH KÈM

* **Tên phương thức:** `inHopDongChiTiet(int maHopDong)` (thuộc `VanHanhService`)
* **Mục tiêu nghiệp vụ:** Trích xuất chi tiết một hợp đồng lao động theo mã định danh, nạp thông tin nhân sự ký kết và duyệt toàn bộ các điều khoản pháp lý đính kèm trong phụ lục hợp đồng.
* **Thực thể & Thuộc tính tham gia:**
  * `HopDong`: `maHopDong`, `maNhanVien`, `loaiHopDong`, `ngayBatDau`, `ngayKetThuc`, `luongThoaThuan`.
  * `DieuKhoanHopDong`: `soThuTuDieu`, `tieuDeDieuKhoan`, `noiDungDieuKhoan`, `loaiDieuKhoan`.
* **Dữ liệu đầu vào:** `maHopDong` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Tra cứu hợp đồng: `hopDongDAO.getById(maHopDong)`. Nếu không thấy, báo lỗi.
  2. Lấy danh sách điều khoản: `dieuKhoanHopDongDAO.getByHopDong(maHopDong)`.
  3. Duyệt bằng `.forEach()` để in từng điều khoản kèm số thứ tự và tiêu đề.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    HopDong hd = hopDongDAO.getById(maHopDong).orElse(null);
    if (hd != null) {
        System.out.printf("HỢP ĐỒNG SỐ: %d | Nhân viên: %s | Loại: %s | Lương: %,.0f VNĐ%n",
                hd.getMaHopDong(), getTenNhanVien(hd.getMaNhanVien()), hd.getLoaiHopDong(), hd.getLuongThoaThuan());
        dieuKhoanHopDongDAO.getByHopDong(maHopDong).forEach(dk -> 
            System.out.printf("  Điều %d [%s]: %s%n", dk.getSoThuTu(), dk.getTieuDe(), dk.getNoiDung()));
    }
    ```
    ```csharp
    var hd = hopDongDAO.GetById(maHopDong);
    if (hd != null) {
        Console.WriteLine($"HỢP ĐỒNG SỐ: {hd.getMaHopDong()} | Nhân viên: {getTenNhanVien(hd.getMaNhanVien())} | Loại: {hd.getLoaiHopDong()} | Lương: {hd.getLuongThoaThuan():N0} VNĐ");
        dieuKhoanHopDongDAO.GetByHopDong(maHopDong).ForEach(dk => 
            Console.WriteLine($"  Điều {dk.getSoThuTu()} [{dk.getTieuDe()}]: {dk.getNoiDung()}"));
    }
    ```

---

### CÂU 12: CẢNH BÁO DANH SÁCH HỢP ĐỒNG SẮP HẾT HẠN TRONG VÒNG 30 NGÀY

* **Tên phương thức:** `canhBaoHopDongSapHetHan()`
* **Mục tiêu nghiệp vụ:** Tự động rà soát kho dữ liệu để phát hiện các hợp đồng lao động sẽ đáo hạn trong vòng 30 ngày tính từ mốc kiểm tra, hỗ trợ phòng nhân sự chuẩn bị thủ tục tái ký hoặc thanh lý hợp đồng.
* **Thực thể & Thuộc tính tham gia:**
  * `HopDong`: `maHopDong`, `maNhanVien`, `ngayKetThuc`, `trangThai`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** Mốc ngày kiểm tra (`LocalDate ngayKiemTra`).
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: `DatabaseMock.dsHopDong.stream()`.
  2. Lọc: `hd.getNgayKetThuc() != null && !hd.getNgayKetThuc().isBefore(ngayKiemTra) && !hd.getNgayKetThuc().isAfter(ngayGioiHan)`.
  3. Tầng Service: Tính khoảng cách số ngày còn lại bằng `ChronoUnit.DAYS.between()` và in bảng cảnh báo.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    hopDongDAO.getSapHetHan(ngayKiemTra, 30).forEach(hd -> {
        long soNgayConLai = ChronoUnit.DAYS.between(ngayKiemTra, hd.getNgayKetThuc());
        System.out.printf("HĐ: %-6d NV: %-22s Ngày hết hạn: %s (Còn %d ngày)%n",
                hd.getMaHopDong(), getTenNhanVien(hd.getMaNhanVien()), hd.getNgayKetThuc(), soNgayConLai);
    });
    ```
    ```csharp
    hopDongDAO.GetSapHetHan(ngayKiemTra, 30).ForEach(hd => {
        int soNgayConLai = hd.getNgayKetThuc().DayNumber - ngayKiemTra.DayNumber;
        Console.WriteLine($"HĐ: {hd.getMaHopDong(),-6} NV: {getTenNhanVien(hd.getMaNhanVien()),-22} Ngày hết hạn: {hd.getNgayKetThuc()} (Còn {soNgayConLai} ngày)");
    });
    ```

---

### CÂU 13: BẢNG CHẤM CÔNG CÁ NHÂN THEO THÁNG/NĂM CỦA MỘT NHÂN VIÊN

* **Tên phương thức:** `bangChamCongCaNhan(int maNhanVien, int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Xuất lịch sử quẹt thẻ chi tiết từng ngày công trong tháng của nhân viên: Giờ vào, Giờ ra, Trạng thái đi làm (ĐÚNG GIỜ, ĐI TRỄ, VỀ SỚM) và Số giờ làm thêm ngoài giờ (OT).
* **Thực thể & Thuộc tính tham gia:**
  * `ChamCong`: `maNhanVien`, `ngayChamCong`, `gioVao`, `gioRa`, `trangThaiCong`, `soGioLamThem`.
* **Dữ liệu đầu vào:** `maNhanVien` (int), `thang` (1-12), `nam` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: `chamCongDAO.getByNhanVienVaThang(maNV, thang, nam)`.
  2. Lọc Stream: Khớp `maNhanVien`, tháng (`getMonthValue()`) và năm (`getYear()`).
  3. Tầng Service: Duyệt danh sách bằng `.forEach()` và in bảng chấm công.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chamCongDAO.getByNhanVienVaThang(maNV, thang, nam).forEach(cc -> 
        System.out.printf("%s | Vào: %s | Ra: %s | Trạng thái: %-18s | OT: %.1f giờ%n",
                cc.getNgayChamCong(), cc.getGioVao(), cc.getGioRa(), cc.getTrangThaiCong(), cc.getSoGioLamThem()));
    ```
    ```csharp
    chamCongDAO.GetByNhanVienVaThang(maNV, thang, nam).ForEach(cc => 
        Console.WriteLine($"{cc.getNgayChamCong()} | Vào: {cc.getGioVao()} | Ra: {cc.getGioRa()} | Trạng thái: {cc.getTrangThaiCong(),-18} | OT: {cc.getSoGioLamThem():F1} giờ"));
    ```

---

### CÂU 14: BÁO CÁO XẾP HẠNG NHÂN VIÊN ĐI TRỄ TRONG THÁNG (TOP ĐI TRỄ)

* **Tên phương thức:** `topNhanVienDiTre(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Thống kê và xếp hạng các nhân viên có số lần vi phạm trạng thái `DI_TRE` nhiều nhất trong tháng, hỗ trợ công tác chấn chỉnh kỷ luật lao động.
* **Thực thể & Thuộc tính tham gia:**
  * `ChamCong`: `maNhanVien`, `ngayChamCong`, `trangThaiCong`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Mở Stream trên bảng `ChamCong`.
  2. Lọc: Đúng tháng, đúng năm VÀ `trangThaiCong == TrangThaiCong.DI_TRE`.
  3. Gom nhóm và đếm: `.collect(Collectors.groupingBy(ChamCong::getMaNhanVien, Collectors.counting()))`.
  4. Mở Stream trên Map, sắp xếp giảm dần theo số lần vi phạm: `.sorted((e1, e2) -> Long.compare(e2.getValue(), e1.getValue()))`.
  5. In danh sách xếp hạng.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chamCongDAO.getAll().stream()
        .filter(cc -> cc.getNgayChamCong().getMonthValue() == thang && cc.getNgayChamCong().getYear() == nam && cc.getTrangThaiCong() == TrangThaiCong.DI_TRE)
        .collect(Collectors.groupingBy(ChamCong::getMaNhanVien, Collectors.counting()))
        .entrySet().stream()
        .sorted((e1, e2) -> Long.compare(e2.getValue(), e1.getValue()))
        .forEach(e -> System.out.printf("Mã NV: %-6d Họ tên: %-22s Số lần đi trễ: %d lần%n", 
                e.getKey(), getTenNhanVien(e.getKey()), e.getValue()));
    ```
    ```csharp
    chamCongDAO.GetAll()
        .Where(cc => cc.getNgayChamCong().Month == thang && cc.getNgayChamCong().Year == nam && cc.getTrangThaiCong() == TrangThaiCong.DI_TRE)
        .GroupBy(cc => cc.getMaNhanVien())
        .Select(g => new { MaNV = g.Key, SoLan = g.Count() })
        .OrderByDescending(x => x.SoLan).ToList()
        .ForEach(x => Console.WriteLine($"Mã NV: {x.MaNV,-6} Họ tên: {getTenNhanVien(x.MaNV),-22} Số lần đi trễ: {x.SoLan} lần"));
    ```

---

### CÂU 15: BÁO CÁO TOP NHÂN VIÊN LÀM THÊM GIỜ (OT) NHIỀU NHẤT TRONG THÁNG

* **Tên phương thức:** `topNhanVienLamThemGio(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Tính tổng số giờ làm thêm ngoài giờ (`soGioLamThem`) của từng nhân sự trong tháng, xếp hạng giảm dần làm cơ sở tính tiền phụ cấp OT và khen thưởng cống hiến.
* **Thực thể & Thuộc tính tham gia:**
  * `ChamCong`: `maNhanVien`, `ngayChamCong`, `soGioLamThem`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Lọc bảng `ChamCong`: Đúng tháng, năm VÀ `soGioLamThem > 0`.
  2. Gom nhóm theo nhân viên và tính tổng giờ: `.collect(Collectors.groupingBy(ChamCong::getMaNhanVien, Collectors.summingDouble(ChamCong::getSoGioLamThem)))`.
  3. Sắp xếp giảm dần theo tổng giờ OT.
  4. Duyệt in danh sách nhân viên cống hiến OT.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chamCongDAO.getAll().stream()
        .filter(cc -> cc.getNgayChamCong().getMonthValue() == thang && cc.getNgayChamCong().getYear() == nam && cc.getSoGioLamThem() > 0)
        .collect(Collectors.groupingBy(ChamCong::getMaNhanVien, Collectors.summingDouble(ChamCong::getSoGioLamThem)))
        .entrySet().stream()
        .sorted((e1, e2) -> Double.compare(e2.getValue(), e1.getValue()))
        .forEach(e -> System.out.printf("Mã NV: %-6d Họ tên: %-22s Tổng giờ OT: %.1f giờ%n", 
                e.getKey(), getTenNhanVien(e.getKey()), e.getValue()));
    ```
    ```csharp
    chamCongDAO.GetAll()
        .Where(cc => cc.getNgayChamCong().Month == thang && cc.getNgayChamCong().Year == nam && cc.getSoGioLamThem() > 0)
        .GroupBy(cc => cc.getMaNhanVien())
        .Select(g => new { MaNV = g.Key, TongOT = g.Sum(c => c.getSoGioLamThem()) })
        .OrderByDescending(x => x.TongOT).ToList()
        .ForEach(x => Console.WriteLine($"Mã NV: {x.MaNV,-6} Họ tên: {getTenNhanVien(x.MaNV),-22} Tổng giờ OT: {x.TongOT:F1} giờ"));
    ```

---

### CÂU 16: DANH SÁCH ĐƠN XIN NGHỈ PHÉP ĐANG CHỜ PHÊ DUYỆT

* **Tên phương thức:** `danhSachDonNghiChoDuyet()`
* **Mục tiêu nghiệp vụ:** Giúp cấp quản lý và phòng nhân sự theo dõi toàn bộ các đơn xin nghỉ phép có trạng thái `CHO_DUYET` để kịp thời xem xét, phê duyệt hoặc từ chối.
* **Thực thể & Thuộc tính tham gia:**
  * `DonXinNghi`: `maDonNghi`, `maNhanVien`, `loaiNghi`, `ngayBatDau`, `ngayKetThuc`, `lyDo`, `trangThaiDuyet`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: `DatabaseMock.dsDonXinNghi.stream().filter(d -> d.getTrangThaiDuyet() == TrangThaiDuyet.CHO_DUYET).collect(...)`.
  2. Tầng Service: Kiểm tra rỗng, nếu có đơn thì in thông tin chi tiết từng đơn nghỉ phép.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    donXinNghiDAO.getChoDuyet().forEach(d -> 
        System.out.printf("Đơn #%-4d | NV: %-22s | Loại: %-18s | Từ %s đến %s | Lý do: %s%n",
                d.getMaDon(), getTenNhanVien(d.getMaNhanVien()), d.getLoaiNghi(), d.getNgayBatDau(), d.getNgayKetThuc(), d.getLyDo()));
    ```
    ```csharp
    donXinNghiDAO.GetChoDuyet().ForEach(d => 
        Console.WriteLine($"Đơn #{d.getMaDon(),-4} | NV: {getTenNhanVien(d.getMaNhanVien()),-22} | Loại: {d.getLoaiNghi(),-18} | Từ {d.getNgayBatDau()} đến {d.getNgayKetThuc()} | Lý do: {d.getLyDo()}"));
    ```

---

### CÂU 17: LỊCH SỬ NGHỈ PHÉP CỦA MỘT NHÂN VIÊN CỤ THỂ

* **Tên phương thức:** `lichSuNghiPhep(int maNhanVien)`
* **Mục tiêu nghiệp vụ:** Tra cứu toàn bộ các đợt xin nghỉ phép trong năm của một nhân sự (nghỉ phép năm, nghỉ ốm, nghỉ không lương...) kèm trạng thái duyệt (`DA_DUYET`, `CHO_DUYET`, `TU_CHOI`).
* **Thực thể & Thuộc tính tham gia:**
  * `DonXinNghi`: `maDonNghi`, `maNhanVien`, `loaiNghi`, `ngayBatDau`, `ngayKetThuc`, `trangThaiDuyet`.
* **Dữ liệu đầu vào:** `maNhanVien` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Gọi `donXinNghiDAO.getByNhanVien(maNhanVien)`.
  2. Lọc bằng Stream: `.filter(d -> d.getMaNhanVien() == maNhanVien)`.
  3. In lịch sử chi tiết ra console.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    donXinNghiDAO.getByNhanVien(maNV).forEach(d -> 
        System.out.printf("Đơn #%-4d | Loại: %-18s | Từ %s đến %s | Trạng thái: %s%n",
                d.getMaDon(), d.getLoaiNghi(), d.getNgayBatDau(), d.getNgayKetThuc(), d.getTrangThaiDuyet()));
    ```
    ```csharp
    donXinNghiDAO.GetByNhanVien(maNV).ForEach(d => 
        Console.WriteLine($"Đơn #{d.getMaDon(),-4} | Loại: {d.getLoaiNghi(),-18} | Từ {d.getNgayBatDau()} đến {d.getNgayKetThuc()} | Trạng thái: {d.getTrangThaiDuyet()}"));
    ```

---

### CÂU 18: THỐNG KÊ NHÂN VIÊN ĐẠT TIÊU CHUẨN CHUYÊN CẦN 100% TRONG THÁNG

* **Tên phương thức:** `nhanVienChuyenCan(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Tôn vinh các nhân sự gương mẫu chấp hành kỷ luật lao động: Đi làm đầy đủ trong tháng, không có bất kỳ lần đi trễ, về sớm hay vắng mặt nào.
* **Thực thể & Thuộc tính tham gia:**
  * `NhanVien`: `maNhanVien`, `ho`, `ten`, `maPhongBan`.
  * `ChamCong`: `maNhanVien`, `ngayChamCong`, `trangThaiCong`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO `isChuyenCan100PhanTram(maNV, thang, nam)`:
     - Kiểm tra có ít nhất 1 bản ghi chấm công trong tháng (`!ds.isEmpty()`).
     - Kiểm tra không có bất kỳ bản ghi nào khác `DUNG_GIO`: `ds.stream().allMatch(c -> c.getTrangThaiCong() == TrangThaiCong.DUNG_GIO)`.
  2. Tầng Service: Lọc toàn bộ nhân viên thỏa mãn điều kiện chuyên cần và in ra.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    nhanVienDAO.getAll().stream()
        .filter(nv -> chamCongDAO.isChuyenCan100PhanTram(nv.getMaNhanVien(), thang, nam))
        .forEach(nv -> System.out.printf("CHUẨN MỰC CHUYÊN CẦN: %-6d %-22s %s%n",
                nv.getMaNhanVien(), nv.HoTenNV(), getTenPhongBan(nv.getMaPhongBan())));
    ```
    ```csharp
    nhanVienDAO.GetAll()
        .Where(nv => chamCongDAO.IsChuyenCan100PhanTram(nv.getMaNhanVien(), thang, nam)).ToList()
        .ForEach(nv => Console.WriteLine($"CHUẨN MỰC CHUYÊN CẦN: {nv.getMaNhanVien(),-6} {nv.HoTenNV(),-22} {getTenPhongBan(nv.getMaPhongBan())}"));
    ```

---

### CÂU 19: THỐNG KÊ SỐ LƯỢNG NHÂN VIÊN THEO TỪNG LOẠI HỢP ĐỒNG LAO ĐỘNG

* **Tên phương thức:** `thongKeTheoLoaiHopDong()`
* **Mục tiêu nghiệp vụ:** Phân tích tỷ trọng cơ cấu lao động theo hình thức hợp đồng (Thử việc, Xác định thời hạn, Vô thời hạn), phục vụ công tác tuân thủ pháp lý và xây dựng quỹ phúc lợi.
* **Thực thể & Thuộc tính tham gia:**
  * `HopDong`: `loaiHopDong`.
  * Danh mục Enum: `LoaiHopDong` (THU_VIEC, XAC_DINH_THOI_HAN, VO_THOI_HAN).
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Duyệt qua từng giá trị trong Enum: `LoaiHopDong.values()`.
  2. Với mỗi loại hợp đồng, gọi DAO để đếm số bản ghi hợp đồng tương ứng: `hopDongDAO.getByLoai(loai).size()`.
  3. In bảng thống kê tỷ trọng hợp đồng.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    for (LoaiHopDong loai : LoaiHopDong.values()) {
        long count = hopDongDAO.getByLoai(loai).size();
        System.out.printf("Loại hợp đồng %-25s: %d người%n", loai, count);
    }
    ```
    ```csharp
    foreach (LoaiHopDong loai in Enum.GetValues<LoaiHopDong>()) {
        int count = hopDongDAO.GetByLoai(loai).Count;
        Console.WriteLine($"Loại hợp đồng {loai,-25}: {count} người");
    }
    ```

---

### CÂU 20: LỌC CÁC ĐIỀU KHOẢN PHÁP LÝ MANG TÍNH CHẤT BẢO MẬT HOẶC KỶ LUẬT

* **Tên phương thức:** `locDieuKhoanBaoMatVaKyLuat()`
* **Mục tiêu nghiệp vụ:** Rà soát các điều khoản trọng yếu trong toàn bộ phụ lục hợp đồng nhằm phục vụ thanh tra pháp chế doanh nghiệp, lọc ra các điều khoản thuộc nhóm BẢO MẬT THÔNG TIN hoặc KỶ LUẬT LAO ĐỘNG.
* **Thực thể & Thuộc tính tham gia:**
  * `DieuKhoanHopDong`: `maHopDong`, `soThuTuDieu`, `tieuDeDieuKhoan`, `noiDungDieuKhoan`, `loaiDieuKhoan`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Tầng DAO: `getByLoaiDieuKhoan(LoaiDieuKhoan.BAO_MAT, LoaiDieuKhoan.KY_LUAT)`.
  2. Lọc bằng Stream: `dk.getLoaiDieuKhoan() == loai1 || dk.getLoaiDieuKhoan() == loai2`.
  3. Tầng Service: In danh sách điều khoản pháp lý trọng yếu ra console.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    dieuKhoanHopDongDAO.getByLoaiDieuKhoan(LoaiDieuKhoan.BAO_MAT, LoaiDieuKhoan.KY_LUAT).forEach(dk -> 
        System.out.printf("HĐ #%-4d | Điều %d [%-12s]: %-25s | Nội dung: %s%n",
                dk.getMaHopDong(), dk.getSoThuTu(), dk.getLoaiDieuKhoan(), dk.getTieuDe(), dk.getNoiDung()));
    ```
    ```csharp
    dieuKhoanHopDongDAO.GetByLoaiDieuKhoan(LoaiDieuKhoan.BAO_MAT, LoaiDieuKhoan.KY_LUAT).ForEach(dk => 
        Console.WriteLine($"HĐ #{dk.getMaHopDong(),-4} | Điều {dk.getSoThuTu()} [{dk.getLoaiDieuKhoan(),-12}]: {dk.getTieuDe(),-25} | Nội dung: {dk.getNoiDung()}"));
    ```

---

# PHẦN 3: ĐẶC TẢ 10 CÂU TRUY VẤN - PHÂN HỆ TÀI CHÍNH, TIỀN LƯƠNG & KPI
### Sinh viên phụ trách: NGUYỄN GIA HẠO (MSSV: 25162030)

---

### CÂU 21: IN PHIẾU LƯƠNG CÁ NHÂN CHI TIẾT (PAYSLIP)

* **Tên phương thức:** `inPhieuLuongChiTiet(int maNhanVien, int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Xuất phiếu kết toán lương cá nhân (Payslip) minh bạch gồm tổng thu nhập gộp (Gross), các khoản trích nộp bảo hiểm, thuế, phạt (Khấu trừ) và số tiền thực lãnh về tài khoản (Net).
* **Thực thể & Thuộc tính tham gia:**
  * `BangLuong`: `maBangLuong`, `maNhanVien`, `kyThang`, `kyNam`, `thucLanh`.
  * `ChiTietBangLuong`: `tenKhoanMuc`, `loaiKhoanMuc` (CONG/TRU), `soTien`, `ghiChu`.
* **Dữ liệu đầu vào:** `maNhanVien`, `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Tra cứu bảng lương: `bangLuongDAO.getByNhanVienVaKy(maNV, thang, nam)`.
  2. Nạp chi tiết: `chiTietBangLuongDAO.getByBangLuong(bl.getMaBangLuong())`.
  3. In tiêu đề phiếu lương và duyệt danh sách chi tiết từng khoản mục cộng/trừ.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    BangLuong bl = bangLuongDAO.getByNhanVienVaKy(maNV, thang, nam).orElse(null);
    if (bl != null) {
        System.out.printf("PHIẾU LƯƠNG: %s | Tháng: %d/%d | Lương thực lãnh: %,.0f VNĐ%n",
                getTenNhanVien(bl.getMaNhanVien()), thang, nam, bl.getLuongThucLanh());
        chiTietBangLuongDAO.getByBangLuong(bl.getMaBangLuong()).forEach(ct -> 
            System.out.printf("  [%s] %-25s: %,12.0f VNĐ (%s)%n", 
                    ct.getLoaiKhoanMuc(), ct.getTenKhoanMuc(), ct.getSoTien(), ct.getGhiChu()));
    }
    ```
    ```csharp
    var bl = bangLuongDAO.GetByNhanVienVaKy(maNV, thang, nam);
    if (bl != null) {
        Console.WriteLine($"PHIẾU LƯƠNG: {getTenNhanVien(bl.getMaNhanVien())} | Tháng: {thang}/{nam} | Lương thực lãnh: {bl.getLuongThucLanh():N0} VNĐ");
        chiTietBangLuongDAO.GetByBangLuong(bl.getMaBangLuong()).ForEach(ct => 
            Console.WriteLine($"  [{ct.getLoaiKhoanMuc()}] {ct.getTenKhoanMuc(),-25}: {ct.getSoTien(),12:N0} VNĐ ({ct.getGhiChu()})"));
    }
    ```

---

### CÂU 22: BẢNG TỔNG HỢP CHI PHÍ LƯƠNG THÁNG TOÀN CÔNG TY

* **Tên phương thức:** `tongChiPhiLuongThang(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Giúp Giám đốc Tài chính (CFO) tính toán tổng ngân sách tiền lương công ty phải giải ngân trong tháng bằng cách cộng dồn toàn bộ lương thực lãnh của tất cả nhân viên.
* **Thực thể & Thuộc tính tham gia:**
  * `BangLuong`: `kyThang`, `kyNam`, `thucLanh`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Lấy danh sách bảng lương trong kỳ: `bangLuongDAO.getByKy(thang, nam)`.
  2. Mở Stream, chuyển sang `DoubleStream`: `.mapToDouble(BangLuong::getLuongThucLanh)`.
  3. Tính tổng: `.sum()`.
  4. In tổng chi phí và tổng số lượng nhân viên được chi trả.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    List<BangLuong> dsKy = bangLuongDAO.getByKy(thang, nam);
    double tongChi = dsKy.stream().mapToDouble(BangLuong::getLuongThucLanh).sum();
    System.out.printf("TỔNG CHI PHÍ LƯƠNG THÁNG %d/%d: %,.0f VNĐ (%d nhân viên)%n", thang, nam, tongChi, dsKy.size());
    ```
    ```csharp
    var dsKy = bangLuongDAO.GetByKy(thang, nam);
    double tongChi = dsKy.Sum(b => b.getLuongThucLanh());
    Console.WriteLine($"TỔNG CHI PHÍ LƯƠNG THÁNG {thang}/{nam}: {tongChi:N0} VNĐ ({dsKy.Count} nhân viên)");
    ```

---

### CÂU 23: THỐNG KÊ QUỸ LƯƠNG THỰC LÃNH THEO TỪNG PHÒNG BAN

* **Tên phương thức:** `thongKeQuyLuongTheoPhongBan(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Phân tích cơ cấu ngân sách tiền lương theo từng phòng ban để đánh giá hiệu quả chi phí nhân sự giữa các bộ phận (Khối Kỹ thuật, Kinh doanh, Nhân sự...).
* **Thực thể & Thuộc tính tham gia:**
  * `PhongBan`: `maPhongBan`, `tenPhongBan`.
  * `NhanVien`: `maNhanVien`, `maPhongBan`.
  * `BangLuong`: `maNhanVien`, `kyThang`, `kyNam`, `thucLanh`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Duyệt qua từng phòng ban: `phongBanDAO.getAll().forEach()`.
  2. Với mỗi phòng, lọc danh sách bảng lương trong kỳ có nhân viên thuộc phòng ban đó.
  3. Tính tổng bằng `.mapToDouble(BangLuong::getLuongThucLanh).sum()`.
  4. In tên phòng ban và tổng quỹ lương tương ứng.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    phongBanDAO.getAll().forEach(pb -> {
        double tongPB = bangLuongDAO.getByKy(thang, nam).stream()
            .filter(bl -> {
                NhanVien nv = nhanVienDAO.getById(bl.getMaNhanVien()).orElse(null);
                return nv != null && nv.getMaPhongBan() == pb.getmaPhongBan();
            })
            .mapToDouble(BangLuong::getLuongThucLanh).sum();
        System.out.printf("Phòng ban %-22s: %,15.0f VNĐ%n", pb.gettenPhongBan(), tongPB);
    });
    ```
    ```csharp
    phongBanDAO.GetAll().ForEach(pb => {
        double tongPB = bangLuongDAO.GetByKy(thang, nam)
            .Where(bl => {
                var nv = nhanVienDAO.GetById(bl.getMaNhanVien());
                return nv != null && nv.getMaPhongBan() == pb.getmaPhongBan();
            })
            .Sum(bl => bl.getLuongThucLanh());
        Console.WriteLine($"Phòng ban {pb.gettenPhongBan(),-22}: {tongPB,15:N0} VNĐ");
    });
    ```

---

### CÂU 24: TOP 3 THU NHẬP THỰC LÃNH CAO NHẤT TRONG THÁNG

* **Tên phương thức:** `top3ThuNhapCaoNhat(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Trích xuất 3 nhân sự có thu nhập thực lãnh cao nhất trong kỳ, phục vụ phân tích chế độ đãi ngộ đối với nhân tài và cấp quản trị.
* **Thực thể & Thuộc tính tham gia:**
  * `BangLuong`: `maNhanVien`, `kyThang`, `kyNam`, `thucLanh`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Lấy danh sách bảng lương trong kỳ: `bangLuongDAO.getByKy(thang, nam)`.
  2. Sắp xếp giảm dần theo thu nhập: `.sorted((a, b) -> Double.compare(b.getLuongThucLanh(), a.getLuongThucLanh()))`.
  3. Cắt 3 bản ghi đầu tiên: `.limit(3)`.
  4. In danh sách Top 3 thu nhập.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    bangLuongDAO.getByKy(thang, nam).stream()
        .sorted((a, b) -> Double.compare(b.getLuongThucLanh(), a.getLuongThucLanh()))
        .limit(3)
        .forEach(bl -> System.out.printf("TOP %s: %,.0f VNĐ%n", getTenNhanVien(bl.getMaNhanVien()), bl.getLuongThucLanh()));
    ```
    ```csharp
    bangLuongDAO.GetByKy(thang, nam)
        .OrderByDescending(b => b.getLuongThucLanh())
        .Take(3).ToList()
        .ForEach(bl => Console.WriteLine($"TOP {getTenNhanVien(bl.getMaNhanVien())}: {bl.getLuongThucLanh():N0} VNĐ"));
    ```

---

### CÂU 25: THỐNG KÊ CHI TIẾT KHẤU TRỪ BẢO HIỂM BẮT BUỘC (BHXH, BHYT, BHTN)

* **Tên phương thức:** `thongKeKhauTruBaoHiem(int thang, int nam)`
* **Mục tiêu nghiệp vụ:** Bóc tách các khoản giảm trừ mang tính chất bảo hiểm nhà nước từ bảng `ChiTietBangLuong`, tính tổng số tiền trích nộp theo từng loại bảo hiểm (BHXH, BHYT, BHTN) để nộp cơ quan Bảo hiểm Xã hội.
* **Thực thể & Thuộc tính tham gia:**
  * `ChiTietBangLuong`: `tenKhoanMuc`, `loaiKhoanMuc`, `soTien`.
* **Dữ liệu đầu vào:** `thang`, `nam`.
* **Luồng xử lý (Pipeline Logic):**
  1. Lọc bảng chi tiết lương: `ct.getLoaiKhoanMuc() == LoaiKhoanMuc.TRU && ct.getTenKhoanMuc().toUpperCase().contains("BH")`.
  2. Gom nhóm theo tên khoản mục bảo hiểm: `Collectors.groupingBy(ChiTietBangLuong::getTenKhoanMuc, Collectors.summingDouble(ChiTietBangLuong::getSoTien))`.
  3. In bảng kê tổng số tiền trích nộp bảo hiểm.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chiTietBangLuongDAO.getAll().stream()
        .filter(ct -> ct.getLoaiKhoanMuc() == LoaiKhoanMuc.TRU && ct.getTenKhoanMuc().toUpperCase().contains("BH"))
        .collect(Collectors.groupingBy(ChiTietBangLuong::getTenKhoanMuc, Collectors.summingDouble(ChiTietBangLuong::getSoTien)))
        .forEach((ten, tong) -> System.out.printf("%-30s: %,.0f VNĐ%n", ten, tong));
    ```
    ```csharp
    chiTietBangLuongDAO.GetAll()
        .Where(ct => ct.getLoaiKhoanMuc() == LoaiKhoanMuc.TRU && ct.getTenKhoanMuc().ToUpper().Contains("BH"))
        .GroupBy(ct => ct.getTenKhoanMuc())
        .ToList().ForEach(g => Console.WriteLine($"{g.Key,-30}: {g.Sum(c => c.getSoTien()):N0} VNĐ"));
    ```

---

### CÂU 26: BÁO CÁO DANH MỤC TIỀN THƯỞNG VÀ PHỤ CẤP ĐÃ CHI TOÀN CÔNG TY

* **Tên phương thức:** `baoCaoTienThuongVaPhuCap()`
* **Mục tiêu nghiệp vụ:** Tổng hợp toàn bộ các khoản mục chi thêm ngoài lương cơ bản (Phụ cấp trách nhiệm, Thưởng dự án, Thưởng KPI, Phụ cấp ăn trưa...) để quản lý các gói đãi ngộ nhân viên.
* **Thực thể & Thuộc tính tham gia:**
  * `ChiTietBangLuong`: `tenKhoanMuc`, `loaiKhoanMuc`, `soTien`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Lọc: `ct.getLoaiKhoanMuc() == LoaiKhoanMuc.CONG && !ct.getTenKhoanMuc().contains("Lương cơ bản")`.
  2. Gom nhóm theo tên khoản phụ cấp/thưởng và tính tổng số tiền đã chi trả.
  3. In danh mục báo cáo.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chiTietBangLuongDAO.getAll().stream()
        .filter(ct -> ct.getLoaiKhoanMuc() == LoaiKhoanMuc.CONG && !ct.getTenKhoanMuc().contains("Lương cơ bản"))
        .collect(Collectors.groupingBy(ChiTietBangLuong::getTenKhoanMuc, Collectors.summingDouble(ChiTietBangLuong::getSoTien)))
        .forEach((ten, tong) -> System.out.printf("Khoản chi %-25s: %,.0f VNĐ%n", ten, tong));
    ```
    ```csharp
    chiTietBangLuongDAO.GetAll()
        .Where(ct => ct.getLoaiKhoanMuc() == LoaiKhoanMuc.CONG && !ct.getTenKhoanMuc().Contains("Lương cơ bản"))
        .GroupBy(ct => ct.getTenKhoanMuc())
        .ToList().ForEach(g => Console.WriteLine($"Khoản chi {g.Key,-25}: {g.Sum(c => c.getSoTien()):N0} VNĐ"));
    ```

---

### CÂU 27: BẢN IN PHIẾU ĐÁNH GIÁ HIỆU SUẤT (KPI) CHI TIẾT TỪNG TIÊU CHÍ

* **Tên phương thức:** `inPhieuDanhGiaChiTiet(int maDanhGia)`
* **Mục tiêu nghiệp vụ:** In bản đánh giá hiệu suất định kỳ của một nhân viên gồm điểm tổng kết, xếp loại thi đua và danh sách chi tiết từng tiêu chí đánh giá (Chất lượng công việc, Tiến độ, Kỷ luật...) kèm điểm số và trọng số tương ứng.
* **Thực thể & Thuộc tính tham gia:**
  * `DanhGiaHieuSuat`: `maDanhGia`, `maNhanVien`, `diemTrungBinh`, `xepLoai`.
  * `ChiTietDanhGia`: `tieuChiDanhGia`, `diemSo`, `trongSo`.
* **Dữ liệu đầu vào:** `maDanhGia` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Tra cứu phiếu đánh giá: `danhGiaHieuSuatDAO.getById(maDanhGia)`.
  2. Nạp chi tiết: `chiTietDanhGiaDAO.getByDanhGia(maDanhGia)`.
  3. In tiêu đề phiếu KPI và duyệt in từng tiêu chí kèm tỷ lệ trọng số (%).
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    DanhGiaHieuSuat dg = danhGiaHieuSuatDAO.getById(maDanhGia).orElse(null);
    if (dg != null) {
        System.out.printf("PHIẾU ĐÁNH GIÁ: %s | Điểm: %.2f | Xếp loại: %s%n",
                getTenNhanVien(dg.getMaNhanVien()), dg.getDiemTrungBinh(), dg.getXepLoai());
        chiTietDanhGiaDAO.getByDanhGia(maDanhGia).forEach(ct -> 
            System.out.printf("  - %-25s: Điểm %.1f (Trọng số %.0f%%)%n", 
                    ct.getTenTieuChi(), ct.getDiemSo(), ct.getTrongSo() * 100));
    }
    ```
    ```csharp
    var dg = danhGiaHieuSuatDAO.GetById(maDanhGia);
    if (dg != null) {
        Console.WriteLine($"PHIẾU ĐÁNH GIÁ: {getTenNhanVien(dg.getMaNhanVien())} | Điểm: {dg.getDiemTrungBinh():F2} | Xếp loại: {dg.getXepLoai()}");
        chiTietDanhGiaDAO.GetByDanhGia(maDanhGia).ForEach(ct => 
            Console.WriteLine($"  - {ct.getTenTieuChi(),-25}: Điểm {ct.getDiemSo():F1} (Trọng số {ct.getTrongSo() * 100:F0}%)"));
    }
    ```

---

### CÂU 28: XẾP HẠNG HIỆU SUẤT NHÂN VIÊN THEO TỪNG NHÓM PHÂN LOẠI (XUẤT SẮC, TỐT, TB, YẾU)

* **Tên phương thức:** `xepHangHieuSuatTheoLoai(int nam)`
* **Mục tiêu nghiệp vụ:** Phân nhóm nhân sự trong năm theo các danh hiệu thi đua (XUẤT SẮC, TỐT, TRUNG BÌNH, YẾU) làm cơ sở xét duyệt tiền thưởng Tết và thăng chức.
* **Thực thể & Thuộc tính tham gia:**
  * `DanhGiaHieuSuat`: `maNhanVien`, `nam`, `xepLoai`, `diemTrungBinh`.
  * `NhanVien`: `ho`, `ten`.
* **Dữ liệu đầu vào:** `nam` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Lấy danh sách đánh giá trong năm: `danhGiaHieuSuatDAO.getByNam(nam)`.
  2. Gom nhóm theo xếp loại: `.collect(Collectors.groupingBy(DanhGiaHieuSuat::getXepLoai))`.
  3. Duyệt từng nhóm xếp loại và in danh sách nhân viên trực thuộc nhóm đó.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    danhGiaHieuSuatDAO.getByNam(nam).stream()
        .collect(Collectors.groupingBy(DanhGiaHieuSuat::getXepLoai))
        .forEach((xl, ds) -> {
            System.out.printf("=== XẾP LOẠI %s (%d người) ===%n", xl, ds.size());
            ds.forEach(d -> System.out.printf("  - %-22s: Điểm %.2f%n", getTenNhanVien(d.getMaNhanVien()), d.getDiemTrungBinh()));
        });
    ```
    ```csharp
    danhGiaHieuSuatDAO.GetByNam(nam).GroupBy(d => d.getXepLoai()).ToList()
        .ForEach(g => {
            Console.WriteLine($"=== XẾP LOẠI {g.Key} ({g.Count()} người) ===");
            g.ToList().ForEach(d => Console.WriteLine($"  - {getTenNhanVien(d.getMaNhanVien()),-22}: Điểm {d.getDiemTrungBinh():F2}"));
        });
    ```

---

### CÂU 29: TÌM KIẾM NHÂN SỰ XUẤT SẮC TOÀN DIỆN (ĐẠT KPI XUẤT SẮC VÀ KHÔNG NGHỈ PHÉP)

* **Tên phương thức:** `nhanSuXuatSacToanDien(int nam)`
* **Mục tiêu nghiệp vụ:** Vinh danh danh hiệu "Chiến sĩ thi đua toàn diện": Vừa đạt xếp loại hiệu suất cao nhất (`XUAT_SAC`), vừa không có bất kỳ ngày nghỉ phép nào trong suốt cả năm.
* **Thực thể & Thuộc tính tham gia:**
  * `DanhGiaHieuSuat`: `maNhanVien`, `nam`, `xepLoai`, `diemTrungBinh`.
  * `DonXinNghi`: `maNhanVien`.
* **Dữ liệu đầu vào:** `nam` (int).
* **Luồng xử lý (Pipeline Logic):**
  1. Mở Stream trên phiếu đánh giá KPI trong năm.
  2. Lọc 1: `d.getXepLoai() == XepLoai.XUAT_SAC`.
  3. Lọc 2: Kiểm tra danh sách đơn nghỉ phép của nhân viên phải rỗng: `donXinNghiDAO.getByNhanVien(d.getMaNhanVien()).isEmpty()`.
  4. In danh sách gương mặt xuất sắc toàn diện.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    danhGiaHieuSuatDAO.getByNam(nam).stream()
        .filter(d -> d.getXepLoai() == XepLoai.XUAT_SAC)
        .filter(d -> donXinNghiDAO.getByNhanVien(d.getMaNhanVien()).isEmpty())
        .forEach(d -> System.out.printf("XUẤT SẮC TOÀN DIỆN: %s | Điểm KPI: %.2f%n", 
                getTenNhanVien(d.getMaNhanVien()), d.getDiemTrungBinh()));
    ```
    ```csharp
    danhGiaHieuSuatDAO.GetByNam(nam)
        .Where(d => d.getXepLoai() == XepLoai.XUAT_SAC)
        .Where(d => donXinNghiDAO.GetByNhanVien(d.getMaNhanVien()).Count == 0).ToList()
        .ForEach(d => Console.WriteLine($"XUẤT SẮC TOÀN DIỆN: {getTenNhanVien(d.getMaNhanVien())} | Điểm KPI: {d.getDiemTrungBinh():F2}"));
    ```

---

### CÂU 30: THỐNG KÊ THU NHẬP TRUNG BÌNH THEO TỪNG CHỨC VỤ

* **Tên phương thức:** `thongKeThuNhapTrungBinhTheoChucVu()`
* **Mục tiêu nghiệp vụ:** Tính mức thu nhập thực lãnh bình quân (`Average`) của từng vị trí chức danh nghề nghiệp trong công ty, phục vụ khảo sát mức đãi ngộ so với mặt bằng thị trường lao động.
* **Thực thể & Thuộc tính tham gia:**
  * `ChucVu`: `maChucVu`, `tenChucVu`.
  * `BangLuong`: `maNhanVien`, `thucLanh`.
  * `NhanVien`: `maNhanVien`, `maChucVu`.
* **Dữ liệu đầu vào:** Không có tham số.
* **Luồng xử lý (Pipeline Logic):**
  1. Duyệt qua từng chức vụ: `chucVuDAO.getAll().forEach()`.
  2. Lọc bảng lương của các nhân viên giữ chức vụ đó.
  3. Tính trung bình bằng `.mapToDouble(BangLuong::getLuongThucLanh).average().orElse(0)`.
  4. In chức vụ và thu nhập trung bình.
*Toán tử C# LINQ áp dụng:* * **Toán tử Stream / LINQ áp dụng:**
    chucVuDAO.getAll().forEach(cv -> {
        double tb = bangLuongDAO.getAll().stream()
            .filter(bl -> {
                NhanVien nv = nhanVienDAO.getById(bl.getMaNhanVien()).orElse(null);
                return nv != null && nv.getMaChucVu() == cv.getmaChucVu();
            })
            .mapToDouble(BangLuong::getLuongThucLanh).average().orElse(0);
        System.out.printf("Chức vụ %-28s: Lương TB %,12.0f VNĐ%n", cv.gettenChucVu(), tb);
    });
    ```
    ```csharp
    chucVuDAO.GetAll().ForEach(cv => {
        var dsLg = bangLuongDAO.GetAll().Where(bl => {
            var nv = nhanVienDAO.GetById(bl.getMaNhanVien());
            return nv != null && nv.getMaChucVu() == cv.getmaChucVu();
        }).ToList();
        double tb = dsLg.Count > 0 ? dsLg.Average(bl => bl.getLuongThucLanh()) : 0;
        Console.WriteLine($"Chức vụ {cv.gettenChucVu(),-28}: Lương TB {tb,12:N0} VNĐ");
    });
    ```

---
