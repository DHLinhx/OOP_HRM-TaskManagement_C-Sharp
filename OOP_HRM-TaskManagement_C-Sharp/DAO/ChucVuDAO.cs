using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class ChucVuDAO : IDAO<ChucVu, int>
{
    public List<ChucVu> GetAll() => DatabaseMock.dsChucVu;

    public ChucVu? GetById(int id) =>
        DatabaseMock.dsChucVu.FirstOrDefault(cv => cv.getmaChucVu() == id);

    public string GetTenChucVu(int maCV) =>
        GetById(maCV)?.gettenChucVu() ?? $"Chức vụ {maCV}";

    public void Add(ChucVu entity) => DatabaseMock.dsChucVu.Add(entity);

    public void Update(ChucVu entity)
    {
        int idx = DatabaseMock.dsChucVu.FindIndex(cv => cv.getmaChucVu() == entity.getmaChucVu());
        if (idx >= 0) DatabaseMock.dsChucVu[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsChucVu.RemoveAll(cv => cv.getmaChucVu() == id) > 0;
}
