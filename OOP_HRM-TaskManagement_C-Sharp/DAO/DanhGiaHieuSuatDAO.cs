using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.dao;

public class DanhGiaHieuSuatDAO : IDAO<DanhGiaHieuSuat, int>
{
    public List<DanhGiaHieuSuat> GetAll() => DatabaseMock.dsDanhGiaHieuSuat;

    public DanhGiaHieuSuat? GetById(int id) =>
        DatabaseMock.dsDanhGiaHieuSuat.FirstOrDefault(dg => dg.getMaDanhGia() == id);

    public List<DanhGiaHieuSuat> GetByNam(int nam) =>
        DatabaseMock.dsDanhGiaHieuSuat.Where(dg => dg.getNam() == nam).ToList();

    public List<DanhGiaHieuSuat> GetByXepLoai(int nam, XepLoai xl) =>
        DatabaseMock.dsDanhGiaHieuSuat
            .Where(dg => dg.getNam() == nam && dg.getXepLoai() == xl)
            .OrderByDescending(dg => dg.getDiemTrungBinh())
            .ToList();

    public void Add(DanhGiaHieuSuat entity) => DatabaseMock.dsDanhGiaHieuSuat.Add(entity);

    public void Update(DanhGiaHieuSuat entity)
    {
        int idx = DatabaseMock.dsDanhGiaHieuSuat.FindIndex(dg => dg.getMaDanhGia() == entity.getMaDanhGia());
        if (idx >= 0) DatabaseMock.dsDanhGiaHieuSuat[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsDanhGiaHieuSuat.RemoveAll(dg => dg.getMaDanhGia() == id) > 0;
}
