using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class ThanhVienNhomDAO : IDAO<ThanhVienNhom, string>
{
    public List<ThanhVienNhom> GetAll() => DatabaseMock.dsThanhVienNhom;

    public ThanhVienNhom? GetById(string compositeKey)
    {
        var parts = compositeKey.Split('_');
        if (parts.Length != 2) return null;
        int maNhom = int.Parse(parts[0]);
        int maNV = int.Parse(parts[1]);
        return DatabaseMock.dsThanhVienNhom
            .FirstOrDefault(tv => tv.getmaNhom().HasValue && tv.getmaNhom().Value == maNhom && tv.getnhomNhanVien() == maNV);
    }

    public List<ThanhVienNhom> GetByNhom(int maNhom) =>
        DatabaseMock.dsThanhVienNhom.Where(tv => tv.getmaNhom().HasValue && tv.getmaNhom().Value == maNhom).ToList();

    public List<ThanhVienNhom> GetByNhanVien(int maNV) =>
        DatabaseMock.dsThanhVienNhom.Where(tv => tv.getnhomNhanVien() == maNV).ToList();

    public void Add(ThanhVienNhom entity) => DatabaseMock.dsThanhVienNhom.Add(entity);

    public void Update(ThanhVienNhom entity)
    {
        int idx = DatabaseMock.dsThanhVienNhom.FindIndex(tv => tv.getmaNhom() == entity.getmaNhom() && tv.getnhomNhanVien() == entity.getnhomNhanVien());
        if (idx >= 0) DatabaseMock.dsThanhVienNhom[idx] = entity;
    }

    public bool Delete(string compositeKey)
    {
        var parts = compositeKey.Split('_');
        if (parts.Length != 2) return false;
        int maNhom = int.Parse(parts[0]);
        int maNV = int.Parse(parts[1]);
        return DatabaseMock.dsThanhVienNhom.RemoveAll(tv => tv.getmaNhom() == maNhom && tv.getnhomNhanVien() == maNV) > 0;
    }
}
