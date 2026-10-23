using com.hrmis.data;
using com.hrmis.enums;
using com.hrmis.models;

namespace com.hrmis.dao;

public class HopDongDAO : IDAO<HopDong, int>
{
    public List<HopDong> GetAll() => DatabaseMock.dsHopDong;

    public HopDong? GetById(int id) =>
        DatabaseMock.dsHopDong.FirstOrDefault(hd => hd.getMaHopDong() == id);

    public HopDong? GetByNhanVien(int maNV) =>
        DatabaseMock.dsHopDong.FirstOrDefault(hd => hd.getMaNhanVien() == maNV);

    public List<HopDong> GetSapHetHan(DateOnly mocThoiGian, int soNgay)
    {
        DateOnly mocGioiHan = mocThoiGian.AddDays(soNgay);
        return DatabaseMock.dsHopDong
            .Where(hd => hd.getNgayHetHan().HasValue
                      && hd.getNgayHetHan().Value >= mocThoiGian
                      && hd.getNgayHetHan().Value <= mocGioiHan)
            .ToList();
    }

    public List<HopDong> GetByLoai(LoaiHopDong loai) =>
        DatabaseMock.dsHopDong.Where(hd => hd.getLoaiHopDong() == loai).ToList();

    public void Add(HopDong entity) => DatabaseMock.dsHopDong.Add(entity);

    public void Update(HopDong entity)
    {
        int idx = DatabaseMock.dsHopDong.FindIndex(hd => hd.getMaHopDong() == entity.getMaHopDong());
        if (idx >= 0) DatabaseMock.dsHopDong[idx] = entity;
    }

    public bool Delete(int id) =>
        DatabaseMock.dsHopDong.RemoveAll(hd => hd.getMaHopDong() == id) > 0;
}
