using com.hrmis.data;
using com.hrmis.models;

namespace com.hrmis.dao;

public class DieuKhoanHopDongDAO : IDAO<DieuKhoanHopDong, int>
{
    public List<DieuKhoanHopDong> GetAll() => DatabaseMock.dsDieuKhoanHopDong;

    public DieuKhoanHopDong? GetById(int id) =>
        DatabaseMock.dsDieuKhoanHopDong.FirstOrDefault(dk => dk.getMaDieuKhoan() == id);

    public List<DieuKhoanHopDong> GetByHopDong(int maHopDong) =>
        DatabaseMock.dsDieuKhoanHopDong.Where(dk => dk.getMaHopDong() == maHopDong).ToList();

    public List<DieuKhoanHopDong> GetByLoaiDieuKhoan(params string[] loais) =>
        DatabaseMock.dsDieuKhoanHopDong
            .Where(dk => !string.IsNullOrEmpty(dk.getLoaiDieuKhoan())
                      && loais.Any(l => dk.getLoaiDieuKhoan().Equals(l, StringComparison.OrdinalIgnoreCase)))
            .ToList();

    public void Add(DieuKhoanHopDong entity) => DatabaseMock.dsDieuKhoanHopDong.Add(entity);

    public void Update(DieuKhoanHopDong entity)
    {
        int idx = DatabaseMock.dsDieuKhoanHopDong.FindIndex(dk => dk.getMaDieuKhoan() == entity.getMaDieuKhoan());
        if (idx >= 0) DatabaseMock.dsDieuKhoanHopDong[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsDieuKhoanHopDong.RemoveAll(dk => dk.getMaDieuKhoan() == id) > 0;
}
