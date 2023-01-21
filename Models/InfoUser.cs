namespace hanmudo.Models
{
    public class InfoUser
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public int belt { get; set; }
        public DateTime datetime { get; set; }
        public int Sequence { get; set; }
        public bool Show { get; set; }
        public ICollection<SeenInfoUser> SeenInfoUsers { get; set; }
    }
}
