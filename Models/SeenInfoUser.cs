namespace hanmudo.Models
{
    public class SeenInfoUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InfoUserId { get; set; }
        public User  User{ get; set; }
        public InfoUser InfoUser { get; set; }
    }
}
