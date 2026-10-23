using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.dao;

public class ChamCongDAO : IDAO<ChamCong, int>
{
    public List<ChamCong> GetAll() => DatabaseMock.dsChamCong;

    public ChamCong? GetById(int id) =>
        DatabaseMock.dsChamCong.FirstOrDefault(cc => cc.getMaChamCong() == id);

    public List<ChamCong> GetByNhanVienVaThang(int maNV, int thang, int nam) =>
        DatabaseMock.dsChamCong
            .Where(cc => cc.getMaNhanVien() == maNV
                      && cc.getNgayLamViec().Month == thang
                      && cc.getNgayLamViec().Year == nam)
            .ToList();

    public int CountDiTre(int maNV, int thang, int nam) =>
        DatabaseMock.dsChamCong.Count(cc => cc.getMaNhanVien() == maNV
                                         && cc.getNgayLamViec().Month == thang
                                         && cc.getNgayLamViec().Year == nam
                                         && cc.getTrangThaiCong() == TrangThaiCong.DI_TRE);

    public double SumGioOT(int maNV, int thang, int nam) =>
        DatabaseMock.dsChamCong
            .Where(cc => cc.getMaNhanVien() == maNV
                      && cc.getNgayLamViec().Month == thang
                      && cc.getNgayLamViec().Year == nam)
            .Sum(cc => cc.getSoGioLamThem());

    public bool IsChuyenCan100PhanTram(int maNV, int thang, int nam)
    {
        int tong = DatabaseMock.dsChamCong.Count(cc => cc.getMaNhanVien() == maNV
                                                    && cc.getNgayLamViec().Month == thang
                                                    && cc.getNgayLamViec().Year == nam);
        int dung = DatabaseMock.dsChamCong.Count(cc => cc.getMaNhanVien() == maNV
                                                    && cc.getNgayLamViec().Month == thang
                                                    && cc.getNgayLamViec().Year == nam
                                                    && cc.getTrangThaiCong() == TrangThaiCong.DUNG_GIO);
        return tong > 0 && tong == dung;
    }

    public void Add(ChamCong entity) => DatabaseMock.dsChamCong.Add(entity);

    public void Update(ChamCong entity)
    {
        int idx = DatabaseMock.dsChamCong.FindIndex(cc => cc.getMaChamCong() == entity.getMaChamCong());
        if (idx >= 0) DatabaseMock.dsChamCong[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsChamCong.RemoveAll(cc => cc.getMaChamCong() == id) > 0;
}
