using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class PhongBanDAO : IDAO<PhongBan, int>
{
    public List<PhongBan> GetAll() => DatabaseMock.dsPhongBan;

    public PhongBan? GetById(int id) =>
        DatabaseMock.dsPhongBan.FirstOrDefault(pb => pb.getmaPhongBan() == id);

    public string GetTenPhongBan(int maPB) =>
        GetById(maPB)?.gettenPhongBan() ?? $"Phòng {maPB}";

    public void Add(PhongBan entity) => DatabaseMock.dsPhongBan.Add(entity);

    public void Update(PhongBan entity)
    {
        int idx = DatabaseMock.dsPhongBan.FindIndex(pb => pb.getmaPhongBan() == entity.getmaPhongBan());
        if (idx >= 0) DatabaseMock.dsPhongBan[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsPhongBan.RemoveAll(pb => pb.getmaPhongBan() == id) > 0;
}
