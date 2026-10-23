using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.dao;

public class DonXinNghiDAO : IDAO<DonXinNghi, int>
{
    public List<DonXinNghi> GetAll() => DatabaseMock.dsDonXinNghi;

    public DonXinNghi? GetById(int id) =>
        DatabaseMock.dsDonXinNghi.FirstOrDefault(dn => dn.getMaDonNghi() == id);

    public List<DonXinNghi> GetChoDuyet() =>
        DatabaseMock.dsDonXinNghi.Where(dn => dn.getTrangThaiDuyet() == TrangThaiDuyet.CHO_DUYET).ToList();

    public List<DonXinNghi> GetByNhanVien(int maNV) =>
        DatabaseMock.dsDonXinNghi.Where(dn => dn.getMaNhanVien() == maNV).ToList();

    public void Add(DonXinNghi entity) => DatabaseMock.dsDonXinNghi.Add(entity);

    public void Update(DonXinNghi entity)
    {
        int idx = DatabaseMock.dsDonXinNghi.FindIndex(dn => dn.getMaDonNghi() == entity.getMaDonNghi());
        if (idx >= 0) DatabaseMock.dsDonXinNghi[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsDonXinNghi.RemoveAll(dn => dn.getMaDonNghi() == id) > 0;
}
