namespace hanmudo.Models
{
    public class CategoryTeknik
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int TeknikId { get; set; }
        public int Sequence { get; set; }
        public Teknik Teknik { get; set; }
        public ICollection<ContentTeknik> ContentTekniks { get; set; }
    }

}
