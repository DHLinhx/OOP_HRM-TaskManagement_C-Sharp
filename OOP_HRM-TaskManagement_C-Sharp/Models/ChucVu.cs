namespace com.hrmis.models;

public class ChucVu
{
    public int? maChucVu { get; set; }
    public string tenChucVu { get; set; } = string.Empty;

    public ChucVu() { }

    public ChucVu(int? maChucVu, string tenChucVu)
    {
        this.maChucVu = maChucVu;
        this.tenChucVu = tenChucVu;
    }

    public int? getmaChucVu() => maChucVu;
    public void setmaChucVu(int? maChucVu) => this.maChucVu = maChucVu;
    public string gettenChucVu() => tenChucVu;
    public void settenChucVu(string tenChucVu) => this.tenChucVu = tenChucVu;

    public override string ToString() => $"Chuc Vu{{maChucVu='{maChucVu}'ten Chuc Vu:'{tenChucVu}";
}
