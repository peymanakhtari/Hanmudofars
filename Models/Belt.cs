namespace hanmudo.Models
{
    public class Belt
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string  Text { get; set; }
        public string Img { get; set; }
        public ICollection<Teknik> Tekniks { get; set; }
    }
}
