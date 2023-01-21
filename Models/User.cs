namespace hanmudo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public int belt { get; set; }
        public DateTime Expire { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ConfirmRules { get; set; }  
        public DateTime CreationDate { get; set; }
        public ICollection<SeenInfoUser> seenInfoUsers { get; set; }

    }
}
