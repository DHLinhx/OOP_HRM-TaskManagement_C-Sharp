namespace com.hrmis.models;

public class CaLamViec
{
    public int MaCa { get; set; }
    public string TenCa { get; set; } = string.Empty;
    public TimeOnly GioBatDau { get; set; }
    public TimeOnly GioKetThuc { get; set; }

    public CaLamViec() { }

    public CaLamViec(int maCa, string tenCa, TimeOnly gioBatDau, TimeOnly gioKetThuc)
    {
        MaCa = maCa;
        TenCa = tenCa;
        GioBatDau = gioBatDau;
        GioKetThuc = gioKetThuc;
    }

    public int getMaCa() => MaCa;
    public string getTenCa() => TenCa;
    public TimeOnly getGioBatDau() => GioBatDau;
    public TimeOnly getGioKetThuc() => GioKetThuc;
}
