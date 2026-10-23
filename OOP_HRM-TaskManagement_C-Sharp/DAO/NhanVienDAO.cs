using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class NhanVienDAO : IDAO<NhanVien, int>
{
    public List<NhanVien> GetAll() => DatabaseMock.dsNhanVien;

    public NhanVien? GetById(int id) =>
        DatabaseMock.dsNhanVien.FirstOrDefault(nv => nv.getMaNhanVien() == id);

    public List<NhanVien> GetByPhongBan(int maPB) =>
        DatabaseMock.dsNhanVien.Where(nv => nv.getMaPhongBan() == maPB).ToList();

    public List<NhanVien> GetByChucVu(int maCV) =>
        DatabaseMock.dsNhanVien.Where(nv => nv.getMaChucVu() == maCV).ToList();

    public List<NhanVien> GetCapDuoi(int maQL) =>
        DatabaseMock.dsNhanVien.Where(nv => nv.getMaQuanLy().HasValue && nv.getMaQuanLy().Value == maQL).ToList();

    public List<NhanVien> GetTopThamNien(int limit) =>
        DatabaseMock.dsNhanVien.OrderBy(nv => nv.getNgayVaoLam()).Take(limit).ToList();

    public List<NhanVien> Search(string keyword)
    {
        string kw = keyword.ToLower().Trim();
        return DatabaseMock.dsNhanVien
            .Where(nv => nv.HoTenNV().ToLower().Contains(kw)
                      || nv.getSoDienThoai().Contains(kw)
                      || nv.getEmail().ToLower().Contains(kw))
            .ToList();
    }

    public void Add(NhanVien entity) => DatabaseMock.dsNhanVien.Add(entity);

    public void Update(NhanVien entity)
    {
        int idx = DatabaseMock.dsNhanVien.FindIndex(nv => nv.getMaNhanVien() == entity.getMaNhanVien());
        if (idx >= 0) DatabaseMock.dsNhanVien[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsNhanVien.RemoveAll(nv => nv.getMaNhanVien() == id) > 0;
}
