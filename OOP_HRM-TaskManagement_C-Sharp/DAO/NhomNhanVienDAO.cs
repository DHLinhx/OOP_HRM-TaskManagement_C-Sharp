using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class NhomNhanVienDAO : IDAO<NhomNhanVien, int>
{
    public List<NhomNhanVien> GetAll() => DatabaseMock.dsNhomNhanVien;

    public NhomNhanVien? GetById(int id) =>
        DatabaseMock.dsNhomNhanVien.FirstOrDefault(nhom => nhom.getmaNhom() == id);

    public string GetTenNhom(int maNhom) =>
        GetById(maNhom)?.gettenNhom() ?? $"Nhóm {maNhom}";

    public void Add(NhomNhanVien entity) => DatabaseMock.dsNhomNhanVien.Add(entity);

    public void Update(NhomNhanVien entity)
    {
        int idx = DatabaseMock.dsNhomNhanVien.FindIndex(nhom => nhom.getmaNhom() == entity.getmaNhom());
        if (idx >= 0) DatabaseMock.dsNhomNhanVien[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsNhomNhanVien.RemoveAll(nhom => nhom.getmaNhom() == id) > 0;
}
