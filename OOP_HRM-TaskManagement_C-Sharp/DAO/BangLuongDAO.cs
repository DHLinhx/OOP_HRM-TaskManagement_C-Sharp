using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class BangLuongDAO : IDAO<BangLuong, int>
{
    public List<BangLuong> GetAll() => DatabaseMock.dsBangLuong;

    public BangLuong? GetById(int id) =>
        DatabaseMock.dsBangLuong.FirstOrDefault(bl => bl.getMaBangLuong() == id);

    public BangLuong? GetByNhanVienVaKy(int maNV, int thang, int nam) =>
        DatabaseMock.dsBangLuong.FirstOrDefault(bl => bl.getMaNhanVien() == maNV && bl.getKyThang() == thang && bl.getKyNam() == nam);

    public List<BangLuong> GetByKy(int thang, int nam) =>
        DatabaseMock.dsBangLuong.Where(bl => bl.getKyThang() == thang && bl.getKyNam() == nam).ToList();

    public List<BangLuong> GetTopThuNhap(int thang, int nam, int limit) =>
        DatabaseMock.dsBangLuong
            .Where(bl => bl.getKyThang() == thang && bl.getKyNam() == nam)
            .OrderByDescending(bl => bl.getThucLanh())
            .Take(limit)
            .ToList();

    public void Add(BangLuong entity) => DatabaseMock.dsBangLuong.Add(entity);

    public void Update(BangLuong entity)
    {
        int idx = DatabaseMock.dsBangLuong.FindIndex(bl => bl.getMaBangLuong() == entity.getMaBangLuong());
        if (idx >= 0) DatabaseMock.dsBangLuong[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsBangLuong.RemoveAll(bl => bl.getMaBangLuong() == id) > 0;
}
