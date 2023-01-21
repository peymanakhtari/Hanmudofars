namespace hanmudo.Models
{
    public class Teknik
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Text { get; set; }
        public int BeltId { get; set; }
        public Belt Belt { get; set; }
        public ICollection<CategoryTeknik> CategoryTeknik { get; set; }


    }
}
