namespace RehberApi.Model
{
    public class TblRehberYetkili
    {
        public int RehberYetkiliID { get; set; }
        public int RehberID { get; set; }
        public string YetkiliAdi { get; set; }
        public string GSM { get; set; }
        public string Mail { get; set; }
    }
}
