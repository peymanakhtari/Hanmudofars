namespace hanmudo.Models
{
    public class ContentTeknik
    {
        public int Id { get; set; }
        public int  Type { get; set; }
        public string  Text { get; set; }
        public byte[] img { get; set; }
        public int Sequence { get; set; }
        public int CategoryTeknikId { get; set; }
        public CategoryTeknik CategoryTeknik { get; set; }
    }
}
