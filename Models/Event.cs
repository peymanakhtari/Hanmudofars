namespace hanmudo.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string LongText { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string alt1 { get; set; }
        public string alt2 { get; set; }
        public string Video { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public string Link4 { get; set; }
        public string NameLink1 { get; set; }
        public string NameLink2 { get; set; }
        public DateTime DateTime { get; set; }
        public int Sequence { get; set; }
        public bool MainPage { get; set; }

    }
}
