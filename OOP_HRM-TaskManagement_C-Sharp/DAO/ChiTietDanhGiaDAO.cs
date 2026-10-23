using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class ChiTietDanhGiaDAO : IDAO<ChiTietDanhGia, int>
{
    public List<ChiTietDanhGia> GetAll() => DatabaseMock.dsChiTietDanhGia;

    public ChiTietDanhGia? GetById(int id) =>
        DatabaseMock.dsChiTietDanhGia.FirstOrDefault(ct => ct.getMaChiTietDanhGia() == id);

    public List<ChiTietDanhGia> GetByDanhGia(int maDanhGia) =>
        DatabaseMock.dsChiTietDanhGia.Where(ct => ct.getMaDanhGia() == maDanhGia).ToList();

    public void Add(ChiTietDanhGia entity) => DatabaseMock.dsChiTietDanhGia.Add(entity);

    public void Update(ChiTietDanhGia entity)
    {
        int idx = DatabaseMock.dsChiTietDanhGia.FindIndex(ct => ct.getMaChiTietDanhGia() == entity.getMaChiTietDanhGia());
        if (idx >= 0) DatabaseMock.dsChiTietDanhGia[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsChiTietDanhGia.RemoveAll(ct => ct.getMaChiTietDanhGia() == id) > 0;
}
