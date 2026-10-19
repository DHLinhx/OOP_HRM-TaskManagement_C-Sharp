namespace com.hrmis.models;

public class PhongBan
{
    public int? maPhongBan { get; set; }
    public string tenPhongBan { get; set; } = string.Empty;
    public int? maTruongPhong { get; set; }
    public string ViTri { get; set; } = string.Empty;

    public PhongBan() { }

    public PhongBan(int? maPhongBan, string tenPhongBan, int? maTruongPhong, string ViTri)
    {
        this.maPhongBan = maPhongBan;
        this.tenPhongBan = tenPhongBan;
        this.maTruongPhong = maTruongPhong;
        this.ViTri = ViTri;
    }

    public int? getmaPhongBan() => maPhongBan;
    public void setmaPhongBan(int? maPhongBan) => this.maPhongBan = maPhongBan;
    public string gettenPhongBan() => tenPhongBan;
    public void settenPhongBan(string tenPhongBan) => this.tenPhongBan = tenPhongBan;
    public int? getmaTruongPhong() => maTruongPhong;
    public void setmaTruongPhong(int? maTruongPhong) => this.maTruongPhong = maTruongPhong;
    public string getViTri() => ViTri;
    public void setViTri(string ViTri) => this.ViTri = ViTri;

    public override string ToString() => $"ma Phong Ban: {maPhongBan} ,ten Phong ban: {tenPhongBan} ,ma Truong Phong: {maTruongPhong}, Vi Tri: {ViTri}";
}
