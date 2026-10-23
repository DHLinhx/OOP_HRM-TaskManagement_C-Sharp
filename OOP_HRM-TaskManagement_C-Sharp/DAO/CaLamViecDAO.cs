using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class CaLamViecDAO : IDAO<CaLamViec, int>
{
    public List<CaLamViec> GetAll() => DatabaseMock.dsCaLamViec;

    public CaLamViec? GetById(int id) =>
        DatabaseMock.dsCaLamViec.FirstOrDefault(ca => ca.getMaCa() == id);

    public string GetTenCa(int maCa) =>
        GetById(maCa)?.getTenCa() ?? $"Ca {maCa}";

    public void Add(CaLamViec entity) => DatabaseMock.dsCaLamViec.Add(entity);

    public void Update(CaLamViec entity)
    {
        int idx = DatabaseMock.dsCaLamViec.FindIndex(ca => ca.getMaCa() == entity.getMaCa());
        if (idx >= 0) DatabaseMock.dsCaLamViec[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsCaLamViec.RemoveAll(ca => ca.getMaCa() == id) > 0;
}
