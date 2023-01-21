using hanmudo.Models;
using Microsoft.EntityFrameworkCore;

namespace hanmudo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<KeyValue> KeyValues { get; set; }
        public DbSet<ContentTeknik> ContentTekniks { get; set; }
        public DbSet<Teknik> Tekniks { get; set; }
        public DbSet<Belt> Belts { get; set; }
        public DbSet<Info> Infos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<CategoryTeknik> CategoryTekniks { get; set; }
        public DbSet<Darian> Darians { get; set; }
        public DbSet<SelfDefence> SelfDefences { get; set; }
        public DbSet<RoleContent> RoleContents { get; set; }
        public DbSet<InfoUser> infoUsers { get; set; }
        public DbSet<SeenInfoUser> seenInfoUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Hanmudo;Integrated Security=True");
        }
    }
}
