using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class ChiTietBangLuongDAO : IDAO<ChiTietBangLuong, int>
{
    public List<ChiTietBangLuong> GetAll() => DatabaseMock.dsChiTietBangLuong;

    public ChiTietBangLuong? GetById(int id) =>
        DatabaseMock.dsChiTietBangLuong.FirstOrDefault(ct => ct.getMaChiTietLuong() == id);

    public List<ChiTietBangLuong> GetByBangLuong(int maBangLuong) =>
        DatabaseMock.dsChiTietBangLuong.Where(ct => ct.getMaBangLuong() == maBangLuong).ToList();

    public void Add(ChiTietBangLuong entity) => DatabaseMock.dsChiTietBangLuong.Add(entity);

    public void Update(ChiTietBangLuong entity)
    {
        int idx = DatabaseMock.dsChiTietBangLuong.FindIndex(ct => ct.getMaChiTietLuong() == entity.getMaChiTietLuong());
        if (idx >= 0) DatabaseMock.dsChiTietBangLuong[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsChiTietBangLuong.RemoveAll(ct => ct.getMaChiTietLuong() == id) > 0;
}
