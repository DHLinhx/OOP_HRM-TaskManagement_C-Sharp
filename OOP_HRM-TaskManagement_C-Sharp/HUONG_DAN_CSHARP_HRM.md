# Áp dụng C# LINQ, Delegates & Lambdas vào Hệ Thống Quản Trị Nhân Sự (HRM & Task Management)

---

## 1. Mục tiêu

Dự án kết hợp chặt chẽ giữa **Lập trình Hướng đối tượng (OOP)** và **Lập trình hàm (Functional Programming)** trong C# .NET 10:
* **Về mặt OOP:** Kế thừa đầy đủ 4 trụ cột cơ bản (*Đóng gói - Encapsulation, Trừu tượng - Abstraction, Kế thừa - Inheritance, Đa hình - Polymorphism*), áp dụng mô hình kiến trúc phân tầng 3 lớp (**Presentation / Program $\to$ Service Layer $\to$ DAO Layer $\to$ Model Layer**), giao diện chuẩn `IDAO<T, TKey>`.
* **Về mặt Functional Programming (C# LINQ & Delegates):** Áp dụng toàn diện biểu thức Lambda (`=>`), Generic Delegates (`Func`, `Action`, `Predicate`) và Language Integrated Query (LINQ) to Objects để thay thế các vòng lặp `foreach`/`while` và `if-else` lồng nhau phức tạp. Sử dụng LINQ cho các tác vụ: lọc hồ sơ (`Where`), ánh xạ dữ liệu (`Select`), sắp xếp (`OrderBy`/`ThenBy`), tính tổng tiền lương (`Sum`), tính trung bình (`Average`), phân nhóm báo cáo (`GroupBy`), và truyền chiến lược tính toán linh hoạt qua Delegate.
* **Nguyên tắc bất biến (Immutability & No Side-Effects):** Mọi thao tác LINQ chỉ thực hiện truy vấn và chiếu dữ liệu hiển thị, tuyệt đối không làm thay đổi trạng thái gốc của kho dữ liệu bên dưới (`DatabaseMock`).

---

## 2. Delegate & Generic Delegates trong C# là gì?

**Delegate** trong C# là một kiểu dữ liệu an toàn về kiểu (type-safe) đại diện cho các tham chiếu đến các phương thức có cùng danh sách tham số và kiểu trả về (tương tự con trỏ hàm trong C++ nhưng được CLR quản lý an toàn). 

Trong khi Java 8 sử dụng khái niệm **Functional Interface** (interface có duy nhất 1 phương thức trừu tượng - SAM), thì C# giải quyết bài toán này trực tiếp thông qua **Delegates** và bộ Generic Delegates xây dựng sẵn trong namespace `System`:

### Bảng đối chiếu giữa Java Functional Interface và C# Generic Delegates:

| Khái niệm Java 8 | Kiểu Delegate C# tương đương | Cú pháp Delegate trong C# | Vai trò thực tế trong dự án HRM |
|---|---|---|---|
| `Predicate<T>` | `Predicate<T>` hoặc `Func<T, bool>` | `bool Method(T obj)` | Điều kiện lọc nhân viên (theo phòng ban, đang làm việc, sắp hết hạn hợp đồng, chuyên cần 100%, KPI xuất sắc) |
| `Consumer<T>` | `Action<T>` | `void Method(T obj)` | Tiếp nhận và xuất dữ liệu ra màn hình Console (`Console.WriteLine`) hoặc ghi log |
| `Supplier<T>` | `Func<T>` | `T Method()` | Cung cấp ngày kiểm tra hệ thống, ngày chốt lương cố định hoặc khởi tạo đối tượng |
| `Function<T, R>` | `Func<T, R>` | `R Method(T obj)` | Ánh xạ đối tượng hoặc chuyển đổi: Mã NV $\to$ Họ tên, Mã PB $\to$ Tên phòng ban, Mã CV $\to$ Tên chức vụ |
| `BiFunction<T, U, R>` | `Func<T, U, R>` | `R Method(T obj1, U obj2)` | Tính toán kết hợp giữa 2 tham số: (Bản ghi chấm công + Đơn giá lương giờ) $\to$ Số tiền làm thêm (OT) |
| `ToDoubleFunction<T>` | `Func<T, decimal>` / `Func<T, double>` | `decimal Method(T obj)` | Trích xuất trường số thực trong `Sum()` / `Average()` của LINQ |
| Custom `@FunctionalInterface` | `delegate` tự định nghĩa | `delegate decimal SalaryCalculator(...)` | Delegate tự định nghĩa đại diện cho chiến lược (Strategy) tính kết toán lương |

> *Ghi chú:* Toàn bộ các delegate `Func`, `Action`, `Predicate` đều có sẵn trong thư viện chuẩn `System` của .NET, lập trình viên không cần khai báo lại.

---

## 3. Predicate & Func<T, bool>: Lọc và kết hợp điều kiện (`&&`, `||`, `!`)

Trong C#, điều kiện lọc dữ liệu có thể được đóng gói dưới dạng `Func<T, bool>` hoặc `Predicate<T>`.

### Xây dựng tập các vị từ chuẩn trong `NhanVienFilters.cs`:

```csharp
namespace OOP_HRM_TaskManagement_C_Sharp.Filters
{
    public static class NhanVienFilters
    {
        // Nhân viên đang làm việc bình thường
        public static readonly Func<NhanVien, bool> DangLamViec = 
            nv => nv.TrangThaiLamViec == TrangThaiLamViec.DangLamViec;

        // Nhân viên đã nghỉ việc (phủ định)
        public static readonly Func<NhanVien, bool> DaNghiViec = 
            nv => nv.TrangThaiLamViec == TrangThaiLamViec.DaNghiViec;

        // Vị từ động: Lọc nhân viên thuộc một phòng ban cụ thể
        public static Func<NhanVien, bool> ThuocPhongBan(int maPhongBan) =>
            nv => nv.MaPhongBan == maPhongBan;

        // Vị từ động: Lọc nhân viên là cấp dưới của một quản lý
        public static Func<NhanVien, bool> CapDuoiCua(int maQuanLy) =>
            nv => nv.MaQuanLy.HasValue && nv.MaQuanLy.Value == maQuanLy;
    }
}
```

### Ứng dụng kết hợp điều kiện trong Service:

```csharp
// Lọc các nhân viên ĐANG LÀM VIỆC VÀ THUỘC PHÒNG BAN SỐ 1 (Phòng CNTT)
int soLuongNV = nhanVienDAO.GetAll()
    .Where(nv => NhanVienFilters.DangLamViec(nv) && NhanVienFilters.ThuocPhongBan(1)(nv))
    .Count();
```

* **Cơ chế thực thi:** 
  * Toán tử `&&`: Yêu cầu cả 2 biểu thức đều trả về `true` (có cơ chế Short-circuit: nếu vế đầu `false` thì bỏ qua vế sau).
  * Toán tử `||`: Thỏa mãn ít nhất 1 trong 2 điều kiện.
  * Toán tử `!`: Đảo ngược giá trị logic (`true` $\to$ `false`).

---

## 4. Action: Tiếp nhận và xuất dữ liệu (`Console.WriteLine`)

`Action<T>` đại diện cho một tác vụ tiếp nhận dữ liệu đầu vào kiểu `T` và không trả về giá trị (`void`) - tương đương `Consumer<T>` trong Java.

```csharp
// Khai báo Action định dạng và in dòng nhân viên ra console
Action<NhanVien> inDongNhanVien = nv => Console.WriteLine(
    $"{nv.MaNhanVien,-8} {nv.HoTen,-22} {nv.GioiTinh,-12} {nv.NgayVaoLam:yyyy-MM-dd}"
);

// Duyệt và tiêu thụ dữ liệu bằng vòng lặp hoặc List.ForEach
nhanVienDAO.GetAll().ForEach(inDongNhanVien);
```

### Tách biệt nơi nhận kết quả giữa Môi trường Thực tế và Môi trường Test:
```csharp
// Trong Console thực tế:
Action<string> outputConsole = Console.WriteLine;

// Trong Unit Test: Thu thập log vào một danh sách để kiểm chứng mà không làm rác Console
List<string> logs = new List<string>();
Action<string> outputTest = logs.Add;
```

---

## 5. Func: Cung cấp ngày tháng và dữ liệu chủ động (Supplier Pattern)

`Func<T>` không nhận tham số đầu vào nhưng trả về một đối tượng kiểu `T` (`T Method()`) - tương đương `Supplier<T>` trong Java. Trong hệ thống HRM, `Func<T>` giải quyết bài toán mốc thời gian kiểm thử:

```csharp
// Cung cấp ngày kiểm tra cố định để kết quả báo cáo luôn nhất quán:
Func<DateOnly> ngayKiemTraDemo = () => new DateOnly(2024, 3, 31);

// Khi chạy thật dùng thời gian máy tính hiện tại:
Func<DateOnly> ngayHienTai = () => DateOnly.FromDateTime(DateTime.Now);
```

### Ứng dụng trong Câu 12 (Cảnh báo hợp đồng sắp hết hạn trong 30 ngày):
```csharp
public void CanhBaoHopDongSapHetHan(Func<DateOnly> dateProvider)
{
    DateOnly mocNgay = dateProvider();
    var dsSapHetHan = hopDongDAO.GetAll()
        .Where(hd => hd.TrangThaiHopDong == TrangThaiHopDong.HieuLuc &&
                     hd.NgayKetThuc >= mocNgay &&
                     hd.NgayKetThuc.DayNumber - mocNgay.DayNumber <= 30)
        .ToList();

    foreach (var hd in dsSapHetHan)
    {
        int soNgay = hd.NgayKetThuc.DayNumber - mocNgay.DayNumber;
        Console.WriteLine($"Hợp đồng #{hd.MaHopDong} hết hạn sau {soNgay} ngày.");
    }
}
```
Nhờ `Func<DateOnly>`, khi bảo vệ đồ án trước Giảng viên, bài chạy luôn hiển thị danh sách hợp đồng hợp lệ mà không phụ thuộc vào thời điểm thực tế máy tính chạy vào năm nào.

---

## 6. Func và Chaining: Biến đổi dữ liệu (Mapping Pipeline với Select)

`Func<T, R>` nhận đầu vào kiểu `T` và biến đổi thành kết quả kiểu `R` - tương đương `Function<T, R>` trong Java.

```csharp
// 1. Func trích xuất họ tên từ đối tượng NhanVien
Func<NhanVien, string> layHoTen = nv => nv.HoTen;

// 2. Func định dạng chuỗi: thêm ký tự đánh dấu đầu dòng
Func<string, string> themDauDong = text => "  -> " + text;

// 3. Sử dụng chuỗi biến đổi trong LINQ Select:
var dsDongBaoCao = nhanVienDAO.GetAll()
    .Select(layHoTen)
    .Select(themDauDong);

foreach (var dong in dsDongBaoCao)
{
    Console.WriteLine(dong);
}
```

* Trong C# LINQ, toán tử `.Select()` chính là hiện thân của hàm ánh xạ cấp cao (Higher-Order Mapping Function).

---

## 7. Delegate tự định nghĩa: `SalaryCalculator` (Strategy Pattern)

Ngoài các Generic Delegates có sẵn (`Func`, `Action`), C# cho phép định nghĩa delegate tường minh bằng từ khóa `delegate`:

```csharp
namespace OOP_HRM_TaskManagement_C_Sharp.Services
{
    // Định nghĩa Delegate đại diện cho Chiến lược tính lương Net
    public delegate decimal SalaryCalculator(decimal baseSalary, decimal allowance, decimal overtimePay, decimal deduction);

    public static class SalaryStrategies
    {
        // Chiến lược chuẩn mực theo quy định Luật Lao động (trừ 10.5% bảo hiểm)
        public static SalaryCalculator Standard => (baseSal, allowance, ot, deduction) =>
        {
            decimal gross = baseSal + allowance + ot;
            decimal bhxh = baseSal * 0.105m; // 10.5% BHXH, BHYT, BHTN
            return gross - bhxh - deduction;
        };

        // Chiến lược tính lương thời vụ / thử việc (không trừ bảo hiểm)
        public static SalaryCalculator Probation => (baseSal, allowance, ot, deduction) =>
        {
            return (baseSal + allowance + ot) - deduction;
        };
    }
}
```

---

## 8. LINQ: Tính tổng chi phí lương và giờ làm thêm (`Sum`, `Average`)

LINQ to Objects hỗ trợ trực tiếp các phương thức tính toán tổng gộp mạnh mẽ trên kiểu dữ liệu số (`decimal`, `double`, `int`):

### Tính tổng chi phí tiền lương toàn công ty trong tháng (Câu 22):
```csharp
var dsBangLuongThang = bangLuongDAO.GetAll()
    .Where(bl => bl.Thang == thang && bl.Nam == nam)
    .ToList();

decimal tongChiPhiLuong = dsBangLuongThang
    .Sum(bl => bl.LuongThucLanh); // Tính tổng dồn trực tiếp
```

### Tính tổng số giờ làm thêm (OT) của nhân viên (Câu 15):
```csharp
double tongGioOT = chamCongDAO.GetAll()
    .Where(cc => cc.MaNhanVien == maNV && cc.SoGioLamThem > 0)
    .Sum(cc => cc.SoGioLamThem);
```
* **Lưu ý kỹ thuật:** Trong C#, nếu danh sách rỗng, phương thức `Sum()` trả về an toàn giá trị `0` mà không gây lỗi `NullReferenceException`.

---

## 9. Thống kê nâng cao: `GroupBy`, gom nhóm báo cáo đa chiều

Toán tử `.GroupBy()` trong C# tương đương trực tiếp với `Collectors.groupingBy()` của Java Stream, nhưng cú pháp C# súc tích và dễ đọc hơn rất nhiều:

### Thống kê nhân sự đa nhiệm tham gia $\ge 2$ nhóm dự án (Câu 7):
```csharp
thanhVienNhomDAO.GetAll()
    .GroupBy(tv => tv.MaNhanVien)              // Gom nhóm theo mã nhân viên
    .Where(g => g.Count() >= 2)               // Lọc những ai tham gia >= 2 nhóm
    .ToList()
    .ForEach(g =>
    {
        string tenNV = GetTenNhanVien(g.Key);
        int soNhom = g.Count();
        Console.WriteLine($"- {tenNV} tham gia {soNhom} nhóm dự án.");
    });
```

### Báo cáo xếp hạng nhân viên đi trễ (Câu 14):
```csharp
var mapDiTre = chamCongDAO.GetAll()
    .Where(cc => cc.TrangThaiCong == TrangThaiCong.DiTre)
    .GroupBy(cc => cc.MaNhanVien)
    .ToDictionary(g => g.Key, g => g.Count());
```

### Tổng hợp chi phí các loại phụ cấp và thưởng đã chi (Câu 26):
```csharp
var tongTheoKhoanMuc = chiTietBangLuongDAO.GetAll()
    .Where(ct => ct.LoaiKhoanMuc == LoaiKhoanMuc.Cong)
    .GroupBy(ct => ct.TenKhoanMuc)
    .ToDictionary(g => g.Key, g => g.Sum(ct => ct.SoTien));
```

---

## 10. `OrderBy`, `Take`, `Distinct` và `string.Join`

### Trích xuất Top 3 nhân viên thâm niên cao nhất (Câu 4):
```csharp
nhanVienDAO.GetAll()
    .OrderBy(nv => nv.NgayVaoLam) // Sắp xếp tăng dần theo ngày vào làm
    .Take(3)                     // Trích xuất đúng 3 phần tử đầu (tương đương limit(3))
    .ToList()
    .ForEach(nv => Console.WriteLine($"{nv.HoTen} - {nv.NgayVaoLam:yyyy-MM-dd}"));
```

### Trích xuất Top 3 thu nhập thực lãnh cao nhất trong tháng (Câu 24):
```csharp
bangLuongDAO.GetAll()
    .Where(bl => bl.Thang == thang && bl.Nam == nam)
    .OrderByDescending(bl => bl.LuongThucLanh) // Giảm dần
    .Take(3)
    .ToList()
    .ForEach(bl => Console.WriteLine($"TOP {GetTenNhanVien(bl.MaNhanVien)}: {bl.LuongThucLanh:N0} VNĐ"));
```

### Nối chuỗi tên các nhóm dự án bằng `string.Join` (Câu 7):
```csharp
string dsTenNhom = string.Join(", ", danhSachThanhVien.Select(tv => GetTenNhom(tv.MaNhom)));
// Kết quả: "Nhóm Core, Nhóm Mobile, Ban Sự Kiện"
```

---

## 11. Xử lý An Toàn Null: `FirstOrDefault` & Null-conditional Operator (`?.`, `??`)

Khác với Java phải bao bọc đối tượng trong `Optional<T>`, C# sở hữu hệ thống ngôn ngữ hỗ trợ **Null Safety** hiện đại bậc nhất thông qua Nullable Types, toán tử `?.` và toán tử kết hợp null `??`:

### Phương thức tìm kiếm tại tầng DAO trả về đối tượng có thể Null:
```csharp
public NhanVien? GetById(int id)
{
    return DatabaseMock.DsNhanVien.FirstOrDefault(nv => nv.MaNhanVien == id);
}
```

### Ánh xạ an toàn không sợ Null (Helper method):
```csharp
private string GetTenNhanVien(int maNhanVien)
{
    // Nếu tìm thấy nhân viên thì lấy HoTen, nếu null thì trả về "Không xác định"
    return nhanVienDAO.GetById(maNhanVien)?.HoTen ?? "Không xác định";
}
```

### Bắt buộc phải có hợp đồng (ném ngoại lệ bằng `??`):
```csharp
HopDong hd = hopDongDAO.GetById(maHopDong) 
    ?? throw new ArgumentException($"Lỗi: Không tìm thấy hợp đồng số {maHopDong}");
```
> *So sánh với Java:* C# code `obj?.Property ?? "Default"` ngắn gọn và tự nhiên hơn rất nhiều so với `Optional.ofNullable(obj).map(Obj::getProperty).orElse("Default")` trong Java.

---

## 12. `Func<T1, T2, TResult>`: Xử lý logic 2 tham số (tương đương BiFunction)

Khi bài toán nghiệp vụ đòi hỏi kết hợp dữ liệu giữa hai thực thể:

```csharp
// Func tính số tiền làm ngoài giờ (OT):
// Tham số 1: Đối tượng ChamCong
// Tham số 2: Mức lương cơ bản giờ của nhân viên (decimal)
// Kết quả: Số tiền làm thêm giờ (OT * lương giờ * 1.5)
Func<ChamCong, decimal, decimal> tinhTienOT = (cong, luongGio) =>
{
    if (cong.SoGioLamThem <= 0) return 0m;
    return (decimal)cong.SoGioLamThem * luongGio * 1.5m;
};
```

---

## 13. Thực thi trì hoãn (Deferred Execution) và Method Group trong C#

Một chuỗi truy vấn LINQ hoạt động theo 2 cơ chế quan trọng:

1. **Deferred Execution (Thực thi trì hoãn / Lười):** Các phương thức như `.Where()`, `.Select()`, `.OrderBy()`, `.Take()` trả về `IEnumerable<T>`. Chúng **KHÔNG HỀ THỰC SỰ CHẠY** khi bạn định nghĩa câu lệnh đó. Phép tính chỉ diễn ra khi bạn duyệt danh sách qua `foreach` hoặc gọi thao tác kích hoạt tức thì.
2. **Immediate Execution (Thực thi tức thì):** Các phương thức như `.ToList()`, `.ToArray()`, `.Count()`, `.Sum()`, `.First()` sẽ ép buộc toàn bộ câu truy vấn LINQ thực thi ngay tại thời điểm đó và lưu kết quả vào bộ nhớ.

### Khái niệm Method Group trong C#:
Method Group trong C# tương đương trực tiếp với Method Reference `::` trong Java:
```csharp
// Cách viết biểu thức Lambda thông thường:
nhanVienDAO.GetAll().Select(nv => nv.HoTen);

// Khi dùng với Action Console.WriteLine:
List<string> dsTen = nhanVienDAO.GetAll().Select(nv => nv.HoTen).ToList();
dsTen.ForEach(Console.WriteLine); // Method Group tham chiếu trực tiếp đến Console.WriteLine
```

---

## 14. Tính tương thích và Công nghệ hiện đại (C# 10 $\to$ C# 14 / .NET 10)

* **DateOnly & TimeOnly:** Dự án sử dụng `System.DateOnly` và `System.TimeOnly` (chuẩn từ .NET 6+), phân biệt rạch ròi giữa ngày tháng thuần túy (ngày sinh, ngày ký hợp đồng) và giờ giấc (giờ check-in/out), tránh hoàn toàn các lỗi lệch múi giờ của `DateTime` cũ.
* **Kiểm tra chuỗi rỗng:** Sử dụng `string.IsNullOrWhiteSpace(str)` tiêu chuẩn.
* **Cú pháp ngắn gọn:** Sử dụng Target-typed `new()`, String Interpolation `$"{nv.HoTen}"`, Record types và Expression-bodied members `=>`.
* **Biên dịch hiệu năng cao:** Biên dịch trên nền tảng .NET 10 mới nhất, tối ưu hóa JIT Compiler cho các câu truy vấn LINQ to Objects.

---

## 15. Phối hợp giữa OOP và Functional Programming trong kiến trúc C#

Dự án HRM phân định rõ ranh giới trách nhiệm giữa hai trường phái lập trình:

```
+--------------------------------------------------------------------------+
|                     KIẾN TRÚC TỔNG THỂ (OOP)                             |
|  - Tính đóng gói (Encapsulation): private fields, auto-properties { get; set; }. |
|  - Trừu tượng hóa & Kế thừa: Giao diện tổng quát IDAO<T, TKey>.          |
|  - Kiến trúc phân tầng 3 lớp: Program -> Service Layer -> DAO Layer.     |
+--------------------------------------------------------------------------+
                                    |
                                    v (Ứng dụng bên trong các hàm xử lý)
+--------------------------------------------------------------------------+
|                  THUẬT TOÁN XỬ LÝ DỮ LIỆU (FUNCTIONAL)                   |
|  - LINQ to Objects & Lambdas: Where, Select, OrderBy, GroupBy, Sum.      |
|  - Hàm thuần khiết (Pure Functions): Không làm biến đổi Mock Database.    |
|  - Code ngắn gọn, trực quan, loại bỏ các vòng lặp foreach lồng nhau.     |
+--------------------------------------------------------------------------+
```

1. **OOP chịu trách nhiệm về Khung xương kiến trúc:** Thiết kế các Model bảo vệ toàn vẹn dữ liệu, giao diện `IDAO<T, TKey>` chuẩn hóa các hành vi CRUD, và các Service phân định nghiệp vụ độc lập giữa 3 thành viên (Lĩnh, Dương, Hạo).
2. **Functional Programming chịu trách nhiệm về Thuật toán xử lý luồng:** Các câu truy vấn dữ liệu được viết bằng LINQ to Objects theo phong cách khai báo (Declarative), giúp code ngắn gọn gấp 3 lần so với code lặp truyền thống, dễ đọc, dễ bảo trì và hoàn toàn tương đồng về mặt nghiệp vụ với Java Stream API.
