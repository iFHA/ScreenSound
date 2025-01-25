using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    public class ScreenSoundContext : DbContext
    {
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Musica> Musicas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        private string ConnectionString = "Data Source=localhost;Initial Catalog=ScreenSound;User ID=sa;Password={sua_senha};TrustServerCertificate=True";
        public ScreenSoundContext(DbContextOptions options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder
            .UseSqlServer(ConnectionString)
            .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Musica>()
            .HasMany(musica => musica.Generos)
            .WithMany(genero => genero.Musicas);
        }
    }
}